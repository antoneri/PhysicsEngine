using UnityEngine;
using System;
using System.Collections.Generic;

namespace PE
{
	public class Sphere : RigidBody
	{
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

			Collider = new SphereCollider (this);
		}
	}
}

