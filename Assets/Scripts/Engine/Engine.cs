using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PE
{
	public class Engine : MonoBehaviour
	{
		public static Engine instance = null;

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

		private IEnumerator GameLoop ()
		{
			/* Simulation loop */
			while (true) {
				double dt = Time.fixedDeltaTime;

				foreach (var particleSystem in particleSystems) {
					// Create particles at emitter
					// (Remove particles at sinks or when they expire in time)
					particleSystem.Update ();

					// Do inter-particle collision detection and construct a
					// neighbour list – or use a fixed interaction list (cloth).

					// Loop over neighbour lists and compute interaction forces.
					// Accumulate the forces.Use Newton’s third law.

					// Accumulate external forces from e.g.gravity.
					foreach (var p in particleSystem) {
                        p.f.Set(p.m * g);
                        /*
						p.f.x = p.m * g.x;
						p.f.y = p.m * g.y;
						p.f.z = p.m * g.z;
                        */

                        p.f.Subtract(0.05 * p.m * p.v);
                        /*
						p.f.x -= 0.05 * p.m * p.v.x;
						p.f.y -= 0.05 * p.m * p.v.y;
						p.f.z -= 0.05 * p.m * p.v.z;
                        */
					}

					// Accumulate dissipative forces, e.g.drag and viscous drag.

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
                        p.v.Add(dt * p.m_inv * p.f);
                        /*
						p.v.x += dt * p.m_inv * p.f.x;
						p.v.y += dt * p.m_inv * p.f.y;
						p.v.z += dt * p.m_inv * p.f.z;
                        */
                        p.x.Add(dt * p.v);
                        /*
						p.x.x += dt * p.v.x;
						p.x.y += dt * p.v.y;
						p.x.z += dt * p.v.z;
                        */
					}

					/* Adjust collided particles */
					foreach (IntersectData data in intersections) {
						Particle p = data.particle;
						if (Vec3.Dot (p.v, data.normal) < 0) {
                            p.v.Set(0.0);
                            /*
							p.v.x = 0;
							p.v.y = 0;
							p.v.z = 0;
                            */
						}
						double x = Vec3.Dot (p.x - data.point, data.normal);
						if (x < 0) {
							p.x = data.point;
						}
					}

				}

				foreach (var particles in particleMeshes) {
					// Reset forces
					for (int i = 0; i < particles.Size; i++) {
						var p = particles [i];
                        p.f.Set(0.0);
						//p.f.x = p.f.y = p.f.z = 0;
					}

					foreach (var n in particles.Neighbors) {
						var pa = n.p1;
						var pb = n.p2;
						var r = pa.x - pb.x;
						var v = pa.v - pb.v;
						pb.f += -(n.k * (r.Length - n.x0) + n.kd * Vec3.Dot (v, r) / r.Length) * r.Normalize;
						pa.f += -pb.f;
					}

					for (int i = 0; i < particles.Size; i++) {
						var p = particles [i];
						if (i == 0 || i == particles.Rows - 1) {
							// Top corners
							// TODO Add upwards force instead
							continue;
						}

						// Add gravity
						p.f += p.m * g;

						// Integrate
						p.v.x += dt * p.m_inv * p.f.x;
						p.v.y += dt * p.m_inv * p.f.y;
						p.v.z += dt * p.m_inv * p.f.z;
						p.x.x += dt * p.v.x;
						p.x.y += dt * p.v.y;
						p.x.z += dt * p.v.z;
					}
				}

				yield return new WaitForFixedUpdate ();
			}
		}
	}
}

