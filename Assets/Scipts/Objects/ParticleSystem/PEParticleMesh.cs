using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PE
{
	public class ParticleMesh : MonoBehaviour
	{
		public List<Particle> particles;
		const double dx = 0.1;

		// Update is called once per frame
		void Update ()
		{
			Mesh mesh = GetComponent<MeshFilter> ().mesh;
			mesh.Clear ();

			mesh.vertices = new Vector3[4 * particles.Count];


			for (int i = 0; i < particles.Count; i++) {
				var idx = 4 * i;
				mesh.vertices [idx] = PE.UnityAdapter.UnityVector(particles [i].x);
				mesh.vertices [idx + 1] = PE.UnityAdapter.UnityVector(new Vec3(particles[i].x.x, particles[i].x.y + dx, particles[i].x.z));
				mesh.vertices [idx + 2] = PE.UnityAdapter.UnityVector(new Vec3(particles[i].x.x + dx, particles[i].x.y + dx, particles[i].x.z));
				mesh.vertices [idx + 3] = PE.UnityAdapter.UnityVector(new Vec3(particles[i].x.x + dx, particles[i].x.y, particles[i].x.z));
			}

			mesh.triangles = new int[6 * mesh.vertices.Length];

			for (int i = 0; i < mesh.vertices.Length; i++) {
				var idx = 4 * i;
				mesh.triangles [6 * i] = idx;
				mesh.triangles [6 * i + 1] = idx + 1;
				mesh.triangles [6 * i + 2] = idx + 2;
				mesh.triangles [6 * i + 3] = idx;
				mesh.triangles [6 * i + 4] = idx + 2;
				mesh.triangles [6 * i + 5] = idx + 3;
			}
		}

		void SetParticles (List<Particle> p)
		{
			particles = p;
		}
	}
}
