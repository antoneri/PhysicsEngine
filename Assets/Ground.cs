using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour
{
    private Entity ground;

	// Use this for initialization
	void Start () {
        ground = new Entity();
        double d = transform.position.magnitude;
        ground.setCollider(new Plane(new PE.Vec3(0, 1.0, 0), d));
        ground.setCollidable(true);
        ground.setStaticObject(true);

        PE.Engine.instance.AddEntity(ground);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
