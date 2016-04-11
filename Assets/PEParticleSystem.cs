using UnityEngine;
using System.Collections.Generic;

namespace PE
{
	public class ParticleSystem : MonoBehaviour
	{
		public int N = 1000;
		public double MAX_AGE = 10;
		public List<Particle> particles;
		public double E_tot;

		// Use this for initialization
		void Start ()
		{
			particles = new List<Particle> ();
		}

		// Update is called once per frame
		void Update ()
		{
			particles.RemoveAll (p => p.age >= MAX_AGE);

			E_tot = 0;
			foreach (var p in particles) {
				E_tot += 0.5 * p.m * (p.v [0] * p.v [0] + p.v [1] * p.v [1] + p.v [2] * p.v [2]); 
			}
		}

		void AddParticle (Particle p)
		{
			particles.Add (p);
		}
	}
}

