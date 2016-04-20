using UnityEngine;
using System.Collections;

using PE;

public class Box : MonoBehaviour {

    private Entity box;

    // Use this for initialization
    void Start () {
        box = new Entity();

        Vector3 min = transform.position - transform.lossyScale * 0.5f;
        Vector3 max = transform.position + transform.lossyScale * 0.5f;
        
        AABB aabb = new AABB(min, max);
        box.setCollider(aabb);
        box.setStaticObject(true);

        PE.Engine.instance.AddEntity(box);
    }
	
	// Update is called once per frame
	void Update () {
        /* Update collider */
        AABB collider = (AABB) box.getCollider();
        collider.min = transform.position - transform.lossyScale * 0.5f;
        collider.max = transform.position + transform.lossyScale * 0.5f;
    }
}
