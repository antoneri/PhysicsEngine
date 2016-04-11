using System;

namespace PE
{
	public struct Vec3
	{
		double x;
		double y;
		double z;

		public Vec3 (double x, double y, double z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public double this [int key] {
			get {
				switch (key) {
				case 0:
					return x;
				case 1:
					return y;
				case 2:
					return z;
				}
			}
			set {
				switch (key) {
				case 0:
					x = value;
				case 1:
					y = value;
				case 2:
					z = value;
				}
			}
		}

		public static double Dot (Vec3 lhs, Vec3 rhs)
		{
			return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
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

        public static Vec3 operator/ (double lhs, Vec3 rhs)
        {
            return new Vec3(lhs / rhs.x, lhs / rhs.y, lhs / rhs.z);
        }

        public static Vec3 operator/ (Vec3 lhs, double rhs)
        {
            return rhs / lhs;
        }
    }
}

