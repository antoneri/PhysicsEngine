using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace PE
{
	public class Matrix<T> : IEnumerable<T>
	{
		protected T[] items;

		public Matrix (int rows, int cols)
		{
			Rows = rows;
			Cols = cols;
			Diagonal = true;
			items = new T[rows * cols];

			for (int i = 0; i < items.Length; i++) {
				items [i] = Activator.CreateInstance<T> ();
			}
		}

		public T this [int i, int j] {
			get {
				return items [i * Cols + j];
			}
			set {
				if (i != j)
					Diagonal = false;

				items [i * Cols + j] = value;
			}
		}

		// This accessor destroys diagonal optimization
		public T this [int i] {
			get {
				return items [i];
			}
			set {
				Diagonal = false;
				items [i] = value;
			}
		}

		public Matrix<T> Transpose {
			get {
				var transpose = new Matrix<T> (Cols, Rows);

				for (int i = 0; i < Rows; i++) {
					for (int j = 0; j < Cols; j++) {
						transpose [j, i] = this [i, j];
					}
				}

				return transpose;
			}
		}

		public bool Diagonal {
			get;
			private set;
		}

		public int Size {
			get {
				return Rows * Cols;
			}
		}

		public int Rows {
			get;
			private set;
		}

		public int Cols {
			get;
			private set;
		}

		public void ForEach (Action<T> action)
		{
			foreach (T item in items) {
				action (item);
			}
		}

		public IEnumerator<T> GetEnumerator ()
		{
			for (int i = 0; i < Size; i++) {
				yield return this [i];
			}
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			throw new NotImplementedException ();
		}

		public override string ToString ()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder ();
			for (int i = 0; i < Rows; i++) {
				sb.Append ("| ");
				for (int j = 0; j < Cols; j++) {
					sb.Append (this [i, j] + " ");
				}
				sb.Append ("|\n");
			}
			return sb.ToString ();
		}

		public override bool Equals (object obj)
		{
			Matrix<T> M = obj as Matrix<T>;

			if (M == null)
				return false;

			if (M.Rows != Rows || M.Cols != Cols)
				return false;

			for (int i = 0; i < Rows; i++) {
				for (int j = 0; j < Cols; j++) {
					if (!M [i, j].Equals (this [i, j]))
						return false;
				}
			}

			return true;
		}

		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}
	}

	/*
     * Vec3Matrix
     */
	public class Vec3Matrix : Matrix<Vec3>
	{
		public Vec3Matrix (int rows, int cols) : base (rows, cols)
		{
		}

		public new Vec3Matrix Transpose {
			get {
				var transpose = new Vec3Matrix (Cols, Rows);

				for (int i = 0; i < Rows; i++) {
					for (int j = 0; j < Cols; j++) {
						transpose [j, i] = this [i, j];
					}
				}

				return transpose;
			}
		}

		public static Vec3Vector operator * (Vec3Matrix M, Vec3Vector v)
		{
			if (M.Cols != v.Size) {
				throw new InvalidOperationException ("Invalid dimensions");
			}

			Vec3Vector vec = new Vec3Vector (M.Rows);

			if (M.Diagonal) {
				for (int i = 0; i < M.Rows; i++) {
					vec [i] = M [i, i] * v [i];
				}
			} else {
				for (int i = 0; i < M.Rows; i++) {
					for (int j = 0; j < M.Cols; j++) {
						vec [i].Add (M [i, j] * v [j]);
					}
				}
			}

			return vec;
		}

		public static Vec3Vector operator* (Vec3Matrix M, Vector<double> v)
		{
			if (M.Cols != v.Size) {
				throw new InvalidOperationException ("Invalid dimensions");
			}

			Vec3Vector vec = new Vec3Vector (M.Rows);

			if (M.Diagonal) {
				for (int i = 0; i < M.Rows; i++) {
					vec [i] = M [i, i] * v [i];
				}
			} else {
				for (int i = 0; i < M.Rows; i++) {
					for (int j = 0; j < M.Cols; j++) {
						vec [i].Add (M [i, j] * v [j]);
					}
				}
			}

			return vec;
		}

		public static Vec3Matrix operator* (Vec3Matrix M1, Vec3Matrix M2)
		{
			if (M1.Cols != M2.Rows) {
				throw new InvalidOperationException ("Invalid dimensions");
			}

			Vec3Matrix M = new Vec3Matrix (M1.Rows, M2.Cols);

			if (M1.Diagonal && M2.Diagonal) {
				for (int i = 0; i < M1.Rows; i++) {
					M [i, i] = M1 [i, i] * M2 [i, i];
				}
			} else if (M1.Diagonal) {
				for (int i = 0; i < M1.Rows; i++) {
					for (int j = 0; j < M2.Cols; j++) {
						M [i, j] = M1 [i, i] * M2 [i, j];
					}
				}
			} else if (M2.Diagonal) {
				for (int i = 0; i < M1.Rows; i++) {
					for (int j = 0; j < M2.Cols; j++) {
						M [i, j] = M1 [i, j] * M2 [j, j];
					}
				}
			} else {
				for (int i = 0; i < M1.Rows; i++) {
					for (int j = 0; j < M2.Cols; j++) {
						for (int k = 0; k < M1.Cols; k++) {
							M [i, j].Add (M1 [i, k] * M2 [k, j]);
						}
					}
				}
			}

			return M;
		}
	}

	/*
     * Mat3Matrix
     */
	public class Mat3Matrix : Matrix<Mat3>
	{
		public Mat3Matrix (int rows, int cols) : base (rows, cols)
		{
		}

		public new Mat3Matrix Transpose {
			get {
				var transpose = new Mat3Matrix (Cols, Rows);

				for (int i = 0; i < Rows; i++) {
					for (int j = 0; j < Cols; j++) {
						transpose [j, i] = this [i, j];
					}
				}

				return transpose;
			}
		}

		public static Vec3Vector operator* (Mat3Matrix M, Vec3Vector v)
		{
			if (M.Cols != v.Size) {
				throw new InvalidOperationException ("Invalid dimensions");
			}

			Vec3Vector vec = new Vec3Vector (M.Rows);

			if (M.Diagonal) {
				for (int i = 0; i < M.Rows; i++) {
					vec [i] = M [i, i] * v [i];
				}
			} else {
				for (int i = 0; i < M.Rows; i++) {
					for (int j = 0; j < M.Cols; j++) {
						vec [i].Add (M [i, j] * v [j]);
					}
				}
			}

			return vec;
		}

		public static Mat3Matrix operator * (Mat3Matrix M1, Mat3Matrix M2)
		{
			if (M1.Cols != M2.Rows) {
				throw new InvalidOperationException ("Invalid dimensions");
			}

			Mat3Matrix M = new Mat3Matrix (M1.Rows, M2.Cols);

			if (M1.Diagonal && M2.Diagonal) {
				for (int i = 0; i < M1.Rows; i++) {
					M [i, i] = M1 [i, i] * M2 [i, i];
				}
			} else if (M1.Diagonal) {
				for (int i = 0; i < M1.Rows; i++) {
					for (int j = 0; j < M2.Cols; j++) {
						M [i, j] = M1 [i, i] * M2 [i, j];
					}
				}
			} else if (M2.Diagonal) {
				for (int i = 0; i < M1.Rows; i++) {
					for (int j = 0; j < M2.Cols; j++) {
						M [i, j] = M1 [i, j] * M2 [j, j];
					}
				}
			} else {
				for (int i = 0; i < M1.Rows; i++) {
					for (int j = 0; j < M2.Cols; j++) {
						for (int k = 0; k < M1.Cols; k++) {
							M [i, j].Add (M1 [i, k] * M2 [k, j]);
						}
					}
				}
			}

			return M;
		}

		public static Vec3Matrix operator * (Mat3Matrix M1, Vec3Matrix M2)
		{
			if (M1.Cols != M2.Rows) {
				throw new InvalidOperationException ("Invalid dimensions");
			}

			Vec3Matrix M = new Vec3Matrix (M1.Rows, M2.Cols);

			if (M1.Diagonal && M2.Diagonal) {
				for (int i = 0; i < M1.Rows; i++) {
					M [i, i] = M1 [i, i] * M2 [i, i];
				}
			} else if (M1.Diagonal) {
				for (int i = 0; i < M1.Rows; i++) {
					for (int j = 0; j < M2.Cols; j++) {
						M [i, j] = M1 [i, i] * M2 [i, j];
					}
				}
			} else if (M2.Diagonal) {
				for (int i = 0; i < M1.Rows; i++) {
					for (int j = 0; j < M2.Cols; j++) {
						M [i, j] = M1 [i, j] * M2 [j, j];
					}
				}
			} else {
				for (int i = 0; i < M1.Rows; i++) {
					for (int j = 0; j < M2.Cols; j++) {
						for (int k = 0; k < M1.Cols; k++) {
							M [i, j].Add (M1 [i, k] * M2 [k, j]);
						}
					}
				}
			}

			return M;
		}

		public static Vec3Matrix operator * (Vec3Matrix M1, Mat3Matrix M2)
		{
			if (M1.Cols != M2.Rows) {
				throw new InvalidOperationException ("Invalid dimensions");
			}

			Vec3Matrix M = new Vec3Matrix (M1.Rows, M2.Cols);

			if (M1.Diagonal && M2.Diagonal) {
				for (int i = 0; i < M1.Rows; i++) {
					M [i, i] = M1 [i, i] * M2 [i, i];
				}
			} else if (M1.Diagonal) {
				for (int i = 0; i < M1.Rows; i++) {
					for (int j = 0; j < M2.Cols; j++) {
						M [i, j] = M1 [i, i] * M2 [i, j];
					}
				}
			} else if (M2.Diagonal) {
				for (int i = 0; i < M1.Rows; i++) {
					for (int j = 0; j < M2.Cols; j++) {
						M [i, j] = M1 [i, j] * M2 [j, j];
					}
				}
			} else {
				for (int i = 0; i < M1.Rows; i++) {
					for (int j = 0; j < M2.Cols; j++) {
						var e = new Vec3 ();
						for (int k = 0; k < M1.Cols; k++) {
							e.Add (M1 [i, k] * M2 [k, j]);
						}
						M [i, j] = e;
					}
				}
			}

			return M;
		}
	}

}

