﻿using UnityEngine;
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
            //Debug.Log("lossy scale: " + child.lossyScale);
            var avgScale = (child.lossyScale.x + child.lossyScale.y + child.lossyScale.z) / 3;
            spheres.Add (new Sphere (child.position, avgScale/2, mass*avgScale));
		}
        //spheres[0].m_inv = new Mat3();
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
