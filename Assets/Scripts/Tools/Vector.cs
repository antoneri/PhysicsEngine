using System;
using System.Collections;
using System.Collections.Generic;

namespace PE
{
	public class Vector<T>: IEnumerable<T>
	{
		int size;
		T[] items;

		public Vector (int size)
		{
			this.size = size;
			items = new T[size];
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
				return size;
			}
		}

		public int Length {
			get {
				return Size;
			}
		}

		public int Count {
			get {
				return Size;
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

