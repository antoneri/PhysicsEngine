using UnityEngine;
using System.Collections.Generic;

namespace PE
{
	public class ParticleSystem : MonoBehaviour
	{
		public int N = 1000;
		public double MAX_AGE = 10;
		public List<Particle> particles;

		// Use this for initialization
		void Start ()
		{
			particles = new List<Particle> ();
		}

		// Update is called once per frame
		void Update ()
		{
			particles.RemoveAll (p => p.age >= MAX_AGE);
		}

		void AddParticle (Particle p)
		{
			particles.Add (p);
		}
	}
}

