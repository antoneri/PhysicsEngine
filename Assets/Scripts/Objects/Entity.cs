using UnityEngine;
using System.Collections;

public class Entity
{
	private Collider collider;
	private bool collidable = false;
	private bool staticObject = false;

	public Collider Collider {
		get {
			return collider;
		}
		set {
			collider = value;
			collidable = true;
		}
	}

	public bool Collidable {
		get {
			return collidable;
		}
		set {
			collidable = value;
		}
	}

	public bool Static {
		get {
			return staticObject;
		}
		set {
			staticObject = value;
		}
	}
}
