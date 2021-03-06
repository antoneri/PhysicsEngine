﻿using System;

namespace PE
{
	public class Mat3
	{
		private double[,] items = new double[3, 3];

		public Mat3 ()
		{
			Diagonal = true;
		}

		public double this [int i, int j] {
			get {
				return items [i, j];
			}
			set {
				if (i != j)
					Diagonal = false;
				
				items [i, j] = value;
			}
		}

		public bool Diagonal {
			get;
			private set;
		}

		public double Determinant {
			get {
				if (Diagonal)
					return this [0, 0] * this [1, 1] * this [2, 2];
				
				return this [0, 0] * (this [1, 1] * this [2, 2] - this [2, 1] * this [1, 2])
				- this [0, 1] * (this [1, 0] * this [2, 2] - this [1, 2] * this [2, 0])
				+ this [0, 2] * (this [1, 0] * this [2, 1] - this [1, 1] * this [2, 0]);
			}
		}

		public Mat3 Inverse {
			get {
				var inverse = new Mat3 ();

				if (Diagonal) {
					if (this [0, 0] == 0 || this [1, 1] == 0 || this [2, 2] == 0) {
						// If the matrix is bool diagonal = true, but one element was explicitly set to zero.
						throw new DivideByZeroException ("Diagonal element of diagonal matrix is zero!");
					}

					inverse [0, 0] = 1 / this [0, 0];
					inverse [1, 1] = 1 / this [1, 1];
					inverse [2, 2] = 1 / this [2, 2];

				} else {
					var d = 1 / Determinant;

					if (double.IsNaN (d)) {
						throw new DivideByZeroException ("Determinant is zero!");
					}

					inverse [0, 0] = (this [1, 1] * this [2, 2] - this [2, 1] * this [1, 2]) * d;
					inverse [0, 1] = -(this [0, 1] * this [2, 2] - this [0, 2] * this [2, 1]) * d;
					inverse [0, 2] = (this [0, 1] * this [1, 2] - this [0, 2] * this [1, 1]) * d;
					inverse [1, 0] = -(this [1, 0] * this [2, 2] - this [1, 2] * this [2, 0]) * d;
					inverse [1, 1] = (this [0, 0] * this [2, 2] - this [0, 2] * this [2, 0]) * d;
					inverse [1, 2] = -(this [0, 0] * this [1, 2] - this [1, 0] * this [0, 2]) * d;
					inverse [2, 0] = (this [1, 0] * this [2, 1] - this [2, 0] * this [1, 1]) * d;
					inverse [2, 1] = -(this [0, 0] * this [2, 1] - this [2, 0] * this [0, 1]) * d;
					inverse [2, 2] = (this [0, 0] * this [1, 1] - this [1, 0] * this [0, 1]) * d;
				}
	
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

		public static Mat3 Identity {
			get {
				return Diag (1);
			}
		}

		public static Mat3 Diag (double value)
		{
			return Diag (value, value, value);
		}

		public static Mat3 SkewSymmetric (Vec3 v)
		{
			Mat3 M = new Mat3 ();

			M [0, 1] = -v [2];
			M [0, 2] = v [1];
			M [1, 0] = v [2];
			M [1, 2] = -v [0];
			M [2, 0] = -v [1];
			M [2, 1] = v [0];
            
			return M;
		}

		public static Mat3 Diag (double i, double j, double k)
		{
			var diag = new Mat3 ();
			diag [0, 0] = i;
			diag [1, 1] = j;
			diag [2, 2] = k;
			return diag;
		}

		public static Mat3 operator+ (Mat3 lhs, Mat3 rhs)
		{
			var result = new Mat3 ();

			for (int i = 0; i < 3; i++) {
				for (int j = 0; j < 3; j++) {
					result [i, j] = lhs [i, j] + rhs [i, j];
				}
			}

			return result;
		}

		public static Mat3 operator- (Mat3 lhs, Mat3 rhs)
		{
			var result = new Mat3 ();

			for (int i = 0; i < 3; i++) {
				for (int j = 0; j < 3; j++) {
					result [i, j] = lhs [i, j] - rhs [i, j];
				}
			}

			return result;
		}

		public static Mat3 operator- (Mat3 mat)
		{
			return -1 * mat;
		}

		public static Mat3 operator* (Mat3 lhs, Mat3 rhs)
		{
			var result = new Mat3 ();

			if (lhs.Diagonal && rhs.Diagonal) {
				for (int i = 0; i < 3; i++) {
					result [i, i] = lhs [i, i] * rhs [i, i];
				}
			} else if (lhs.Diagonal) {
				for (int i = 0; i < 3; i++) {
					for (int j = 0; j < 3; j++) {
						result [i, j] = lhs [i, i] * rhs [i, j];
					}
				}
			} else if (rhs.Diagonal) {
				for (int i = 0; i < 3; i++) {
					for (int j = 0; j < 3; j++) {
						result [i, j] = lhs [i, j] * rhs [j, j];
					}
				}
			} else {
				for (int i = 0; i < 3; i++) {
					for (int j = 0; j < 3; j++) {
						double sum = 0;
						for (int k = 0; k < 3; k++) {
							sum += lhs [i, k] * rhs [k, j];
						}
						result [i, j] = sum;
					}
				}
			}

			return result;
		}

		public static Vec3 operator* (Mat3 lhs, Vec3 rhs)
		{
			var result = new Vec3 ();

			if (lhs.Diagonal) {
				for (int i = 0; i < 3; i++) {
					result [i] = lhs [i, i] * rhs [i];
				}
			} else {
				for (int i = 0; i < 3; i++) {
					double sum = 0;
					for (int k = 0; k < 3; k++) {
						sum += lhs [i, k] * rhs [k];
					}
					result [i] = sum;
				}
			}

			return result;
		}

		public static Vec3 operator* (Vec3 lhs, Mat3 rhs)
		{
			return rhs * lhs;
		}

		public static Mat3 operator* (double lhs, Mat3 rhs)
		{
			var result = new Mat3 ();

			if (rhs.Diagonal) {
				for (int i = 0; i < 3; i++) {
					result [i, i] = lhs * rhs [i, i];
				}
			} else {
				for (int i = 0; i < 3; i++) {
					for (int j = 0; j < 3; j++) {
						result [i, j] = lhs * rhs [i, j];
					}
				}
			}

			return result;
		}

		public static Mat3 operator* (Mat3 lhs, double rhs)
		{
			return rhs * lhs;
		}

		public void Add (Mat3 other)
		{
			if (other.Diagonal) {
				for (int i = 0; i < 3; i++) {
					this [i, i] += other [i, i];
				}
			} else {
				for (int i = 0; i < 3; i++) {
					for (int j = 0; j < 3; j++) {
						this [i, j] += other [i, j];
					}
				}
			}
		}

		public void Subtract (Mat3 other)
		{
			if (other.Diagonal) {
				for (int i = 0; i < 3; i++) {
					this [i, i] -= other [i, i];
				}
			} else {
				for (int i = 0; i < 3; i++) {
					for (int j = 0; j < 3; j++) {
						this [i, j] -= other [i, j];
					}
				}
			}
		}

		public void Set (Mat3 other)
		{
			for (int i = 0; i < 3; i++) {
				for (int j = 0; j < 3; j++) {
					this [i, j] = other [i, j];
				}
			}

			Diagonal = other.Diagonal;
		}

		public override string ToString ()
		{
			var sb = new System.Text.StringBuilder ();

			for (int i = 0; i < 3; i++) {
				sb.Append ("| ");
				for (int j = 0; j < 3; j++) {
					sb.Append (this [i, j] + " ");
				}
				sb.Append ("|\n");
			}
			return sb.ToString ();
		}
	}
}

