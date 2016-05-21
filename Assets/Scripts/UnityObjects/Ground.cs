using UnityEngine;
using System.Collections;

using PE;

public class Ground : MonoBehaviour
{
	private Entity ground;

	// Use this for initialization
	void Start ()
	{
		ground = new Entity ();
		double d = transform.position.magnitude;
		ground.Collider = new Plane (new PE.Vec3 (0, 1.0, 0), d);
		ground.Static = true;

		PE.Engine.instance.AddEntity (ground);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
