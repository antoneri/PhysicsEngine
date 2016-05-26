using System;
using System.Collections.Generic;

namespace PE
{
	public class ParticleMesh : Matrix<Particle>
	{
		List<Spring> neighbors;

		public ParticleMesh (int rows, int cols) : base (rows, cols)
		{
			
		}

		public List<Spring> Neighbors {
			get {
				return neighbors;
			}
			set {
				neighbors = value;
			}
		}
	}
}

