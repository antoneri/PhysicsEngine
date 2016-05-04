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
	}

}

