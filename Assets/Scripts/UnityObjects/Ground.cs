using UnityEngine;
using System.Collections;

using PE;

public class Ground : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
		Entity ground = new RigidBody ();
		ground.m_inv = Mat3.Diag (0);
		ground.I_inv = Mat3.Diag (0);

		double d = transform.position.magnitude;
		var collider = new PE.Plane (new PE.Vec3 (0, 1.0, 0), d);

		collider.self = ground;
		ground.Collider = collider;

		PE.Engine.instance.AddEntity (ground);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
