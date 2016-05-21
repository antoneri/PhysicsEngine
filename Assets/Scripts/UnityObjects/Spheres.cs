using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using PE;

public class Spheres : MonoBehaviour {

	private List<Sphere> spheres = new List<Sphere> ();
	private List<Transform> children = new List<Transform> ();
	
	public double mass = 1;
	public double radius = 0.5;

	// Use this for initialization
	void Start () {
		foreach (Transform child in transform) {
			children.Add (child);
			spheres.Add (new Sphere (child.position, radius, mass));
		}

		Engine.instance.Spheres = spheres;
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < children.Count; i++) {
			children[i].position = spheres [i].x;
			children[i].transform.rotation = spheres [i].q;
		}
	}
}
