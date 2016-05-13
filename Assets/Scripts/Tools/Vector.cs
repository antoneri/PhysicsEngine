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

        public static Vec3Vector operator *(Vector<T> v1, Vec3Vector v2)
        {
            Vec3Vector nv = new Vec3Vector(v1.size);
            for (int i = 0; i < v1.size; i++)
            {
                nv[i] = (double)(object)v1[i] * v2[i];
            }
            return nv;
        }

        public static Vector<double> operator * (double k, Vector<T> v)
        {
            Vector<double> nv = new Vector<double>(v.size);
            for (int i = 0; i < v.size; i++)
            {
                nv[i] = k * (double)(object)v[i];
            }
            return nv;
        }

        //public static Vector<Vec3> operator * (double k, Vector<T> v)
        //{
        //    Vector<Vec3> nv = new Vector<Vec3>(v.size);
        //    for (int i = 0; i < v.size; i++)
        //    {
        //        nv[i] = k * (Vec3)(object)v[i];
        //    }
        //    return nv;
        //}

        //public static Vector<Vec3> operator +(Vector<Vec3> v1, Vector<T> v2)
        //{
        //    if (v1.Size != v2.Size)
        //    {
        //        throw new InvalidOperationException("Invalid dimensions");
        //    }

        //    Vector<Vec3> nv = new Vector<Vec3>(v1.size);
        //    for (int i = 0; i < v1.size; i++)
        //    {
        //        nv[i] = v1[i] + (Vec3)(object)v2[i];
        //    }
        //    return nv;
        //}

        //public static Vector<Vec3> operator -(Vector<Vec3> v1, Vector<T> v2)
        //{
        //        if (v1.Size != v2.Size)
        //    {
        //        throw new InvalidOperationException("Invalid dimensions");
        //    }

        //    Vector<Vec3> nv = new Vector<Vec3>(v1.size);
        //    for (int i = 0; i < v1.size; i++)
        //    {
        //        nv[i] = v1[i] - (Vec3)(object)v2[i];
        //    }
        //    return nv;
        //}

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("[ ");
            foreach (var e in items)
            {
                sb.Append(e.ToString() + "\n");
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

    public class Vec3Vector : Vector<Vec3>
    {
        public Vec3Vector(int size) : base(size)
        {

        }

        public Vec3Vector(Vec3Vector v) : base(v.Size)
        {
            for (int i = 0; i < Size; i++)
            {
                this[i] = new Vec3(v[i]);
            }
        }

        public double Norm
        {
            get
            {
                return Math.Sqrt(Dot(this, this));
            }
        }

        public static double Dot(Vec3Vector v1, Vec3Vector v2)
        {
            double dot = 0;
            for (int i = 0; i < v1.Length; i++)
            {
                dot += Vec3.Dot(v1[i], v2[i]);
            }
            return dot;
        }

        public static Vec3Vector operator *(Vector<double> v1, Vec3Vector v2)
        {
            Vec3Vector nv = new Vec3Vector(v1.Size);
            for (int i = 0; i < v1.Size; i++)
            {
                nv[i] = v1[i] * v2[i];
            }
            return nv;
        }

        public static Vec3Vector operator *(double k, Vec3Vector v)
        {
            Vec3Vector nv = new Vec3Vector(v.Size);
            for (int i = 0; i < v.Size; i++)
            {
                nv[i] = k * v[i];
            }
            return nv;
        }

        public static Vec3Vector operator +(Vec3Vector v1, Vec3Vector v2)
        {
            if (v1.Size != v2.Size)
            {
                throw new InvalidOperationException("Invalid dimensions");
            }

            Vec3Vector nv = new Vec3Vector(v1.Size);
            for (int i = 0; i < v1.Size; i++)
            {
                nv[i] = v1[i] + (Vec3)(object)v2[i];
            }
            return nv;
        }

        public static Vec3Vector operator -(Vec3Vector v1, Vec3Vector v2)
        {
            if (v1.Size != v2.Size)
            {
                throw new InvalidOperationException("Invalid dimensions");
            }

            Vec3Vector nv = new Vec3Vector(v1.Size);
            for (int i = 0; i < v1.Size; i++)
            {
                nv[i] = v1[i] - (Vec3)(object)v2[i];
            }
            return nv;
        }
    }
}

