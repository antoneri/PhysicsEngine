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

			vertices = new Vector3[mesh.vertices.Length];

			// We assume that the mesh is square
			var size = Mathf.CeilToInt (Mathf.Sqrt (mesh.vertices.Length)); 

			particles = new ParticleMesh (size, size);

			for (int i = 0; i < particles.Size; i++) {
				particles [i] = new Particle (mesh.vertices [i], new Vec3 (), 0.1);
			}
		
			Engine.instance.AddParticleMesh (particles);
		}
	
		// Update is called once per frame
		void Update ()
		{
			var mesh = GetComponent<MeshFilter> ().mesh;

			for (int i = 0; i < particles.Size; i++) {
				vertices [i] = particles [i].x;
			}
	
			mesh.vertices = vertices;
		}
	}
}
