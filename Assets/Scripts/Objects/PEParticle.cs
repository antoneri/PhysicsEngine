using UnityEngine;

using PE;

namespace PE
{
	public class Particle : Entity
	{
		public new double m;
		public new double m_inv;

		// Radius and density
		public double r;
		public double p;

		public double age = 0;
		public double lifetime = 0;

		public Particle ()
		{
		}

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
			m = mass;
			m_inv = 1 / mass;
			r = radius;
			p = density;
		}

		public Particle (Vec3 x, Vec3 v, double mass, double radius, double density, double lifetime) : this (x, v, mass, radius, density)
		{
			this.lifetime = lifetime;
		}

		public double Mass {
			get {
				return m;
			}
			set {
				m = value;
				m_inv = 1 / value;
			}
		}
	}
}
