using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PE
{
	public class Cloth : MonoBehaviour
	{
		ParticleMesh particles;
		Vector3[] vertices;

		public double k_stretch = 54;
		public double k_shear = 0;
		public double k_bend = 0;
		public double kd = 0.01;
		public double mass = 0.025;
	    private double density = 1;
        private double radius = 0.25;

		// Use this for initialization
		void Start ()
		{
			var mesh = GetComponent<MeshFilter> ().mesh;
			mesh.MarkDynamic ();
			
			vertices = new Vector3[mesh.vertices.Length];

			// We assume that the mesh is square
			var N = Mathf.CeilToInt (Mathf.Sqrt (mesh.vertices.Length)); 

			particles = new ParticleMesh (N, N);

			for (int i = 0; i < particles.Size; i++) {
				var worldPosition = transform.TransformPoint (mesh.vertices [i]);
				particles [i] = new Particle (worldPosition, new Vec3 (), mass, radius, density);
			}

			// Springs between particles
			List<Spring> neighbors = new List<Spring> ();

			for (int i = 0; i < particles.Rows; i++) {
				for (int j = 0; j < particles.Cols; j++) {
					var current = particles [i, j];
					//      1
					//    /
					//  * - 2
					//  | \
					//  4   3

					//if (i - 1 >= 0 && j + 1 < particles.Cols)
					//	neighbors.Add (new Spring (current, particles [i - 1, j + 1], k_shear, kd));
					if (j + 1 < particles.Cols)
						neighbors.Add (new Spring (current, particles [i, j + 1], k_stretch, kd));
					//if (i + 1 < particles.Rows && j + 1 < particles.Cols)
					//	neighbors.Add (new Spring (current, particles [i + 1, j + 1], k_shear, kd));
					if (i + 1 < particles.Rows)
						neighbors.Add (new Spring (current, particles [i + 1, j], k_stretch, kd));
				}
			}

			if (neighbors.Count != 4 * N * N - 6 * N + 2) {
				//throw new MissingComponentException ("We are missing some springs!");
			}

			List<Spring> nextNextNeighbors = new List<Spring> ();

			for (int i = 0; i < particles.Rows; i++) {
				for (int j = 0; j < particles.Cols; j++) {
					var current = particles [i, j];

					if (i + 2 < particles.Rows)
						nextNextNeighbors.Add (new Spring (current, particles [i + 2, j], k_bend, kd));
					if (j + 2 < particles.Cols)
						nextNextNeighbors.Add (new Spring (current, particles [i, j + 2], k_bend, kd));
				}
			}

			particles.Neighbors = neighbors;
			
			Engine.instance.AddParticleMesh (particles);
		}
	
		// Update is called once per frame
		void Update ()
		{
			var mesh = GetComponent<MeshFilter> ().mesh;

			for (int i = 0; i < particles.Size; i++) {
				vertices [i] = transform.InverseTransformPoint (particles [i].x);
			}
	
			mesh.vertices = vertices;
		}
	}
}
