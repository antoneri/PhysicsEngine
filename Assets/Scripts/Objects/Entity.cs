using UnityEngine;
using System.Collections;

public class Entity {
	
    private Collider collider;
    private bool collidable = false;
    private bool staticObject = false;
    



    public void setStaticObject(bool b)
    {
        staticObject = b;
    }

    public bool isStatic()
    {
        return staticObject;
    }

    public Collider getCollider()
    {
        return collider;
    }

    public void setCollider(Collider c)
    {
        collider = c;
        collidable = true;
    }

    public void setCollidable(bool b)
    {
        collidable = b;
    }

    public bool isCollidable()
    {
        return collidable;
    }

}
