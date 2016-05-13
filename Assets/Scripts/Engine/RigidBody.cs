using UnityEngine;
using System;
using System.Collections.Generic;

namespace PE
{
	public class RigidBody : Collider
	{
		public Vec3 x;
		public Quaternion q;

		public Vec3 v;
		public Vec3 omega;

		public Vec3 f;
		public Vec3 tau;

		public double m;
		public double m_inv;
		public double I;
		public double I_inv;

		public double r = 0.5;

		public RigidBody (Vec3 x, double m, double I)
		{
			this.x = x;
			this.m = m;
			this.I = I;

			q = new Quaternion ();
			v = new Vec3 ();
			omega = new Vec3 ();
			f = new Vec3 ();
			tau = new Vec3 ();

			m_inv = 1 / m;
			I_inv = 1 / I_inv;
		}

		public override List<Intersection> Collides (IEnumerable<Particle> ps)
		{
			return CollisionNotImplemented;
		}

		public override List<Intersection> Collides (RigidBody b)
		{
			return Collides (this, b);
		}
	}
}

