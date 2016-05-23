using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PE
{
	public class Entity : Collidable
	{
		public Vec3 x = new Vec3 ();
		public Vec3 v = new Vec3 ();
		public Vec3 f = new Vec3 ();

		public Quaternion q = new Quaternion ();
		public Vec3 omega = new Vec3 ();
		public Vec3 tau = new Vec3 ();

		public Mat3 m = new Mat3 ();
		public Mat3 I = new Mat3 ();
		public Mat3 m_inv = new Mat3 ();
		public Mat3 I_inv = new Mat3 ();

		public Mat3 K = Mat3.Diag (1);
	}
}
