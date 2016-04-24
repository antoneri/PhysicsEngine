using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PE
{
	public class Engine : MonoBehaviour
	{
		public static Engine instance = null;

		public Vector3 Wind = new Vector3 (0, 0, 0);
		private Vec3 wind = new Vec3 (0);

		private const double AIR_P = 1.18;
		/* Density */
		private const double AIR_u = 1.002;
		/* Viscosity */

		private readonly Vec3 g = new Vec3 (0, -9.82, 0);

		List<ParticleSystem> particleSystems = new List<ParticleSystem> ();
		List<Entity> entities = new List<Entity> ();
		List<ParticleMesh> particleMeshes = new List<ParticleMesh> ();

		private List<IntersectData> intersections = new List<IntersectData> (10000);

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

		public void AddParticleMesh (ParticleMesh particleMesh)
		{
			particleMeshes.Add (particleMesh);
		}

		public void AddEntity (Entity e)
		{
			entities.Add (e);
		}
			
		// Use this for initialization
		void Start ()
		{
			StartCoroutine (GameLoop ());
		}

		public void FixedUpdate ()
		{
			float rate = 400f; // Hz
			float dT = Time.fixedDeltaTime;
			int N = Mathf.CeilToInt (dT * rate);
			var dt = dT / N;

			for (int i = 0; i < N; i++) {
				ClothUpdate (dt);
			}
		}

		public void ClothUpdate (float dt)
		{
			foreach (var particles in particleMeshes) {
				// Reset forces
				for (int i = 0; i < particles.Size; i++) {
					particles [i].f.SetZero ();
				}

				foreach (var n in particles.Neighbors) {
					var pa = n.p1;
					var pb = n.p2;
					var r = pb.x - pa.x;
					var v = pb.v - pa.v;
					var u = r.UnitVector;
					var f = -(n.k * (r.Length - n.x0) + n.kd * Vec3.Dot (v, u)) * u;
					pb.f.Add (f);
					pa.f.Add (-f);
				}

				for (int i = 0; i < particles.Size; i++) {
					var p = particles [i];
					if (i == 0 || i == particles.Rows - 1) {
						// Top corners
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

		private IEnumerator GameLoop ()
		{
			/* Simulation loop */
			while (true) {
				double dt = Time.fixedDeltaTime;
				wind = Wind;

				foreach (var particleSystem in particleSystems) {

					/* Clear forces */
					foreach (var p in particleSystem) {
						p.f.Set (0);
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
						double dp = p.p - AIR_P;
						/* Fb = (Pair - Pp) * g * V */
						p.f.Add (dp * g * (p.m / p.p));
						//p.f.Add (p.m * g);
                        

                        
						//p.f.Add (-0.5 * p.m * g);

						/* Air drag force */
						//double kd = 0.000018;
						Vec3 u = p.v - wind;

						//Vec3 Fair = -kd * u;
						double C = 0.5; /* Drag constant */
						/* Air drag: Fdrag = 1/2 * C * P_air * A * V^2 */
						//Vec3 Fair = -0.5 * C * AIR_P * p.r * p.r * Math.PI * p.v.SqLength * p.v.UnitVector;
						Vec3 Fair = -6 * Math.PI * AIR_u * p.r * u;

						p.f.Add (Fair);
					}
						
					// Find contact sets with external boundaries, e.g.a plane.
					// Handle external boundary conditions by reflecting the
					// the velocities.
					intersections.Clear ();
					foreach (Entity e in entities) {
						intersections.AddRange (e.getCollider ().Collides (particleSystem));
					}
                    
					foreach (IntersectData data in intersections) {
						double e = 0.8;
						Vec3 v = data.particle.v;
						data.particle.v = v - (1 + e) * (Vec3.Dot (v, data.normal) * data.normal);
					}
                    


					// Take a timestep and integrate using e.g.Verlet / Leap Frog
					foreach (var p in particleSystem) {
						p.v.Add (dt * p.m_inv * p.f);

						p.x.Add (dt * p.v);
					}

					/* Adjust collided particles */
					foreach (IntersectData data in intersections) {
						Particle p = data.particle;
						if (Vec3.Dot (p.v, data.normal) < 0) {
							p.v.Set (0.0);
						}
						double x = Vec3.Dot (p.x - data.point, data.normal);
						if (x < 0) {
							p.x = data.point;
						}
					}

				}
					
				yield return new WaitForFixedUpdate ();
			}
		}

	}
}

