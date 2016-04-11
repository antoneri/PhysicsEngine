using UnityEngine;
using System.Collections.Generic;

namespace PE
{
	public class ParticleSystem : MonoBehaviour
	{
		public List<Particle> particles;
		public double MAX_AGE = 10;
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
				E_tot += 0.5 * p.m * Vector3.Dot (p.v, p.v); 
			}
		}

		public void AddParticle (Particle p)
		{
			particles.Add (p);
		}
	}
}

