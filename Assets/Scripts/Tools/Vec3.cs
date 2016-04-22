using UnityEngine;
using System;

namespace PE
{
	public class Vec3
	{
		public double x;
		public double y;
		public double z;

		/*
		 * Constructors
		 */
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

		/*
		 * Getters and setters
		 */
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

		/*
		 * Vector properties
		 */
		public double Length {
			get {
				return Math.Sqrt (x * x + y * y + z * z);
			}
		}

        public double SqLength
        {
            get
            {
                return x * x + y * y + z * z;
            }
        }

        public Vec3 UnitVector {
			get {
				return this / Length;
			}
		}

		/*
		 * Vector-vector operations
		 */
		public static double Dot (Vec3 lhs, Vec3 rhs)
		{
			return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
		}

		public static Vec3 Cross (Vec3 lhs, Vec3 rhs)
		{
			var x = lhs.y * rhs.z - lhs.z * rhs.y;
			var y = lhs.z * rhs.x - lhs.x * rhs.z;
			var z = lhs.x * rhs.y - lhs.y * rhs.x;
			return new Vec3 (x, y, z);
		}

		/*
		 * Static operators
		 */
		public static Vec3 operator+ (Vec3 lhs, Vec3 rhs)
		{
			return new Vec3 (lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
		}

		public static Vec3 operator- (Vec3 v)
		{
			return new Vec3 (-v.x, -v.y, -v.z);
		}

		public static Vec3 operator- (Vec3 lhs, Vec3 rhs)
		{
			return new Vec3 (lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
		}

		public static Vec3 operator* (double lhs, Vec3 rhs)
		{
			return new Vec3 (lhs * rhs.x, lhs * rhs.y, lhs * rhs.z);
		}

        public static Vec3 operator* (Vec3 lhs, Vec3 rhs)
        {
            return new Vec3(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z);
        }

		public static Vec3 operator* (Vec3 lhs, double rhs)
		{
			return rhs * lhs;
		}

		public static Vec3 operator/ (Vec3 lhs, double rhs)
		{
			var inv = 1 / rhs;
			return new Vec3 (lhs.x * inv, lhs.y * inv, lhs.z * inv);
		}

		/*
		 * Java-style mutators
		 */
		public void Add (Vec3 v)
		{
			x += v.x;
			y += v.y;
			z += v.z;
		}

		public void Negate ()
		{
			x = -x;
			y = -y;
			z = -z;
		}

		public void Subtract (Vec3 v)
		{
			x -= v.x;
			y -= v.y;
			z -= v.z;
		}

		public void Scale (double k)
		{
			x *= k;
			y *= k;
			z *= k;
		}

		public void Divide (double k)
		{
			Scale (1 / k);
		}

		public void SetZero ()
		{
			Set (0);
		}

		public void Set (double v)
		{
			Set (v, v, v);
		}

		public void Set (Vec3 v)
		{
			Set (v.x, v.y, v.z);
		}

		public void Set (double x, double y, double z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		/*
		 * Conversion operators
		 */
		public static implicit operator Vector3 (Vec3 v)
		{
			return new Vector3 ((float)v.x, (float)v.y, (float)v.z);
		}

		public static implicit operator Vec3 (Vector3 v)
		{
			return new Vec3 ((double)v.x, (double)v.y, (double)v.z);
		}

		public override string ToString ()
		{
			return "[" + x + ", " + y + ", " + z + "]";
		}
	}
}

