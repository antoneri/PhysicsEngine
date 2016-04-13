using UnityEngine;
using System.Collections.Generic;

namespace PE
{
	public class ParticleSystem
	{
		public List<Particle> particles = new List<Particle> ();
		public double MAX_AGE = 10.0;
		public double E_tot;

		// Use this for initialization
		void Start ()
		{
		}

		// Update is called once per frame
		public void Update ()
		{
			particles.RemoveAll (p => p.age >= MAX_AGE);

            E_tot = 0;
            for (int i = 0; i < particles.Count; i++)
            {
                Particle p = particles[i];
                E_tot += 0.5 * p.m * Vec3.Dot(p.v, p.v);
                p.age += Time.deltaTime;
            }

		}

		public void AddParticle (Particle p)
		{
			particles.Add (p);
		}
	}
}

