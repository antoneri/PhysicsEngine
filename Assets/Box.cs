using UnityEngine;
using System.Collections;

using PE;

public class Box : MonoBehaviour
{
	private Entity box = new Entity ();
	private Vector3 ColliderThickness = new Vector3 (0.2f, 0.2f, 0.2f);

	// Use this for initialization
	void Start ()
	{
		Vector3 min = transform.position - transform.lossyScale * 0.5f;
		Vector3 max = transform.position + transform.lossyScale * 0.5f;

		box.Collider = new AABB (min, max);
		box.Static = true;

        //growColliderBox();

		PE.Engine.instance.AddEntity (box);
	}
	
	// Update is called once per frame
	void Update ()
	{
        //growColliderBox();
        updateCollider();
	}

    private void updateCollider()
    {
        /* Update collider */
        AABB collider = (AABB)box.Collider;
        collider.min = transform.position - transform.lossyScale * 0.5f;
        collider.max = transform.position + transform.lossyScale * 0.5f;
    }

    private void growColliderBox()
    {
        /* Update collider */
        AABB collider = (AABB)box.Collider;
        collider.min = transform.position - transform.lossyScale * 0.5f - ColliderThickness;
        collider.max = transform.position + transform.lossyScale * 0.5f + ColliderThickness;
    }
}
