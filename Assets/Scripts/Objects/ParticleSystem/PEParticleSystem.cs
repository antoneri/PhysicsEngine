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
		private const double MAX_AGE = 5.0;

		// Update is called once per frame
		public void Update ()
		{
			this.RemoveAll (p => p.age >= MAX_AGE);
			this.ForEach (p => p.age += Time.deltaTime);
		}

		public double Energy {
			get {
				return 0.5 * this.Sum (p => p.m * Vec3.Dot (p.v, p.v));
			}
		}
	}
}

