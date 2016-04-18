using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PE
{
	public class Cloth : MonoBehaviour
	{
		ParticleMesh particles;
		Vector3[] vertices;

		// Use this for initialization
		void Start ()
		{
			var mesh = GetComponent<MeshFilter> ().mesh;
			mesh.MarkDynamic ();

			// We assume that the mesh is square
			var size = Mathf.CeilToInt (Mathf.Sqrt (mesh.vertices.Length)); 

			particles = new ParticleMesh (size, size);
			vertices = new Vector3[mesh.vertices.Length];

			for (int i = 0; i < particles.Rows; i++) {
				for (int j = 0; j < particles.Cols; j++) {
					var v = mesh.vertices;
					particles [i, j] = new Particle (v [i * particles.Cols + j], new Vec3 (), 0.1);
				}
			}

			Engine.instance.AddParticleMesh (particles);
		}
	
		// Update is called once per frame
		void Update ()
		{
			var mesh = GetComponent<MeshFilter> ().mesh;

			for (int i = 0; i < particles.Rows; i++) {
				for (int j = 0; j < particles.Cols; j++) {
					vertices [i * particles.Cols + j] = particles [i, j].x;
				}
			}

			mesh.vertices = vertices;
		}
	}
}
