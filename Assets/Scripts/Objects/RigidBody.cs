using System;
using UnityEngine;

namespace PE
{
	public class RigidBody : Entity
	{
		public Quaternion q = new Quaternion();
		public Vec3 omega = new Vec3();
		public Vec3 tau = new Vec3();
	}
}

