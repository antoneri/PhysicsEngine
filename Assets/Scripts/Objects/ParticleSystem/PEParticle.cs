using UnityEngine;

using PE;

namespace PE
{
	public class Particle
	{
		public Vec3 x;
		public Vec3 v;
		public Vec3 f;
		public double m;
		public double m_inv;
        public double r; /* Radius */
        public double p; /* Density */
		public double age = 0;

		public Particle (Vec3 x, Vec3 v, double mass, double radius, double density)
		{
			this.x = x;
			this.v = v;
			f = new Vec3 ();
			m = mass;
			m_inv = 1 / mass;
            r = radius;
            p = density;
		}
	}
}
