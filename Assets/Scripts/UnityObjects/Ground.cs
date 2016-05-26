using UnityEngine;
using System.Collections;

using PE;

public class Ground : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
		var ground = new Entity ();
		ground.m_inv = Mat3.Diag (0);
		ground.I_inv = Mat3.Diag (0);

		double d = transform.position.magnitude;
		ground.Collider = new PlaneCollider (ground, new PE.Vec3 (0, 1.0, 0), d);

		PE.Engine.instance.AddEntity (ground);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
