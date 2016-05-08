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

		/* Density */
		private const double AIR_P = 1.18;
		/* Viscosity */
		private const double AIR_u = 1.002;

		private List<ParticleSystem> particleSystems = new List<ParticleSystem> ();
		private List<Entity> entities = new List<Entity> ();
		private List<ParticleMesh> particleMeshes = new List<ParticleMesh> ();
		private List<List<Particle>> ropes = new List<List<Particle>> ();
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
		public void AddParticleSystem (PE.ParticleSystem particleSystem)
		{
			particleSystems.Add (particleSystem);
		}

		public void AddParticleMesh (ParticleMesh mesh)
		{
			particleMeshes.Add (mesh);
		}

		public void AddRope (List<Particle> rope)
		{
			ropes.Add (rope);
		}

		public void AddEntity (Entity entity)
		{
			entities.Add (entity);
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
				intersections.Clear ();
				CheckCollisions (particles);
				HandleCollisions ();
				AdjustIntersections ();
			}

			RopeUpdate (Time.fixedDeltaTime);

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
					spring.p1.f.Add (-f);
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

                double k = 1; /* Spring constant for system */
                double d = 3; /* Number of timesteps to stabilize the constraint */

                var n = rope.Count;

				// Constraint matrix
				var G = new Matrix<Vec3> (n-1, n);

				// Set G to zero
				//for (int i = 0; i < G.Rows; i++) {
				//	for (int j = 0; j < G.Cols; j++) {
				//		G [i, j] = new Vec3 ();
				//	}
				//}

				// Set constraints
				for (int i = 0; i < n - 1; i++) {
					Particle pi = rope [i];
					Particle pj = rope [i + 1];
					Vec3 u = (pi.x - pj.x).UnitVector;
					G [i, i] = u;
					G [i, i + 1] = -u;
				}

				// Mass matrix
				var Minv = new Matrix<Vec3> (n, n);

				// Set M to zero
				//for (int i = 0; i < Minv.Rows; i++) {
				//	for (int j = 0; j < Minv.Cols; j++) {
				//		Minv [i, j] = new Vec3 ();
				//	}
				//}

				// All forces
				var f = new Vector<Vec3>(n);
				// All velocities
				var W = new Vector<Vec3>(n);
                // All positions
                var q = new Vector<Vec3>(n);

				// Set values to M, f, W
				for (int i = 0; i < n; i++) {
					Minv [i, i] = new Vec3 (rope [i].m_inv);
					f [i] = rope [i].f;
					W [i] = rope [i].v;
                    q[i] = rope[i].x;
				}

                /* Constant parameters in SPOOK */
                double a = 4 / (dt * (1 + 4 * d));
                double b = (4 * d) / (1 + 4 * d);
                double e = 4 / (dt * dt * k * (1 + 4 * d));

                Matrix<Vec3> S = G * Minv * G.Transpose;
                for (int i = 0; i < S.Rows; i++)
                {
                    S[i, i] *= e;
                }

                Vector<Vec3> B = -a * (G * q) - b * (G * W) - dt * (G * (Minv * f));

                // Solve for lambda
                Vector<Vec3> lambda = Solver.GaussSeidel(S, B, 3);
                //Debug.Log(lambda);

                // Integrate
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
				intersections.Clear ();
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
			foreach (Entity entity in entities) {
				intersections.AddRange (entity.Collider.Collides (particles));
			}
		}

		private void HandleCollisions ()
		{
			foreach (Intersection data in intersections) {
				double e = 0.8;
				var v = data.particle.v;
				data.particle.v = v - (1 + e) * Vec3.Dot (v, data.normal) * data.normal;
			}
		}

		private void AdjustIntersections ()
		{
			/* Adjust collided particles */
			foreach (Intersection data in intersections) {
				var p = data.particle;
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
