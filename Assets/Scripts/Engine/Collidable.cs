using System;

namespace PE
{
	public class Collidable
	{
		public Collidable ()
		{
			Static = false;
		}

		public Collider Collider { get; set; }

		public bool Static { get; set; }
	}
}

