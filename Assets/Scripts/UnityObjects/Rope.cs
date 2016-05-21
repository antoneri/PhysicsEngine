﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PE
{
	public class Rope : MonoBehaviour
	{
		public int NUM_PARTICLES = 20;

        public double mass_last_particle = 1;
        public double spring_k = 1000;

		ParticleSystem particles = new ParticleSystem ();
        private UnityEngine.ParticleSystem.Particle[] unityParticles;

		// Use this for initialization
		void Start ()
		{
            unityParticles = new UnityEngine.ParticleSystem.Particle[NUM_PARTICLES];

            var startPos = new Vec3 (-3, 15, 0);
			var dx = new Vec3 (-0.25, -0.25, 0);
			var mass = 1.0;

			for (int i = 0; i < NUM_PARTICLES; i++) {
				particles.Add (new Particle (startPos + i * dx, mass));
			}

            particles[NUM_PARTICLES - 1].m = mass_last_particle;

            List<Constraint> constraints = new List<Constraint>();
            constraints.Add(new PositionConstraint(0, 0, startPos));
            double length = 0.1;
            for (int i = 0; i < NUM_PARTICLES - 1; i++)
            {
                constraints.Add(new DistanceConstraint(i, i + 1, length, spring_k));
            }

            particles.constraints = constraints;

            Engine.instance.AddRope (particles);
		}

		// Update is called once per frame
		void Update ()
		{
			for (int i = 0; i < particles.Count; i++) {
				unityParticles [i] = new UnityEngine.ParticleSystem.Particle () {
					position = (Vector3)particles [i].x - transform.position,
					startColor = Color.black,
					startSize = 0.5f,
				};
			}

			GetComponent<UnityEngine.ParticleSystem> ().SetParticles (unityParticles, particles.Count);

		}
	}

}
