using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace PE
{
	public class Engine : MonoBehaviour
	{
		public static Engine instance = null;

		public float clothFrameRate = 300f;
		private int clothTimeSteps;
		private double clothDeltaTime;

		/*
		 * World parameters
		 */
		private readonly Vec3 g = new Vec3 (0, -9.82, 0);
		private const double AIR_P = 1.18;
		private const double AIR_u = 1.002;

		/*
		 * Visible in engine inspector
		 */
		public Vector3 wind = new Vector3 (0, 0, 0);
		public uint maxSolverIterations = 50;
		public IterationDirection iterationDirection = IterationDirection.Forward;
		public float mainLoopFrequency = 60f;
		public double timestepsToStabilizeConstraint = 3;
		public bool simulateCloth = true;
		public bool simulateParticleSystem = true;
		public bool simulateRope = true;
		public bool simulateSpheres = true;

		/*
		 * Engine internals
		 */
		private HGrid hgrid = new HGrid ();
		private List<Entity> entities = new List<Entity> ();
		private List<Intersection> intersections = new List<Intersection> (10000);

		/*
		 * Logging and data collection
		 */
		string ropeDataFile = "rope-convergence.txt";
		StreamWriter ropeData;
        string sphereDataFile = "sphere-convergence.txt";
        StreamWriter sphereData;
		int ropeTimeSteps = 0;
        int sphereTimeSteps = 0;
        const int timestepLimit = 500;

		void Awake ()
		{
			if (instance == null)
				instance = this;
			else if (instance != this)
				Destroy (gameObject);

			// Don't destroy when reloading scene
			DontDestroyOnLoad (gameObject);
		}

		void Start ()
		{
			Time.fixedDeltaTime = 1 / mainLoopFrequency;

			clothTimeSteps = Mathf.CeilToInt (Time.fixedDeltaTime * clothFrameRate);
			clothDeltaTime = Time.fixedDeltaTime / clothTimeSteps;

            File.Create (ropeDataFile).Close ();
            ropeData = File.AppendText (ropeDataFile);
            File.Create (sphereDataFile).Close ();
            sphereData = File.AppendText (sphereDataFile);
        }

        /*
		 * Properties
		 */
        public void AddEntity (Entity entity)
		{
			entities.Add (entity);
		}

		public ParticleSystem ParticleSystem {
			get;
			set;
		}

		public ParticleMesh Cloth {
			get;
			set;
		}

		public ParticleSystem Rope {
			get;
			set;
		}

		public List<Sphere> Spheres {
			get;
			set;
		}

		/*
		 * Main loop
		 */
		public void FixedUpdate ()
		{
			Time.fixedDeltaTime = 1 / mainLoopFrequency;

			if (simulateCloth)
				ClothUpdate (clothDeltaTime);

			if (simulateRope)
				RopeUpdate (Time.fixedDeltaTime);

			if (simulateSpheres)
				SpheresUpdate (Time.fixedDeltaTime);

			if (simulateParticleSystem)
				ParticleUpdate (Time.fixedDeltaTime);
		}

		public void ClothUpdate (double dt)
		{
			if (Cloth == null)
				return;

			for (int step = 0; step < clothTimeSteps; step++) {
				// Reset forces
				foreach (var p in Cloth) {
					p.f.SetZero ();					
				}

				// Compute spring forces
				foreach (var spring in Cloth.Neighbors) {
					var f = spring.Force;
					spring.p2.f.Add (f);
					f.Negate ();
					spring.p1.f.Add (f);
				}

				for (int i = 0; i < Cloth.Size; i++) {
					var p = Cloth [i];

					// Add gravity
					p.f.Add (p.m * g);

					// Add air friction forces
					if (p.v.SqLength > 0.00001) { // Arbitrary cutoff
						double air_fric = -1e-2;
						var f_air = air_fric * Vec3.Dot (p.v, p.v) * p.v.UnitVector;
						p.f.Add (f_air);
					}

					// Integrate
					p.v.Add (dt * p.m_inv * p.f);
					p.x.Add (dt * p.v);
				}
			}

			intersections.Clear ();
			CheckCollisions (Cloth);
			HandleCollisions ();
			AdjustIntersections ();
		}

		private void RopeUpdate (double dt)
		{
			if (Rope == null)
				return;

			// Data collection
			if (ropeTimeSteps < timestepLimit)
				ropeTimeSteps++;
			else if (ropeTimeSteps == timestepLimit) {
				ropeTimeSteps++;
				Debug.Log ("Rope sample complete! Timestep: " + 1 / Time.fixedDeltaTime + "Hz");
				ropeData.Close ();
				ropeData = null;
			}

			// Add gravity
			foreach (var p in Rope) {
				p.f.Set (p.m * g);
			}

			intersections.Clear ();
			CheckCollisions (Rope);
			HandleCollisions ();

			/* Constant parameters in SPOOK */
			double d = timestepsToStabilizeConstraint;
			double a = 4 / (dt * (1 + 4 * d));
			double b = (4 * d) / (1 + 4 * d);

			var N = Rope.Count;
			List<Constraint> C = Rope.constraints;

			var G = new Vec3Matrix (C.Count, N); // Constraint Jacobian matrix
			var M_inv = new Mat3Matrix (N, N); // Inverse Mass matrix

			// All forces, velocities, generalized positions
			var f = new Vec3Vector (N);
			var W = new Vec3Vector (N);
			var q = new Vec3Vector (C.Count);

			for (int i = 0; i < C.Count; i++) {
				// Set Jacobians
				var c = C [i];
				Vec3[] jac = c.getJacobians (Rope);
				G [i, c.body_i] = jac [0];
				G [i, c.body_j] = jac [1];

				// Set constraints q
				q [i] = c.getConstraint (Rope);
			}

			// Set values to M, f, W
			for (int i = 0; i < N; i++) {
				M_inv [i, i] = Mat3.Diag (Rope [i].m_inv);
				f [i] = Rope [i].f;
				W [i] = Rope [i].v;
			}

			Vec3Matrix S = G * M_inv * G.Transpose;

			for (int i = 0; i < S.Rows; i++) {
				var e = 4 / (dt * dt * C [i].k * (1 + 4 * d));
				S [i, i].Add (e);
			}

			Vec3Vector B = -a * q - b * (G * W) - dt * (G * (M_inv * f));

			Vec3Vector lambda = Solver.GaussSeidel (S, B, maxSolverIterations, iterationDirection, logFile: ropeData);
            
			Vec3Vector fc = G.Transpose * lambda;

			// Integrate
			for (int i = 0; i < N; i++) {
				Particle p = Rope [i];
				p.v = p.v + p.m_inv * fc [i] + dt * p.m_inv * p.f;
				p.x = p.x + dt * p.v;
			}

			AdjustIntersections ();
		}

		private void SpheresUpdate (double dt)
		{
			if (Spheres == null)
				return;

			var contactObjects = new List<Entity> ();
			var contactIntersectionData = new List<Intersection> ();
			var contactCollisionMatrices = new List<Mat3> ();

			/* Build up broad phase collision hash grid */
			var hgridObjects = AddSpheresToHGrid ();

			for (int i = 0; i < Spheres.Count; i++) {
				Sphere sphere = Spheres [i];

				// Add gravity and take a half timestep
				sphere.f.Set (sphere.m * g);
				sphere.v.Add (0.5 * dt * sphere.m_inv * sphere.f);

				intersections.Clear ();

				/* Broad phase collision detection againt other spheres */
				List<Sphere> possibleCollisions = hgrid.CheckObjAgainstGrid (hgridObjects [i]);
				hgrid.RemoveObject (hgridObjects [i]);

				foreach (Sphere other in possibleCollisions) {
					var data = sphere.Collider.CheckIntersection (other);

					foreach (var d in data) {
						d.self = sphere;
					}

					intersections.AddRange (data);
				}
					
				foreach (Entity entity in entities) {
					// FIXME not needed?
					// This is here so that the ground doesn't accumulate velocity
					entity.v.SetZero (); 

					var data = entity.Collider.CheckIntersection (sphere);

					foreach (var d in data) {
						d.self = sphere;
					}

					intersections.AddRange (data);
				}

				foreach (Intersection data in intersections) {
					const double e = 0.8;
					const double mu = 0.1;

					Entity other = data.entity;
					var u = sphere.v - other.v;
					var r = sphere.x - other.x;
					var u_n = Vec3.Dot (u, data.normal) * data.normal;
					var r_a = sphere.x - data.point;
					var r_b = other.x - data.point;

					/* Check if contact is a separating contact */
					if (Vec3.Dot (u, data.normal) > 0)
						continue;

					var rax = Mat3.SkewSymmetric (r_a);
					var rbx = Mat3.SkewSymmetric (r_b);

					var I_a = sphere.I_inv;
					var I_b = other.I_inv;
					var M_a = sphere.m_inv;
					var M_b = other.m_inv;

					Mat3 K = M_a + M_b;// - (rax * I_a * rax.Transpose + rbx * I_b * rbx.Transpose);
					Vec3 J = K.Inverse * (-e * u_n - u);

					var j_n = Vec3.Dot (J, data.normal) * data.normal;
					var j_t = J - j_n;

					bool in_allowed_friction_cone = j_t.SqLength < mu * mu * j_n.SqLength;

					if (!in_allowed_friction_cone) {
						Vec3 n = data.normal;
						Vec3 t = j_t.UnitVector;
						var j = -(1 + e) * u_n.Length / Vec3.Dot (n * K, n - mu * t);
						J = -j * n - mu * j * t;
					}

					sphere.v.Add (sphere.m_inv * J);
					sphere.omega.Add (sphere.I_inv * Vec3.Cross (r_a, J));
					other.v.Add (-other.m_inv * J);
					other.omega.Add (-other.I_inv * Vec3.Cross (r_b, J));
						
					var u_new = sphere.v - other.v;
					var contactTest = Vec3.Dot (data.normal, u_new);

					/* Check if contact is a resting contact */
					if (contactTest <= 0.3) {
						// Add to contact matrix
						if (!contactObjects.Contains (other))
							contactObjects.Add (other);

						if (!contactObjects.Contains (sphere))
							contactObjects.Add (sphere);

						data.i = contactObjects.FindIndex (s => {
							return s == sphere;
						});

						data.j = contactObjects.FindIndex (s => {
							return s == other;
						});

						contactIntersectionData.Add (data);
						contactCollisionMatrices.Add (K);
					}
				}

				// Add the other half of the time step
				sphere.v.Add (0.5 * dt * sphere.m_inv * sphere.f);
			}

			if (contactObjects.Count > 0) {
				double d = timestepsToStabilizeConstraint;
				double k = 1000;
				double a = 4 / (dt * (1 + 4 * d));
				double b = (4 * d) / (1 + 4 * d);

				var M = contactIntersectionData.Count;
				var N = contactObjects.Count;

				var G = new Vec3Matrix (M, N);
				var CollisionMatrix = new Mat3Matrix (M, M);
				var dW = new Vec3Vector (N);
				var W = new Vec3Vector (N);
				var q = new Vec3Vector (M);
				var M_inv = new Mat3Matrix (N, N);

				// Jacobian
				for (int i = 0; i < M; i++) {
					int body_i = contactIntersectionData [i].i;
					int body_j = contactIntersectionData [i].j;
					G [i, body_i] = contactIntersectionData [i].normal;
					G [i, body_j] = -contactIntersectionData [i].normal;
					CollisionMatrix [i, i] = contactCollisionMatrices [i];
				}

				// Set constraints q
				for (int i = 0; i < M; i++) {
					q [i] = -contactIntersectionData [i].distance * contactIntersectionData [i].normal;
				}

				// Set values to M, f, W
				for (int i = 0; i < N; i++) {
					dW [i] = contactObjects [i].m_inv * contactObjects [i].f;
					W [i] = contactObjects [i].v;
					M_inv [i, i] = contactObjects [i].m_inv;
				}

				Vec3Matrix S = G * M_inv * G.Transpose;

				for (int i = 0; i < S.Rows; i++) {
					var e = 4 / (dt * dt * k * (1 + 4 * d));
					S [i, i].Add (e);
				}

				Vec3Vector B = -a * q - b * (G * W) - dt * (G * dW);

                // Data collection
                if (sphereTimeSteps < timestepLimit)
                    sphereTimeSteps++;
                else if (sphereTimeSteps == timestepLimit) {
                    sphereTimeSteps++;
                    Debug.Log ("Spheres sample complete! Timestep: " + 1 / Time.fixedDeltaTime + "Hz");
                    sphereData.Close ();
                    sphereData = null;
                }

                Vec3Vector lambda = Solver.GaussSeidel (S, B, maxSolverIterations, logFile: sphereData);

				/* If lambda has negative values, clamp to zero */
				foreach (Vec3 elem in lambda) {
					for (int j = 0; j < 3; j++) {
						if (elem [j] < 0) {
							elem [j] = 0;
							//Debug.Log ("Negative lambda");
						}
					}
				}

				var fc = G.Transpose * lambda;

				for (int i = 0; i < contactObjects.Count; i++) {
					contactObjects [i].v.Add (contactObjects [i].m_inv * fc [i]);
				}
			}

			// Finally, integrate
			foreach (var sphere in Spheres) {
				sphere.x.Add (dt * sphere.v);
			}
		}

		private void ParticleUpdate (double dt)
		{
			if (ParticleSystem == null)
				return;
			
			// Clear forces
			foreach (var p in ParticleSystem) {
				p.f.SetZero ();
			}

			// Accumulate external forces from e.g.gravity.
			// Accumulate dissipative forces, e.g.drag and viscous drag.
			foreach (var p in ParticleSystem) {
				/* Gravity */
				p.f.Add (p.m * g);
				p.v.Add (0.5 * dt * p.m_inv * p.f);

				/* Air drag force */
				double kd = 0.18;
				Vec3 u = p.v - (Vec3)wind;
				Vec3 Fair = -kd * u;
				p.f.Add (Fair);
			}
				
			// Find contact sets with external boundaries
			CheckCollisions (ParticleSystem);
			HandleCollisions ();

			// Take a timestep and integrate
			foreach (var p in ParticleSystem) {
				p.v.Add (0.5 * dt * p.m_inv * p.f);
				p.x.Add (dt * p.v);
			}

			// Adjust collided particles
			AdjustIntersections ();
		}

		private void CheckCollisions (IEnumerable<Particle> particles)
		{
			intersections.Clear ();

			foreach (Entity entity in entities) {
				intersections.AddRange (entity.Collider.CheckIntersection (particles));
			}
		}

		private void HandleCollisions ()
		{
			foreach (Intersection data in intersections) {
				double e = 0.8;
				var v = data.entity.v;
				data.entity.v = v - (1 + e) * Vec3.Dot (v, data.normal) * data.normal;
			}
		}

		private void AdjustIntersections ()
		{
			foreach (Intersection data in intersections) {
				var p = data.entity as Particle;
				if (Vec3.Dot (p.v, data.normal) < 0) {
					p.v.SetZero ();
				}

				if (Vec3.Dot (p.x - data.point, data.normal) < 0) {
					p.x = data.point;
				}
			}
		}


		List<HGridObject> AddSpheresToHGrid ()
		{
			List<HGridObject> objects = new List<HGridObject> ();

			foreach (var sphere in Spheres) {
				HGridObject obj = new HGridObject (sphere, sphere.x, (float)sphere.r);
				hgrid.AddObject (obj);
				objects.Add (obj);
			}

			return objects;
		}

	}

}
