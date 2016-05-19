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

		// Cached objects
		private Vec3 v_rel = new Vec3();
		private Vec3 x_rel = new Vec3();

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
				u.Scale (-(k * x + kd * Vec3.Dot (v, u)));
				return u;
			}
		}

		public double Displacement {
			get {
				return Distance - x0;
			}
		}

		public double Distance {
			get {
				return RelativePosition.Length;
			}
		}

		public Vec3 Direction {
			get {
				return RelativePosition.UnitVector;
			}
		}

		public Vec3 RelativePosition {
			get {
				x_rel.Set (p2.x);
				x_rel.Subtract (p1.x);
				return x_rel;
			}
		}

		public Vec3 RelativeVelocity {
			get {
				v_rel.Set (p2.v);
				v_rel.Subtract (p1.v);
				return v_rel;
			}
		}
	}
}

