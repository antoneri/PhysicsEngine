using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour
{
    private Entity ground;

	// Use this for initialization
	void Start () {
        ground = new Entity();
        Vector3 position = transform.position;
        ground.setCollider(new Plane(new PE.Vec3(0, 1.0, 0), position));
        ground.setCollidable(true);
        ground.setStaticObject(true);

        PE.Engine.instance.AddEntity(ground);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
