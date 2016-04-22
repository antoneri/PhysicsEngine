using UnityEngine;

using PE;

namespace PE
{
	public class Particle
	{
		public Vec3 x;
		public Vec3 v;
		public Vec3 f;
		public double m;
		public double m_inv;
		/* Radius */
		public double r;
		/* Density */
		public double p;
		public double age = 0;

		public Particle (Vec3 x, double mass) : this (x, new Vec3 (), mass)
		{
		}

		public Particle (Vec3 x, Vec3 v, double mass) : this (x, v, mass, 0, 0)
		{
		}

		public Particle (Vec3 x, Vec3 v, double mass, double radius, double density)
		{
			this.x = x;
			this.v = v;
			f = new Vec3 ();
			m = mass;
			m_inv = 1 / mass;
			r = radius;
			p = density;
		}
	}
}
