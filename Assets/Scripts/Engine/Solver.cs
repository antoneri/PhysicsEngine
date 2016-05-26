using UnityEngine;
using System.IO;
using System.Collections;

namespace PE
{
	public enum IterationDirection
	{
		Forward,
		Backward,
	}

	public class Solver
	{
		private const double errorThreshold = 0.001;

		public static Vec3Vector GaussSeidel (Vec3Matrix S, Vec3Vector B, uint iterations, IterationDirection direction = IterationDirection.Forward, TextWriter logFile = null)
		{
			if (logFile == null) {
				logFile = new NullWriter ();
			}

			Vec3Vector lambda = new Vec3Vector (B.Size);

			for (int i = 0; i < iterations; i++) {
				double[] err = GaussSeidelIterate (S, lambda, B, direction);
				double absError = err [0];
				double sigma = err [1];

				logFile.WriteLine (i + 1 + " " + absError + " " + sigma);

				if (absError < errorThreshold) {
					break;
				}
			}

			return lambda;
		}

		private static double[] GaussSeidelIterate (Vec3Matrix S, Vec3Vector lambda, Vec3Vector B, IterationDirection direction)
		{
			Vec3Vector lambda_old = lambda.Clone ();
			double sigma = 0;
			var deltaLambda = new Vec3 ();
			var sum = new Vec3 ();

			if (direction == IterationDirection.Forward) {
				for (int i = 0; i < lambda.Length; i++) {
					sum.SetZero ();

					for (int j = 0; j < lambda.Length; j++) {
						sum += S [i, j] * lambda [j];
					}

					deltaLambda.Set ((1.0 / S [i, i]) * (B [i] - sum));
					sigma += deltaLambda.SqLength;
					lambda [i] += deltaLambda;
				}
			} else {
				for (int i = lambda.Length - 1; i >= 0; i--) {
					sum.SetZero ();

					for (int j = lambda.Length - 1; j >= 0; j--) {
						sum += S [i, j] * lambda [j];
					}

					deltaLambda.Set ((1.0 / S [i, i]) * (B [i] - sum));
					sigma += deltaLambda.SqLength;
					lambda [i] += deltaLambda;
				}
			}

			var error = new double[2] {
				(lambda - lambda_old).Norm, // Absolute Error
				sigma / lambda.Length // Sigma
			};

			return error;
		}
	}

	public class NullWriter : TextWriter
	{
		public override System.Text.Encoding Encoding {
			get { return System.Text.Encoding.Default; }
		}

		public override void Write (char value)
		{
			// No-op
		}
	}
}
