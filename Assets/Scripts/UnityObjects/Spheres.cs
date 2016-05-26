using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using PE;

public class Spheres : MonoBehaviour
{
	private List<Sphere> spheres = new List<Sphere> ();
	private List<Transform> children = new List<Transform> ();
	
	public double density = 1;

	// Use this for initialization
	void Start ()
	{
		foreach (Transform child in transform) {
			children.Add (child);
			var avgScale = (child.lossyScale.x + child.lossyScale.y + child.lossyScale.z) / 3;
			double r = avgScale / 2;
			double volume = 4 / 3 * Mathf.PI * r * r * r;
			spheres.Add (new Sphere (child.position, r, density * volume));
		}

		Engine.instance.Spheres = spheres;
	}
	
	// Update is called once per frame
	void Update ()
	{
		for (int i = 0; i < children.Count; i++) {
			children [i].position = spheres [i].x;
			children [i].transform.rotation = spheres [i].q;
		}
	}
}
