﻿using UnityEngine;

using PE;

namespace PE
{
	public class Particle
	{
		public Vec3 x;
		public Vec3 v;
		public Vec3 f;
		public double m;
		public double age = 0;

		public Particle (Vec3 x, Vec3 v, double mass)
		{
			this.x = x;
			this.v = v;
			this.m = mass;
		}
	}
}
