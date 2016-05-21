using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PE
{
	public class Engine : MonoBehaviour
	{
		public static Engine instance = null;

		private const float clothFrameRate = 400f;
		private int clothTimeSteps;
		private double clothDeltaTime;

		private readonly Vec3 g = new Vec3 (0, -9.82, 0);

		public Vector3 wind = new Vector3 (0, 0, 0);
		public uint solver_iterations = 10;

		private const double AIR_P = 1.18;
		// Air density
		private const double AIR_u = 1.002;
		// Air viscosity

		private List<ParticleSystem> particleSystems = new List<ParticleSystem> ();
		private List<Entity> entities = new List<Entity> ();
		private List<ParticleMesh> particleMeshes = new List<ParticleMesh> ();
		private List<ParticleSystem> ropes = new List<ParticleSystem> ();
		private List<Intersection> intersections = new List<Intersection> (10000);
		private List<Sphere> spheres;

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
		public void AddParticleSystem (PE.ParticleSystem particleSystem)
		{
			particleSystems.Add (particleSystem);
		}

		public void AddParticleMesh (ParticleMesh mesh)
		{
			particleMeshes.Add (mesh);
		}

		public void AddRope (ParticleSystem rope)
		{
			ropes.Add (rope);
		}

		public void AddEntity (Entity entity)
		{
			entities.Add (entity);
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
			for (int i = 0; i < clothTimeSteps; i++) {
				ClothUpdate (clothDeltaTime);
			}

			foreach (var particles in particleMeshes) {
				CheckCollisions (particles);
				HandleCollisions ();
				AdjustIntersections ();
			}

			RopeUpdate (Time.fixedDeltaTime);

			SpheresUpdate (Time.fixedDeltaTime);

			ParticleUpdate (Time.fixedDeltaTime);
		}

		public void ClothUpdate (double dt)
		{
			foreach (var particles in particleMeshes) {
				// Reset forces
				foreach (var p in particles) {
					p.f.SetZero ();					
				}

				// Compute spring forces
				foreach (var spring in particles.Neighbors) {
					var f = spring.Force;
					spring.p2.f.Add (f);
					f.Negate ();
					spring.p1.f.Add (f);
				}

				for (int i = 0; i < particles.Size; i++) {
					var p = particles [i];
					if (i == 0) {
						// TODO Add upwards force instead
						//continue;
					}

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
		}

		private void RopeUpdate (double dt)
		{
			foreach (var rope in ropes) {
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
				//var e = new Vec3(4 / (dt * dt * k * (1 + 4 * d)));

				var n = rope.Count;
				List<Constraint> C = rope.constraints;
				// Constraint Jacobian matrix
				var G = new Matrix<Vec3> (C.Count, n);
				// Inverse Mass matrix
				var M_inv = new Matrix<Vec3> (n, n);

				// All forces
				var f = new Vec3Vector (n);
				// All velocities
				var W = new Vec3Vector (n);
				// All generalized positions
				var q = new Vec3Vector (C.Count);

				// Set Jacobians
				//            for (int i = 0; i < n-1; i++) {
				//	Particle pi = rope [i];
				//	Particle pj = rope [i + 1];
				//	Vec3 u = (pi.x - pj.x).UnitVector;
				//	G [i, i] = u;   
				//	G [i, i+1] = -u;
				//}

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
				//var L = 2;
				//for (int i = 0; i < n-1; i++)
				//{
				//    Particle pi = rope[i];
				//    Particle pj = rope[i + 1];

				//    q[i] = new Vec3((pi.x - pj.x).SqLength - L); //0.5 * (Math.Pow ((pi.x - pj.x).SqLength, 2) - L);
				//}

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
				//uint max_iter = 10;
				Vec3Vector lambda = Solver.GaussSeidel (S, B, solver_iterations);
                
				var fc = G.Transpose * lambda;

				//Debug.Log("fc: " + fc + "\n");
				//Debug.Log(("lambda: " + lambda));
				//Debug.Log("GT: " + G.Transpose);

				// Integrate
				for (int i = 0; i < n; i++) {
					Particle p = rope [i];
					p.v = p.v + p.m_inv * fc [i] + dt * p.m_inv * p.f;
					p.x = p.x + dt * p.v;
				}

				AdjustIntersections ();
			}
		}

		private void SpheresUpdate (double dt)
		{
			if (spheres == null)
				return;

			for (int idx = 0; idx < spheres.Count; idx++) {
				Sphere body = spheres [idx];
			
				body.f.Set (body.m * g); // TODO remove

				intersections.Clear ();

				foreach (Entity entity in entities) {
					intersections.AddRange (entity.Collider.Collides (body));
				}

				// Collision with plane
				foreach (Intersection data in intersections) {
					const double e = 0.8;
					const double mu = 0.8;

					var other = data.entity;
					var u = other.v - body.v;
					var r = data.point - body.x;
					var u_n = Vec3.Dot (u, r) * r.UnitVector;
					var r_a = data.point - body.x;

                    var rax = Mat3.SkewSymmetric(r_a);

                    var I_a = body.I_inv;
					var M_a = body.m_inv;

					Mat3 K = M_a - rax * I_a * rax;
					Vec3 J = K.Inverse * (-e * u_n - u);

					var j_n = Vec3.Dot (J, data.normal) * data.normal;
					var j_t = J - j_n;
					bool in_allowed_friction_cone = j_t.Length <= mu * j_n.Length;

					if (!in_allowed_friction_cone) {
						Vec3 n = data.normal;
						Vec3 t = j_t.UnitVector;
						var j = -(1 + e) * u_n.Length / Vec3.Dot (n * K, n - mu * t);
						J = j * n - mu * j * t;
					}

					body.v.Add (body.m_inv * J);
					body.omega.Add (body.I_inv * Vec3.Cross (r_a, J));
				}

				intersections.Clear ();

				for (int j = idx + 1; j < spheres.Count; j++) {
					intersections.AddRange (body.Collider.Collides (spheres [j]));
				}

				foreach (Intersection data in intersections) {
					const double e = 0.8;
					const double mu = 0.8;

					Sphere other = data.entity as Sphere;
					var u = other.v - body.v;
					var r = other.x - body.x;
					var u_n = Vec3.Dot (u, r) * r.UnitVector;
					var r_a = data.point - body.x;
					var r_b = data.point - other.x;

					var rax = Mat3.SkewSymmetric(r_a);

					var rbx = Mat3.SkewSymmetric(r_b);

					var I_a = body.I_inv;
					var I_b = other.I_inv;
					var M_a = body.m_inv;
					var M_b = other.I_inv;

					Mat3 K = M_a + M_b - (rax * I_a * rax + rbx * I_b * rbx);
					Vec3 J = K.Inverse * (-e * u_n - u);

					var j_n = Vec3.Dot (J, data.normal) * data.normal;
					var j_t = J - j_n;
					bool in_allowed_friction_cone = j_t.Length <= mu * j_n.Length;

					if (!in_allowed_friction_cone) {
						Vec3 n = data.normal;
						Vec3 t = j_t.UnitVector;
						var j = -(1 + e) * u_n.Length / Vec3.Dot (n * K, n - mu * t);
						J = j * n - mu * j * t;
					}

					body.v.Add (body.m_inv * J);
					body.omega.Add (body.I_inv * Vec3.Cross (r_a, J));
					other.v.Add (-other.m_inv * J);
					other.omega.Add (-other.I_inv * Vec3.Cross (r_b, J));
				}

				if (idx == 1)
					continue;

				body.v.Add (dt * body.m_inv * body.f);
				body.x.Add (dt * body.v);

			}
		}

		private void ParticleUpdate (double dt)
		{
			foreach (var particleSystem in particleSystems) {

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
