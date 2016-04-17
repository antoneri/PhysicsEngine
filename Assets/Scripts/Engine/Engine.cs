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
						p.f.x = p.m * g.x;
						p.f.y = p.m * g.y;
						p.f.z = p.m * g.z;
						p.f.x -= 0.05 * p.m * p.v.x;
						p.f.y -= 0.05 * p.m * p.v.y;
						p.f.z -= 0.05 * p.m * p.v.z;
					}

					// Accumulate dissipative forces, e.g.drag and viscous drag.

					// Find contact sets with external boundaries, e.g.a plane.
					// Handle external boundary conditions by reflecting the
					// the velocities.


					// Take a timestep and integrate using e.g.Verlet / Leap Frog
					foreach (var p in particleSystem) {
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

