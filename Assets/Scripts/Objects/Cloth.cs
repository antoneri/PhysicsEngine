using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PE
{
	public class Cloth : MonoBehaviour
	{
		List<PE.Particle> particles = new List<PE.Particle> ();

		private class Size
		{
			public int x;
			public int y;

			public Size (int x, int y)
			{
				this.x = x;
				this.y = y;
			}
		}

		private class MeshMatrix
		{
			private int rows;
			private int cols;
			public PE.Vec3[,] vertices;

			public MeshMatrix (Vector3[] v)
			{
				rows = cols = Mathf.CeilToInt (Mathf.Sqrt (v.Length));
				vertices = new PE.Vec3[rows, cols];

				for (int i = 0; i < rows; i++) {
					for (int j = 0; j < cols; j++) {
						this [i, j] = v [i * rows + j];
					}
				}
			}

			public Size Size {
				get {
					return new Size (rows, cols);
				}
			}

			public PE.Vec3 this [int i, int j] {
				get {
					return vertices [i, j];
				}
				set {
					vertices [i, j] = value;
				}
			}

			public static implicit operator Vector3[] (MeshMatrix m)
			{
				var size = m.Size;
				Vector3[] v = new Vector3[size.x * size.y];
				for (int i = 0; i < size.x; i++) {
					for (int j = 0; j < size.y; j++) {
						v [i * size.x + j] = m [i, j];
					}
				}
				return v;
			}
		}

		// Use this for initialization
		void Start ()
		{
		}
	
		// Update is called once per frame
		void Update ()
		{
			Mesh mesh = GetComponent<MeshFilter> ().mesh;
			var matrix = new MeshMatrix (mesh.vertices);

			for (int i = 0; i < matrix.Size.x; i++) {
				for (int j = 0; j < matrix.Size.y; j++) {
					matrix [i, j].x += 0.1f;
				}
			}

			mesh.vertices = matrix;
		}
	}
}
