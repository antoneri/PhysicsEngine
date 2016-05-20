using UnityEngine;
using System;
using System.Collections.Generic;

namespace PE
{
	public class Sphere : Collider
	{
		public Vec3 x;
		public Quaternion q;

		public Vec3 v;
		public Vec3 omega;

		public Vec3 f;
		public Vec3 tau;

		public Mat3 m;
		public Mat3 m_inv;
		public Mat3 I;
		public Mat3 I_inv;

		public double r;

		public Sphere (Vec3 x, double r, double m)
		{
			if (m == 0) {
				throw new DivideByZeroException ("Mass can't be zero!");
			}

			double i = 2.0 / 5.0 * m * r * r;

			if (i == 0) {
				throw new DivideByZeroException ("Inertia can't be zero!");
			}

			this.x = x;
			this.r = r;

			this.m = Mat3.Diag(m);
			m_inv = Mat3.Diag (1.0 / m);

			I = Mat3.Diag (i);
			I_inv = Mat3.Diag (1 / i);

			q = new Quaternion ();
			v = new Vec3 ();
			omega = new Vec3 ();
			f = new Vec3 ();
			tau = new Vec3 ();
		}

		public override List<Intersection> Collides (IEnumerable<Particle> ps)
		{
			return CollisionNotImplemented;
		}

		public override List<Intersection> Collides (Sphere b)
		{
			return Collides (this, b);
		}
	}
}

