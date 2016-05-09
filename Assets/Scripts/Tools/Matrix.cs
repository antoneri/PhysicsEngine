using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace PE
{
	public class Matrix<T>: IEnumerable<T>
	{
		protected int rows;
		protected int cols;
		protected T[] items;
		protected Matrix<T> transpose = null;

		public Matrix (int rows, int cols)
		{
			this.rows = rows;
			this.cols = cols;
			items = new T[rows * cols];

			for (int i = 0; i < items.Length; i++) {
				items [i] = Activator.CreateInstance<T> ();
			}
		}

		public T this [int i, int j] {
			get {
				return items [i * cols + j];
			}
			set {
				items [i * cols + j] = value;
			}
		}

		public T this [int i] {
			get {
				return items [i];
			}
			set {
				items [i] = value;
			}
		}

		public int Size {
			get {
				return rows * cols;
			}
		}

		public int Rows {
			get {
				return rows;
			}
		}

		public int Cols {
			get {
				return cols;
			}
		}

		public Matrix<T> Transpose {
			get {
				// FIXME source of bugs
				if (transpose == null) {
					transpose = new Matrix<T> (Cols, Rows);

					for (int i = 0; i < Rows; i++) {
						for (int j = 0; j < Cols; j++) {
							transpose [j, i] = this [i, j];
						}
					}
				}

				return transpose;
			}
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

		public static Vector<Vec3> operator * (Matrix<T> M, Vector<Vec3> v)
		{
			if (M.Cols != v.Size) {
				throw new InvalidOperationException ("Invalid dimensions");
			}

			Vector<Vec3> nv = new Vector<Vec3> (M.Rows);

			for (int i = 0; i < M.Rows; i++) {
				Vec3 e = new Vec3 ();
				for (int j = 0; j < M.Cols; j++) {
					e = e + (Vec3)(object)M [i, j] * v [j];
				}
				nv [i] = e;
			}

			return nv;
		}

		public static Vector<Vec3> operator * (Matrix<T> M, Vector<double> v)
		{
			if (M.Cols != v.Size) {
				throw new InvalidOperationException ("Invalid dimensions");
			}

			var nv = new Vector<Vec3> (M.Rows);

			for (int i = 0; i < M.Rows; i++) {
				var e = new Vec3 ();
				for (int j = 0; j < M.Cols; j++) {
					e = e + (Vec3)(object)M [i, j] * v [j];
				}
				nv [i] = e;
			}

			return nv;
		}

		public static Matrix<Vec3> operator * (Matrix<T> M1, Matrix<Vec3> M2)
		{
			if (M1.Cols != M2.Rows) {
				throw new InvalidOperationException ("Invalid dimensions");
			}

			Matrix<Vec3> nM = new Matrix<Vec3> (M1.Rows, M2.Cols);

			for (int i = 0; i < M1.Rows; i++) {
				for (int j = 0; j < M2.Cols; j++) {
					Vec3 e = new Vec3 ();
					for (int k = 0; k < M1.Cols; k++) {
						e = e + (Vec3)(Object)M1 [i, k] * M2 [k, j];
					}
					nM [i, j] = e;
				}
			}

			return nM;
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

	}

}

