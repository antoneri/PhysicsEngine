using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PE
{
	public class Engine : MonoBehaviour
	{
		public static Engine instance = null;

		readonly Vec3 g = new Vec3 (0, -9.82, 0);
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

				/*Create particles at emitter
            	(Remove particles at sinks or when they expire in time) */
				foreach (var particleSystem in particleSystems) {
					particleSystem.Update ();
				}

				/*Do inter-particle collision detection and construct a
				neighbour list – or use a fixed interaction list (cloth). */

				/*Loop over neighbour lists and compute interaction
				forces.Accumulate the forces.Use Newton’s third law. */

				/*Accumulate external forces from e.g.gravity.*/
				foreach (var particleSystem in particleSystems) {
					foreach (var p in particleSystem) {
						p.f = p.m * g;
					}
				}

				/*Accumulate dissipative forces, e.g.drag and viscous drag. */
					
				/* Find contact sets with external boundaries, e.g.a plane.
				Handle external boundary conditions by reflecting the
				the velocities. */


				/*Take a timestep and integrate using e.g.Verlet / Leap Frog */
				foreach (var particleSystem in particleSystems) {
					foreach (var p in particleSystem) {
						p.v = p.v + dt * p.f / p.m;
						p.x = p.x + dt * p.v;
					}
				}

				yield return new WaitForFixedUpdate ();
			}
		}
	}
}

