﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using PE;

namespace PE
{
	public class Engine : MonoBehaviour
	{
		Vec3 g = new Vec3 (0, -9.82, 0);
		List<ParticleSystem> particleSystems = new List<ParticleSystem> ();

		// Use this for initialization
		void Start ()
		{
			
			StartCoroutine (GameLoop ());

		}

		private IEnumerator GameLoop ()
		{
			/* Simulation loop */
			while (true) {

				/*Create particles at emitter
            	(Remove particles at sinks or when they expire in time) */

				/*Do inter-particle collision detection and construct a
				neighbour list – or use a fixed interaction list (cloth). */

				/*Loop over neighbour lists and compute interaction
				forces.Accumulate the forces.Use Newton’s third law. */

				/*Accumulate external forces from e.g.gravity.*/

				/*Accumulate dissipative forces, e.g.drag and viscous drag. */

				for (int i = 0; i < particleSystems.Count; i++) {
					for (int j = 0; j < particleSystems [i].particles.Count; j++) {
                        double dt = Time.fixedDeltaTime;
						var p = particleSystems [i].particles [j];
						p.f = g;
						p.v = p.v + dt * p.f / p.m;
						p.x = p.x + dt * p.v;
					}
				}

				/* Find contact sets with external boundaries, e.g.a plane.
				Handle external boundary conditions by reflecting the
				the velocities. */

				/*Take a timestep and integrate using e.g.Verlet / Leap Frog */

				yield return new WaitForFixedUpdate ();
			}
		}
	}
}

