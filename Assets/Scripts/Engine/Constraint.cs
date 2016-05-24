using UnityEngine;
using System.Collections.Generic;
using System;

namespace PE
{
	public abstract class Constraint
	{
		public int body_i;
		public int body_j;
		public double k = 1000;

		public Constraint (int body_i, int body_j)
		{
			this.body_i = body_i;
			this.body_j = body_j;
		}

		public Constraint (int body_i, int body_j, double k) : this (body_i, body_j)
		{
			this.k = k;
		}

		abstract public Vec3 getConstraint (List<Particle> ps);

		abstract public Vec3[] getJacobians (List<Particle> ps);
	}

	public class DistanceConstraint : Constraint
	{
		private double L;

		public DistanceConstraint (int body_i, int body_j, double length, double k) : base (body_i, body_j, k)
		{
			L = length;
		}

		public DistanceConstraint (int body_i, int body_j, double length) : base (body_i, body_j)
		{
			L = length;
		}

		public override Vec3 getConstraint (List<Particle> ps)
		{
			Particle pi = ps [body_i];
			Particle pj = ps [body_j];
			Vec3 xij = pi.x - pj.x;

			return new Vec3 (xij.SqLength - L);
		}

		public override Vec3[] getJacobians (List<Particle> ps)
		{
			Particle pi = ps [body_i];
			Particle pj = ps [body_j];
			Vec3 u = (pi.x - pj.x).UnitVector;

			Vec3[] G = new Vec3[2];
			G [0] = u;
			G [1] = -u;

			return G;
		}
	}

	public class PositionConstraint : Constraint
	{
		private Vec3 x;

		public PositionConstraint (int body_i, int body_j, Vec3 position, double k) : base (body_i, body_j, k)
		{
			x = position;
		}

		public PositionConstraint (int body_i, int body_j, Vec3 position) : base (body_i, body_j)
		{
			x = position;
		}

		public override Vec3 getConstraint (List<Particle> ps)
		{
			Particle pi = ps [body_i];

			return new Vec3 ((x - pi.x).SqLength);
		}

		public override Vec3[] getJacobians (List<Particle> ps)
		{
			Particle pi = ps [body_i];
			Vec3 dx = x - pi.x;

			Vec3 u;

			if (dx.SqLength == 0) {
				u = new Vec3 ();
			} else {
				u = dx.UnitVector;
			}

			Vec3[] G = new Vec3[2];
			G [0] = -u;
			G [1] = -u;

			return G;
		}
	}
}
