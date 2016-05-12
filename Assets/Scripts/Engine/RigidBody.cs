using UnityEngine;
using System;

namespace PE
{
	public class RigidBody
	{
		Collider collider;

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

		public Collider Collider {
			get {
				return this.collider;
			}
			set {
				collider = value;
			}
		}
	}
}

