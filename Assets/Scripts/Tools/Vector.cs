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

            for (int i = 0; i < items.Length; i++)
            {
                items[i] = Activator.CreateInstance<T>();
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

        public static Vector<Vec3> operator * (double k, Vector<T> v)
        {
            Vector<Vec3> nv = new Vector<Vec3>(v.size);
            for (int i = 0; i < v.size; i++)
            {
                nv[i] = k * (Vec3)(object)v[i];
            }
            return nv;
        }

        public static Vector<Vec3> operator -(Vector<T> v1, Vector<T> v2)
        {
                if (v1.Size != v2.Size)
            {
                throw new InvalidOperationException("Invalid dimensions");
            }

            Vector<Vec3> nv = new Vector<Vec3>(v1.size);
            for (int i = 0; i < v1.size; i++)
            {
                nv[i] = (Vec3)(object)v1[i] - (Vec3)(object)v2[i];
            }
            return nv;
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("[ ");
            foreach (var e in items)
            {
                sb.Append(e.ToString() + " ");
            };
            sb.Append("]");
            return sb.ToString();
        }

        public override bool Equals(object obj)
        {

            Vector<T> v = obj as Vector<T>;
            if (v == null)
                return false;

            if (v.Length != items.Length)
                return false;
  
            for (int i = 0; i < v.Length; i++)
            {
                if (!v[i].Equals(items[i]))
                    return false;
            }

            return true;
        }
    }
}

