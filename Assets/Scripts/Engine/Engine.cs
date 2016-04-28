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

		public Vector3 Wind = new Vector3 (0, 0, 0);
		private Vec3 wind = new Vec3 (0);

		/* Density */
		private const double AIR_P = 1.18;
		/* Viscosity */
		private const double AIR_u = 1.002;

		private List<ParticleSystem> particleSystems = new List<ParticleSystem> ();
		private List<Entity> entities = new List<Entity> ();
		private List<ParticleMesh> particleMeshes = new List<ParticleMesh> ();
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

		public void AddParticleSystem (PE.ParticleSystem ps)
		{
			particleSystems.Add (ps);
		}

		public void AddParticleMesh (ParticleMesh mesh)
		{
			particleMeshes.Add (mesh);
		}

		public void AddEntity (Entity e)
		{
			entities.Add (e);
		}
			
		// Use this for initialization
		void Start ()
		{
			clothTimeSteps = Mathf.CeilToInt (Time.fixedDeltaTime * clothFrameRate);
			clothDeltaTime = Time.fixedDeltaTime / clothTimeSteps;
		}

		public void FixedUpdate ()
		{

			for (int i = 0; i < clothTimeSteps; i++) {
				ClothUpdate (clothDeltaTime);
			}

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
						continue;
					}

					// Add gravity
					p.f.Add (p.m * g);

					// Add air friction forces
					if (p.v.Length != 0) {
						var f_air = -1e-2 * p.v.SqLength * p.v.UnitVector;
						p.f.Add (f_air);
					}

					// Integrate
					p.v.Add (dt * p.m_inv * p.f);
					p.x.Add (dt * p.v);

				}
			}
		}

		private void ParticleUpdate (double dt)
		{
			wind = Wind;
			foreach (var particles in particleMeshes) {
				intersections.Clear ();
				CheckCollisions (particles);
				HandleCollisions ();
				AdjustIntersections ();
			}

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
					/* Gravity, uses Buoyant force to counter gravity force */
					//double dp = p.p - AIR_P;
					/* Fb = (Pair - Pp) * g * V */
					//p.f.Add (dp * g * (p.m / p.p));
					p.f.Add (p.m * g);
                    
					/* Air drag force */
					//double kd = 0.000018;
					Vec3 u = p.v - wind;

					//Vec3 Fair = -kd * u;
					double C = 0.5; /* Drag constant */
					/* Air drag: Fdrag = 1/2 * C * P_air * A * V^2 */
					//Vec3 Fair = -0.5 * C * AIR_P * p.r * p.r * Math.PI * p.v.SqLength * p.v.UnitVector;
					Vec3 Fair = -6 * Math.PI * AIR_u * p.r * u;
					//p.f.Add (Fair);
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
			foreach (var entity in entities) {
				intersections.AddRange (entity.Collider.Collides (particles));
			}
		}

		private void HandleCollisions ()
		{
			foreach (var data in intersections) {
				double e = 0.8;
				var v = data.particle.v;
				data.particle.v = v - (1 + e) * Vec3.Dot (v, data.normal) * data.normal;
			}
		}

		private void AdjustIntersections ()
		{
			/* Adjust collided particles */
			foreach (var data in intersections) {
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
