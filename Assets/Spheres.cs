using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using PE;

public class Spheres : MonoBehaviour {

	private List<RigidBody> bodies = new List<RigidBody> ();
	double mass = 1;
	double radius = 0.5;
	Spheres[] spheres;

	// Use this for initialization
	void Start () {
		spheres = GetComponentsInChildren<Spheres> ();

		for (int i = 0; i < spheres.Length; i++) {
			var sphere = spheres [i];
			Vec3 x = sphere.transform.position;
			var I = 2 / 5 * mass * radius * radius;
			var body = new RigidBody (x, mass, I);
			body.Collider = new Sphere (x, radius);
			bodies.Add (body);
		}

		Engine.instance.RigidBodies = bodies;
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < spheres.Length; i++) {
			spheres [i].transform.position = bodies [i].x;
			spheres [i].transform.rotation = bodies [i].q;
		}
	}
}
