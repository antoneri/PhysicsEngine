using System;

namespace PE
{
	public class Vec3
	{
		public double x;
		public double y;
		public double z;

        public Vec3(double v)
        {
            x = v;
            y = v;
            z = v;
        }

		public Vec3 (double x, double y, double z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public double this [int index] {
			get {
                return this[index];
			}
			set {
                this[index] = value;
			}
		}

		public static double Dot (Vec3 lhs, Vec3 rhs)
		{
			return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
		}

        public static double length(Vec3 v)
        {
            return Math.Sqrt(Math.Pow(v.x, 2) + Math.Pow(v.y, 2) + Math.Pow(v.z, 2));
        }

		public static Vec3 operator+ (Vec3 lhs, Vec3 rhs)
		{
			return new Vec3 (lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
		}

		public static Vec3 operator- (Vec3 lhs, Vec3 rhs)
		{
			return new Vec3 (lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
		}

        public static Vec3 operator* (double lhs, Vec3 rhs)
        {
            return new Vec3(lhs * rhs.x, lhs * rhs.y, lhs * rhs.z);
        }

        public static Vec3 operator* (Vec3 lhs, double rhs)
        {
            return rhs * lhs;
        }

        public static Vec3 operator/ (Vec3 lhs, double rhs)
        {
            return new Vec3(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs);
        }

        public override string ToString()
        {
            return "[" + x + ", " + y + ", " + z + "]";
        }

    }
}

