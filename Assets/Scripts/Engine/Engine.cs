using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PE
{
	public class Engine : MonoBehaviour
	{
		public static Engine instance = null;

		private const float clothFrameRate = 300f;
		private int clothTimeSteps;
		private double clothDeltaTime;

		private readonly Vec3 g = new Vec3 (0, -9.82, 0);

		public Vector3 wind = new Vector3 (0, 0, 0);
		public uint solver_iterations = 10;

		private const double AIR_P = 1.18;
		// Air density
		private const double AIR_u = 1.002;
		// Air viscosity

		private List<Entity> entities = new List<Entity> ();

		private new ParticleSystem particleSystem;
		private ParticleMesh cloth;
		private ParticleSystem rope;
		private List<Sphere> spheres;

		private List<Intersection> intersections = new List<Intersection> (10000);

		void Awake ()
		{
			if (instance == null)
				instance = this;
			else if (instance != this)
				Destroy (gameObject);

			//Sets this to not be destroyed when reloading scene
			DontDestroyOnLoad (gameObject);
		}

		void Start ()
		{
			clothTimeSteps = Mathf.CeilToInt (Time.fixedDeltaTime * clothFrameRate);
			clothDeltaTime = Time.fixedDeltaTime / clothTimeSteps;
		}

		/*
		 * Setters
		 */
		public void AddEntity (Entity entity)
		{
			entities.Add (entity);
		}

		public ParticleSystem ParticleSystem {
			set {
				particleSystem = value;
			}
		}

		public ParticleMesh Cloth {
			set {
				cloth = value;
			}
		}

		public ParticleSystem Rope {
			set {
				rope = value;
			}
		}

		public List<Sphere> Spheres {
			set {
				spheres = value;
			}
		}

		/*
		 * Main loop
		 */
		public void FixedUpdate ()
		{
			ClothUpdate (clothDeltaTime);
			RopeUpdate (Time.fixedDeltaTime);
			SpheresUpdate (Time.fixedDeltaTime);
			ParticleUpdate (Time.fixedDeltaTime);
		}

		public void ClothUpdate (double dt)
		{
			if (cloth == null)
				return;

			for (int step = 0; step < clothTimeSteps; step++) {
				// Reset forces
				foreach (var p in cloth) {
					p.f.SetZero ();					
				}

				// Compute spring forces
				foreach (var spring in cloth.Neighbors) {
					var f = spring.Force;
					spring.p2.f.Add (f);
					f.Negate ();
					spring.p1.f.Add (f);
				}

				for (int i = 0; i < cloth.Size; i++) {
					var p = cloth [i];

					// Add gravity
					p.f.Add (p.m * g);

					// Add air friction forces
					if (p.v.Length != 0) {
						double air_fric = -1e-2;
						var f_air = air_fric * Vec3.Dot (p.v, p.v) * p.v.UnitVector;
						p.f.Add (f_air);
					}

					// Integrate
					p.v.Add (dt * p.m_inv * p.f);
					p.x.Add (dt * p.v);
				}
			}

			CheckCollisions (cloth);
			HandleCollisions ();
			AdjustIntersections ();
		}

		private void RopeUpdate (double dt)
		{
			if (rope == null)
				return;
			
			// Add gravity
			foreach (var p in rope) {
				p.f.Set (p.m * g);
			}

			intersections.Clear ();
			CheckCollisions (rope);
			HandleCollisions ();

			//double k = 1000; /* Spring constant for system */
			double d = 3; /* Number of timesteps to stabilize the constraint */

			/* Constant parameters in SPOOK */
			double a = 4 / (dt * (1 + 4 * d));
			double b = (4 * d) / (1 + 4 * d);

			var n = rope.Count;
			List<Constraint> C = rope.constraints;
			// Constraint Jacobian matrix
			var G = new Vec3Matrix (C.Count, n);
			// Inverse Mass matrix
			var M_inv = new Vec3Matrix (n, n);

			// All forces, velocities, generalized positions
			var f = new Vec3Vector (n);
			var W = new Vec3Vector (n);
			var q = new Vec3Vector (C.Count);

			// Set Jacobians
			for (int i = 0; i < C.Count; i++) {
				var c = C [i];
				int body_i = c.body_i;
				int body_j = c.body_j;

				Vec3[] jac = c.getJacobians (rope);
				G [i, body_i] = jac [0];
				G [i, body_j] = jac [1];
			}

			// Set constraints q
			for (int i = 0; i < C.Count; i++) {
				var c = C [i];
				Vec3 g = c.getConstraint (rope);
				q [i] = g;
			}

			// Set values to M, f, W
			for (int i = 0; i < n; i++) {
				M_inv [i, i] = new Vec3 (rope [i].m_inv);
				f [i] = rope [i].f;
				W [i] = rope [i].v;
			}
				
			Matrix<Vec3> S = G * M_inv * G.Transpose;

			for (int i = 0; i < S.Rows; i++) {
				var e = 4 / (dt * dt * C [i].k * (1 + 4 * d));
				S [i, i].Add (e);
			}
			Vec3Vector B = -a * q - b * (G * W) - dt * (G * (M_inv * f));

			// Solve for lambda
			Vec3Vector lambda = Solver.GaussSeidel (S, B, solver_iterations);
            
			var fc = G.Transpose * lambda;

			// Integrate
			for (int i = 0; i < n; i++) {
				Particle p = rope [i];
				p.v = p.v + p.m_inv * fc [i] + dt * p.m_inv * p.f;
				p.x = p.x + dt * p.v;
			}

			AdjustIntersections ();
		}

		private void SpheresUpdate (double dt)
		{
			if (spheres == null)
				return;

			var intersectionData = new List<Intersection> ();

			var contactObjects = new List<Entity> ();
			var contactIntersectionData = new List<Intersection> ();
			var contactCollisionMatrices = new List<Mat3> ();

			for (int i = 0; i < spheres.Count; i++) {
				Sphere sphere = spheres [i];
			
				// Add gravity
				sphere.f.Set (sphere.m * g);
				sphere.v.Add (0.5 * dt * sphere.m_inv * sphere.f);

				intersections.Clear ();

				for (int j = i + 1; j < spheres.Count; j++) {
					var data = sphere.Collider.Collides (spheres [j]);

					data.ForEach (each => {
						each.self = sphere;
					});

					intersections.AddRange (data);
				}

				foreach (Entity entity in entities) {
					entity.v.SetZero ();

					var data = entity.Collider.Collides (sphere);

					data.ForEach (each => {
						each.self = sphere;
					});

					intersections.AddRange (data);
				}

				intersectionData.AddRange (intersections);

				foreach (Intersection data in intersections) {
					const double e = 0.8;
					const double mu = 0.5;

					Entity other = data.entity;
					var u = sphere.v - other.v;
					var r = sphere.x - other.x;
					var u_n = Vec3.Dot (u, data.normal) * data.normal;//Vec3.Dot (u, r) * r.UnitVector;
					var r_a = sphere.x - data.point;
					var r_b = other.x - data.point;

					if (Vec3.Dot (u, data.normal) > 0)
						continue; /* Separating */

					var rax = Mat3.SkewSymmetric (r_a);
					var rbx = Mat3.SkewSymmetric (r_b);

					var I_a = sphere.I_inv;
					var I_b = other.I_inv;
					var M_a = sphere.m_inv;
					var M_b = other.m_inv;

					//Debug.Log("normal: " + data.normal);

					Mat3 K = M_a + M_b - (rax * I_a * rax.Transpose + rbx * I_b * rbx.Transpose);
					Vec3 J = (M_a + M_b).Inverse * (-e * u_n - u);
					sphere.K = K;
					contactCollisionMatrices.Add (K);


					var j_n = Vec3.Dot (J, data.normal) * data.normal;
					var j_t = J - j_n;

					bool in_allowed_friction_cone = j_t.Length <= mu * j_n.Length;

					if (!in_allowed_friction_cone) {
						Vec3 n = data.normal;
						Vec3 t = j_t.UnitVector;
						var j = -(1 + e) * u_n.Length / Vec3.Dot (n * K, n - mu * t);
						J = j * n - mu * j * t;
					}

					// FIXME changed the signs here
					sphere.v.Add (sphere.m_inv * J);
					sphere.omega.Add (sphere.I_inv * Vec3.Cross (r_a, J));
					other.v.Add (-other.m_inv * J);
					other.omega.Add (-other.I_inv * Vec3.Cross (r_b, J));

					//Debug.Log("J: " + J);

					//var normalVelocityDiff = (data.normal * (sphere.v - other.v)).Length;
					var contactTest = Vec3.Dot (data.normal, sphere.v);
					//Debug.Log("contactTest: " + contactTest);
					if (contactTest <= 0.1) { /* Resting contact */
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
					}
				}
			}

			for (int i = 0; i < spheres.Count; i++) {
				Sphere s = spheres [i];
				s.v.Add (0.5 * dt * s.m_inv * s.f);
			}

			if (contactObjects.Count > 0) {
				//Debug.Log("Contact");
				double d = 3;
				double k = 100;
				double a = 4 / (dt * (1 + 4 * d));
				double b = (4 * d) / (1 + 4 * d);

				var M = contactIntersectionData.Count;
				var N = contactObjects.Count;

				var G = new Vec3Matrix (M, N);
				var CollisionMatrix = new Mat3Matrix (M, M);
				var dW = new Vec3Vector (N);
				var W = new Vec3Vector (N);
				var q = new Vec3Vector (M);

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
					q [i] = contactIntersectionData [i].distance * contactIntersectionData [i].normal;
				}

				// Set values to M, f, W
				for (int i = 0; i < N; i++) {
					dW [i] = contactObjects [i].m_inv * contactObjects [i].f;
					W [i] = contactObjects [i].v;
				}

				var S = G.Transpose * (CollisionMatrix * G);

				for (int i = 0; i < S.Rows; i++) {
					var e = 4 / (dt * dt * k * (1 + 4 * d));
					S [i, i].Add (e);
				}

				Vec3Vector B = -a * q - b * (G * W) - dt * (G * dW);

				Vec3Vector lambda = Solver.GaussSeidel (S, B, solver_iterations);
				//Debug.Log("Lambda: " + lambda);
				/* If lambda has zero values, clamp to zero */
				for (int i = 0; i < lambda.Size; i++) {
					for (int j = 0; j < 3; j++) {
						if (lambda [i] [j] < 0) {
							lambda [i] [j] = 0; /*Debug.Log("Negative lambda");*/
						}
					}
				}

				var fc = G.Transpose * lambda;
				//Debug.Log("fc " + fc);
				//Debug.Log("fc: " + fc);
				for (int i = 0; i < contactObjects.Count; i++) {
					contactObjects [i].v.Add (contactObjects [i].m_inv * fc [i]); // Wat
				}
			}

			for (int i = 0; i < spheres.Count; i++) {
				Sphere s = spheres [i];
				s.x.Add (dt * s.v);
			}

		}

		private void ParticleUpdate (double dt)
		{
			if (particleSystem == null)
				return;
			
			/* Clear forces */
			foreach (var p in particleSystem) {
				p.f.SetZero ();
			}

			// Create particles at emitter
			// (Remove particles at sinks or when they expire in time)
			particleSystem.Update ();

			// Do inter-particle collision detection and construct a
			// neighbour list – or use a fixed interaction list (cloth).

			// Loop over neighbour lists and compute interaction forces.
			// Accumulate the forces.Use Newton’s third law.

			// Accumulate external forces from e.g.gravity.
			// Accumulate dissipative forces, e.g.drag and viscous drag.
			foreach (var p in particleSystem) {
				/* Gravity */
				p.f.Add (p.m * g);
                
				/* Air drag force */
				double kd = 0.18;
				Vec3 u = p.v - (Vec3)wind;
				Vec3 Fair = -kd * u;
				p.f.Add (Fair);
			}
				
			// Find contact sets with external boundaries, e.g.a plane.
			// Handle external boundary conditions by reflecting the
			// the velocities.
			CheckCollisions (particleSystem);
			HandleCollisions ();

			// Take a timestep and integrate using e.g.Verlet / Leap Frog
			foreach (var p in particleSystem) {
				p.v.Add (dt * p.m_inv * p.f);
				p.x.Add (dt * p.v);
			}

			/* 
            If there still are overlaps in the contact set with
            external boundaries, you could project the positions of
            the particles to the constraint manifold, e.g.to the
            surface of the plane. 
            */
			AdjustIntersections ();
		}

		private void CheckCollisions (IEnumerable<Particle> particles)
		{
			intersections.Clear ();

			foreach (Entity entity in entities) {
				intersections.AddRange (entity.Collider.Collides (particles));
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
			/* Adjust collided particles */
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



	}

}
