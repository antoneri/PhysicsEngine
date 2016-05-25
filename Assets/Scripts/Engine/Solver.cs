using UnityEngine;
using System.IO;
using System.Collections;

namespace PE
{
	public enum IterationDirection
	{
		forward,
		backward,
	}

	public class Solver
	{
		const double ERROR_THRESH = 0.001;

		public static Vec3Vector GaussSeidel (Vec3Matrix S, Vec3Vector B, uint iterations, IterationDirection direction = IterationDirection.forward, TextWriter logFile = null)
		{
			Vec3Vector lambda = new Vec3Vector (B.Size);

			for (int i = 0; i < iterations; i++) {
				double absError = GaussSeidelIterate (S, lambda, B, direction);

				//Debug.Log ("Solver error: " + absError + " at iteration " + i);
				if (logFile != null) {
					logFile.WriteLine (i + " " + absError);
				}

				if (absError < ERROR_THRESH) {
					break;
				}
			}

			return lambda;
		}

		private static double GaussSeidelIterate (Vec3Matrix S, Vec3Vector lambda, Vec3Vector B, IterationDirection direction = IterationDirection.forward)
		{
			Vec3Vector r = lambda.Clone;

			if (direction == IterationDirection.forward) {
				for (int i = 0; i < lambda.Length; i++) {
					Vec3 sum = new Vec3 ();

					for (int j = 0; j < lambda.Length; j++) {
						sum += S [i, j] * lambda [j];
					}

					var deltaLambda = (1.0 / S [i, i]) * (B [i] - sum);
					lambda [i] += deltaLambda;
				}
			} else {
				for (int i = lambda.Length - 1; i >= 0; i--) {
					Vec3 sum = new Vec3 ();

					for (int j = lambda.Length - 1; j >= 0; j--) {
						sum += S [i, j] * lambda [j];
					}

					var deltaLambda = (1.0 / S [i, i]) * (B [i] - sum);
					lambda [i] += deltaLambda;
				}
			}
	
			return (lambda - r).Norm;
		}
	}
}
