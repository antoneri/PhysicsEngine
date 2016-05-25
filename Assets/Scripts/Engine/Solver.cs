using UnityEngine;
using System.Collections;

namespace PE
{
	public class Solver
	{
		const double ERROR_THRESH = 0.001;

		public static Vec3Vector GaussSeidel (Vec3Matrix S, Vec3Vector B, uint iterations)
		{
			Vec3Vector lambda = new Vec3Vector (B.Size);

			for (int i = 0; i < iterations; i++) {
				double absError = GaussSeidelIterate (S, lambda, B);
				if (absError < ERROR_THRESH) {
					break;
				}
				//Debug.Log("Solver error: " + absError);
			}

			return lambda;
		}

		private static double GaussSeidelIterate (Vec3Matrix S, Vec3Vector lambda, Vec3Vector B)
		{
			Vec3Vector r = new Vec3Vector (lambda);

			for (int i = 0; i < lambda.Length; i++) {
				Vec3 sum = new Vec3 ();

				for (int j = 0; j < i; j++) {
					sum += S [i, j] * lambda [j];
				}

				for (int j = i + 1; j < lambda.Length; j++) {
					sum += S [i, j] * lambda [j];
				}

				lambda [i] = (1.0 / S [i, i]) * (B [i] - sum); 
			}

			return (lambda - r).Norm;
		}
	}
}
