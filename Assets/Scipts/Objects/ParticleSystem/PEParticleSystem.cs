using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PE
{
	public class ParticleSystem : List<Particle>
	{
		public double MAX_AGE = 10.0;
		public double MASS = 1e-5;

		// Update is called once per frame
		public void Update ()
		{
			this.RemoveAll (p => p.age >= MAX_AGE);

			this.ForEach (p => {
				p.age += Time.deltaTime;
			});
		}

		public void AddParticle (Vec3 x, Vec3 v)
		{
			this.Add (new Particle () {
				x = x,
				v = v,
				f = new Vec3 (0),
				m = MASS,
				age = 0,
			});
		}

		public double Energy {
			get {
				return 0.5 * this.Sum (p => p.m * Vec3.Dot (p.v, p.v));
			}
		}
	}
}

