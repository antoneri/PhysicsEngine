using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PE
{
	public class Rope : MonoBehaviour
	{
		const int NUM_PARTICLES = 10;

		List<Particle> particles = new List<Particle> ();
		private UnityEngine.ParticleSystem.Particle[] unityParticles = new UnityEngine.ParticleSystem.Particle[NUM_PARTICLES];

		// Use this for initialization
		void Start ()
		{
			var startPos = new Vec3 (-3, 7, 0);
			var dx = new Vec3 (0, -1, 0);
			var mass = 1.0;

			for (int i = 0; i < NUM_PARTICLES; i++) {
				particles.Add (new Particle (startPos + i * dx, mass));
			}

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

