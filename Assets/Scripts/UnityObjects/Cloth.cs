﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PE
{
	public class Cloth : MonoBehaviour
	{
		ParticleMesh particles;
		Vector3[] vertices;

		public float k_stretch = 600;
        public float k_shear = 200;
        public float k_bend = 10;
        public float kd = 5;
        public float mass = 0.1f;

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
				particles [i] = new Particle (worldPosition, mass);
			}

			// Springs between particles
			var neighbors = new List<Spring> ();

			for (int i = 0; i < particles.Rows; i++) {
				for (int j = 0; j < particles.Cols; j++) {
					var current = particles [i, j];
					//      1
					//    /
					//  * - 2 - 5
					//  | \
					//  4   3
					//  |
					//  6

					if (i - 1 >= 0 && j + 1 < particles.Cols) {
						var neighbor = particles [i - 1, j + 1];
						neighbors.Add (new Spring (current, neighbor, k_shear, kd));
					}

					if (j + 1 < particles.Cols) {
						var neighbor = particles [i, j + 1];
						neighbors.Add (new Spring (current, neighbor, k_stretch, kd));
					}

					if (i + 1 < particles.Rows && j + 1 < particles.Cols) {
						var neighbor = particles [i + 1, j + 1];
						neighbors.Add (new Spring (current, neighbor, k_shear, kd));
					}

					if (i + 1 < particles.Rows) {
						var neighbor = particles [i + 1, j];
						neighbors.Add (new Spring (current, neighbor, k_stretch, kd));
					}

					if (i + 2 < particles.Rows) {
						var nextNeighbor = particles [i + 2, j];
						neighbors.Add (new Spring (current, nextNeighbor, k_bend, kd));
					}

					if (j + 2 < particles.Cols) {
						var nextNeighbor = particles [i, j + 2];
						neighbors.Add (new Spring (current, nextNeighbor, k_bend, kd));
					}
				}
			}

			particles.Neighbors = neighbors;
			
			Engine.instance.Cloth = particles;
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
