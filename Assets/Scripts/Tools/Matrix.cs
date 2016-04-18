using System;

namespace PE
{
	public class Matrix<T>
	{
		protected int rows;
		protected int cols;
		protected T[,] items;

		public Matrix (int rows, int cols)
		{
			this.rows = rows;
			this.cols = cols;
			items = new T[rows, cols];
		}

		public T this [int i, int j] {
			get {
				return items [i, j];
			}
			set {
				items [i, j] = value;
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
	}
}

