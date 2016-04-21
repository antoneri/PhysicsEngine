using UnityEngine;
using System;

namespace PE
{
	public class Vec3
	{
		public double x;
		public double y;
		public double z;

		public Vec3 () : this (0)
		{
		}

		public Vec3 (double v) : this (v, v, v)
		{
		}

		public Vec3 (double x, double y, double z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public double this [int index] {
			get {
				switch (index) {
				case 0:
					return x;
				case 1:
					return y;
				case 2:
					return z;
				default:
					throw new ArgumentOutOfRangeException ();
				}
			}
			set {
				switch (index) {
				case 0:
					x = value;
					break;
				case 1:
					y = value;
					break;
				case 2:
					z = value;
					break;
				default:
					throw new ArgumentOutOfRangeException ();
				}
			}
		}

        public void Set(double x, double y, double z)
        {
            this.x = x; this.y = y; this.z = z;
        }

        public void Set(Vec3 v)
        {
            x = v.x; y = v.y; z = v.z;
        }

        public void Set(double v)
        {
            x = v; y = v; z = v;
        }

		public double Length {
			get {
				return Math.Sqrt (x * x + y * y + z * z);
			}
		}

		public Vec3 Normalize {
			get {
				return this / Length;
			}
		}

		public static implicit operator Vector3 (Vec3 v)
		{
			return new Vector3 ((float)v.x, (float)v.y, (float)v.z);
		}

		public static implicit operator Vec3 (Vector3 v)
		{
			return new Vec3 ((double)v.x, (double)v.y, (double)v.z);
		}

		public static double Dot (Vec3 lhs, Vec3 rhs)
		{
			return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
		}

		public static Vec3 operator+ (Vec3 lhs, Vec3 rhs)
		{
			return new Vec3 (lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
		}

        public void Add (Vec3 v)
        {
            x += v.x; y += v.y; z += v.z;
        }

		public static Vec3 operator- (Vec3 v)
		{
			return new Vec3 (-v.x, -v.y, -v.z);
		}

        public void Negate()
        {
            x = -x; y = -y; z = -z;
        }

		public static Vec3 operator- (Vec3 lhs, Vec3 rhs)
		{
			return new Vec3 (lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
		}

        public void Subtract(Vec3 v)
        {
            x -= v.x; y -= v.y; z -= v.z;
        }

		public static Vec3 operator* (double lhs, Vec3 rhs)
		{
			return new Vec3 (lhs * rhs.x, lhs * rhs.y, lhs * rhs.z);
		}

        public void mult (double k)
        {
            x *= k; y *= k; z *= k;
        }

		public static Vec3 operator* (Vec3 lhs, double rhs)
		{
			return rhs * lhs;
		}

		public static Vec3 operator/ (Vec3 lhs, double rhs)
		{
			double inv = 1 / rhs;
			return new Vec3 (lhs.x * inv, lhs.y * inv, lhs.z * inv);
		}

        public void divide (double k)
        {
            x /= k; y /= k; z /= k;
        }

		public override string ToString ()
		{
			return "[" + x + ", " + y + ", " + z + "]";
		}

	}
}

