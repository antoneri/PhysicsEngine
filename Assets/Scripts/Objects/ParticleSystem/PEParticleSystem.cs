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
		private const double MAX_AGE = 10.0;

		// Update is called once per frame
		public void Update ()
		{
			var dt = Time.deltaTime;

			for (int i = this.Count - 1; i >= 0; i--) {
				var p = this [i];
				p.age += dt;

				if (p.age > MAX_AGE) {
					this.RemoveAt (i);
				}
			}
		}

		public double Energy {
			get {
				return 0.5 * this.Sum (p => p.m * Vec3.Dot (p.v, p.v));
			}
		}
	}
}

