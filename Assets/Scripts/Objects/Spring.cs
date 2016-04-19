using System;

namespace PE
{
	public class Spring
	{
		public Particle p1;
		public Particle p2;
		public double x0;
		public double k;
		public double kd = 0.01;

		public Spring (Particle p1, Particle p2, double k)
		{
			this.p1 = p1;
			this.p2 = p2;
			this.k = k;
			x0 = (p1.x - p2.x).Length;
		}
	}
}

