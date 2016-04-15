using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PE
{
	public class ParticleSystem : IEnumerable<Particle>
	{
		public List<Particle> particles = new List<Particle> ();
		public double MAX_AGE = 10.0;
		public double MASS = 1e-5;
		public double E_tot;

		// Update is called once per frame
		public void Update ()
		{
			particles.RemoveAll (p => p.age >= MAX_AGE);

			// Update total energy and particle age
			E_tot = 0;

			foreach (var p in particles) {
				E_tot += 0.5 * p.m * Vec3.Dot (p.v, p.v);
				p.age += Time.deltaTime; // FIXME this is called in Engine.GameLoop, should we use another time step?
			}
		}

		public void AddParticle (Particle p)
		{
			particles.Add (p);
		}

		public void AddParticle (Vec3 x, Vec3 v)
		{
			particles.Add (new Particle () {
				x = x,
				v = v,
				f = new Vec3 (0),
				m = MASS,
				age = 0,
			});
		}

		public int ParticleCount {
			get { return particles.Count; }
		}

		// Looping over particles
		public IEnumerator<Particle> GetEnumerator ()
		{
			return particles.GetEnumerator ();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			throw new NotImplementedException ();
		}
	}
}

