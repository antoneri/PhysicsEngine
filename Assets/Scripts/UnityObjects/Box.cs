using UnityEngine;
using System.Collections;

using PE;

public class Box : MonoBehaviour
{
	private Entity box = new Entity ();
	private Vector3 ColliderThickness = new Vector3 (0.1f, 0.1f, 0.1f);

	// Use this for initialization
	void Start ()
	{
		Vector3 min = transform.position - transform.lossyScale * 0.5f;
		Vector3 max = transform.position + transform.lossyScale * 0.5f;

		box.Collider = new AABB (min, max);

		Engine.instance.AddEntity (box);
	}
	
	// Update is called once per frame
	void Update ()
	{
		AABB collider = (AABB)box.Collider;
        collider.min = transform.position - transform.lossyScale * 0.5f - ColliderThickness;
        collider.max = transform.position + transform.lossyScale * 0.5f + ColliderThickness;
	}
}
