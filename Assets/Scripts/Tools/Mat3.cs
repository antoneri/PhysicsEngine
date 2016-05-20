using System;

namespace PE
{
	public class Mat3
	{
		private double[,] items = new double[3,3];

		public Mat3 ()
		{
		}

		public double this[int i, int j] {
			get {
				return items [i, j];
			}
			set {
				items [i, j] = value;
			}
		}

		public double Determinant {
			get {
				return this [0, 0] * (this [1, 1] * this [2, 2] - this [2, 1] * this [1, 2])
					- this [0, 1] * (this [1, 0] * this [2, 2] - this [1, 2] * this [2, 0])
					+ this [0, 2] * (this [1, 0] * this [2, 1] - this [1, 1] * this [2, 0]);
			}
		}

		public Mat3 Inverse {
			get {
				var d = Determinant;
				var inverse = new Mat3 ();
				inverse [0, 0] = (this [1, 1] * this [2, 2] - this [2, 1] * this [1, 2]) * d;
				inverse [0, 1] = -(this [0, 1] * this [2, 2] - this [0, 2] * this [2, 1]) * d;
				inverse [0, 2] = (this [0, 1] * this [1, 2] - this [0, 2] * this [1, 1]) * d;
				inverse [1, 0] = -(this [1, 0] * this [2, 2] - this [1, 2] * this [2, 0]) * d;
				inverse [1, 1] = (this [0, 0] * this [2, 2] - this [0, 2] * this [2, 0]) * d;
				inverse [1, 2] = -(this [0, 0] * this [1, 2] - this [1, 0] * this [0, 2]) * d;
				inverse [2, 0] = (this [1, 0] * this [2, 1] - this [2, 0] * this [1, 1]) * d;
				inverse [2, 1] = -(this [0, 0] * this [2, 1] - this [2, 0] * this [0, 1]) * d;
				inverse [2, 2] = (this [0, 0] * this [1, 1] - this [1, 0] * this [0, 1]) * d;
				return inverse;
			}
		}

		public Mat3 Transpose {
			get {
				var transpose = new Mat3 ();
				for (int i = 0; i < 3; i++) {
					for (int j = 0; j < 3; j++) {
						transpose [i, j] = this [j, i];
					}
				}
				return transpose;
			}
		}

		public static Mat3 operator+(Mat3 lhs, Mat3 rhs) {
			var result = new Mat3();

			for (int i = 0; i < 3; i++) {
				for (int j = 0; j < 3; j++) {
					result [i, j] = lhs [i, j] + rhs [i, j];
				}
			}

			return result;
		}

		public static Mat3 operator-(Mat3 lhs, Mat3 rhs) {
			var result = new Mat3();

			for (int i = 0; i < 3; i++) {
				for (int j = 0; j < 3; j++) {
					result [i, j] = lhs [i, j] - rhs [i, j];
				}
			}

			return result;
		}

		public static Mat3 operator*(Mat3 lhs, Mat3 rhs) {
			var result = new Mat3();

			for (int i = 0; i < 3; i++) {
				for (int j = 0; j < 3; j++) {
					double sum = 0;
					for (int k = 0; k < 3; k++) {
						sum += lhs [i, k] * rhs [k, j];
					}
					result [i, j] = sum;
				}
			}

			return result;
		}

		public static Vec3 operator*(Mat3 lhs, Vec3 rhs) {
			var result = new Vec3();

			for (int i = 0; i < 3; i++) {
				double sum = 0;
				for (int k = 0; k < 3; k++) {
					sum += lhs [i, k] * rhs [k];
				}
				result [i] = sum;
			}

			return result;
		}

		public static Mat3 operator*(double lhs, Mat3 rhs) {
			var result = new Mat3();

			for (int i = 0; i < 3; i++) {
				for (int j = 0; j < 3; j++) {
					result [i, j] = lhs * rhs [i, j];
				}
			}

			return result;
		}
	}
}

