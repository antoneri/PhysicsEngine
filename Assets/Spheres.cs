using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using PE;

public class Spheres : MonoBehaviour {

	private List<RigidBody> bodies = new List<RigidBody> ();
	private List<Transform> children = new List<Transform> ();

	double mass = 1;
	double radius = 0.5;

	// Use this for initialization
	void Start () {
		foreach (Transform child in transform) {
			children.Add (child);
			var I = 2 / 5 * mass * radius * radius;
			var body = new RigidBody (child.position, mass, I);
			bodies.Add (body);
		}

		Engine.instance.RigidBodies = bodies;
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < children.Count; i++) {
			children[i].position = bodies [i].x;
			children[i].transform.rotation = bodies [i].q;
		}
	}
}
