using UnityEngine;
using System.Collections.Generic;
using System;

namespace PE {
    public abstract class Constraint {

        public int body_i;
        public int body_j;
        public double k = 1000;

        public Constraint(int body_i, int body_j)
        {
            this.body_i = body_i;
            this.body_j = body_j;
        }
        public Constraint(int body_i, int body_j, double k) : this (body_i, body_j)
        {
            this.k = k;
        }

        abstract public Vec3 getConstraint(List<Particle> ps);
        abstract public Vec3[] getJacobians(List<Particle> ps);
    }

    public class DistanceConstraint : Constraint
    {
        private double L;
        public DistanceConstraint(int body_i, int body_j, double length, double k) : base(body_i, body_j, k)
        {
            L = length;
        }

        public DistanceConstraint(int body_i, int body_j, double length) : base(body_i, body_j)
        {
            L = length;
        }

        public override Vec3 getConstraint(List<Particle> ps)
        {

            Particle pi = ps[body_i];
            Particle pj = ps[body_j];

            Vec3 g = new Vec3((pi.x - pj.x).SqLength - L);

            return g;
        }

        public override Vec3[] getJacobians(List<Particle> ps)
        {
            Particle pi = ps[body_i];
            Particle pj = ps[body_j];
            Vec3 u = (pi.x - pj.x).UnitVector;

            Vec3[] G = new Vec3[2];
            G[0] = u;
            G[1] = -u;

            return G;
        }
    }

    public class PositionConstraint : Constraint
    {

        private Vec3 p;

        public PositionConstraint(int body_i, int body_j, Vec3 position, double k) : base(body_i, body_j, k)
        {
            p = position;
        }

        public PositionConstraint(int body_i, int body_j, Vec3 position) : base(body_i, body_j)
        {
            p = position;
        }

        public override Vec3 getConstraint(List<Particle> ps)
        {
            Particle pi = ps[body_i];

            Vec3 g = new Vec3((p - pi.x).SqLength);

            return g;
        }

        public override Vec3[] getJacobians(List<Particle> ps)
        {
            Particle pi = ps[body_i];
            Vec3 dp = p - pi.x;

            Vec3 u;
            if (dp.SqLength == 0)
            {
                u = new Vec3(0);
            } else
            {
                u = dp.UnitVector;
            }

            Vec3[] G = new Vec3[2];
            G[0] = -u;
            G[1] = -u;

            return G;
        }
    }

}
