using System;

namespace PE
{
	public class Spring
	{
		public Particle p1;
		public Particle p2;
		public double x0;
		public double k;
		public double kd;

		public Spring (Particle p1, Particle p2, double k, double kd)
		{
			this.p1 = p1;
			this.p2 = p2;
			this.k = k;
			this.kd = kd;
			x0 = (p1.x - p2.x).Length;
		}

		public Vec3 Force {
			get {
				var x = Displacement;
				var v = RelativeVelocity;
				var u = Direction;
				return -(k * x + kd * Vec3.Dot (v, u)) * u;
			}
		}

		public double Displacement {
			get {
				return Distance - x0;
			}
		}

		public double Distance {
			get {
				return (p2.x - p1.x).Length;
			}
		}

		public Vec3 Direction {
			get {
				return (p2.x - p1.x).UnitVector;
			}
		}

		public Vec3 RelativeVelocity {
			get {
				return p2.v - p1.v;
			}
		}
	}
}

