using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace PE
{
	public class Intersection
	{
		public Entity self;
		public Entity entity;
		public double distance;
		public Vec3 normal;
		public Vec3 point;
		public int i;
		public int j;

		public override string ToString ()
		{
			return distance + "\n" + normal.ToString () + "\n" + point.ToString ();
		}
	}

	public abstract class Collider
	{
		public readonly List<Intersection> CollisionNotImplemented = new List<Intersection> ();

		public abstract List<Intersection> Collides (IEnumerable<Particle> ps);

		public abstract List<Intersection> Collides (Sphere b);

		public static List<Intersection> Collides (IEnumerable<Particle> ps, PlaneCollider pl)
		{
			List<Intersection> intersections = new List<Intersection> ();
			foreach (var p in ps) {
				double d = Math.Abs (Vec3.Dot (p.x, pl.normal) - pl.d);
				if (d < PlaneCollider.thickness) {
					Intersection data = new Intersection {
						entity = p,
						distance = PlaneCollider.thickness - d,
						normal = pl.normal,
						point = (p.x - (Vec3.Dot (pl.normal, p.x) + pl.d) * pl.normal) + PlaneCollider.thickness * pl.normal
					};
					intersections.Add (data);
				}
			}
			return intersections;
		}

		public static List<Intersection> Collides (IEnumerable<Particle> ps, AABB b)
		{
			List<Intersection> intersections = new List<Intersection> ();
			foreach (var p in ps) {
				if (p.x.x < b.max.x && p.x.x > b.min.x &&
				    p.x.y < b.max.y && p.x.y > b.min.y &&
				    p.x.z < b.max.z && p.x.z > b.min.z) {

					Vec3 n = PointNormalAABB (p.x, b);
					Vec3 AABBPoint = ClosestPointAABB (p.x, b, n);
					Intersection data = new Intersection {
						entity = p,
						distance = 0,
						normal = n,
						point = AABBPoint
					};
					intersections.Add (data);
				}
			}
			return intersections;
		}

		/* AABB intersect data */
		public static Vec3 PointNormalAABB (Vec3 p, AABB b)
		{
			Vec3 normal = new Vec3 ();
			double minDistance = double.MaxValue;

			if (Math.Abs (b.min.x - p.x) < minDistance) {
				normal.Set (-1, 0, 0);
				minDistance = Math.Abs (b.min.x - p.x);
			}
			if (Math.Abs (b.max.x - p.x) < minDistance) {
				normal.Set (1, 0, 0);
				minDistance = Math.Abs (b.max.x - p.x);
			}
			if (Math.Abs (b.min.y - p.y) < minDistance) {
				normal.Set (0, -1, 0);
				minDistance = Math.Abs (b.min.y - p.y);
			}
			if (Math.Abs (b.max.y - p.y) < minDistance) {
				normal.Set (0, 1, 0);
				minDistance = Math.Abs (b.max.y - p.y);
			}
			if (Math.Abs (b.min.z - p.z) < minDistance) {
				normal.Set (0, 0, -1);
				minDistance = Math.Abs (b.min.z - p.z);
			}
			if (Math.Abs (b.max.z - p.z) < minDistance) {
				normal.Set (0, 0, 1);
				minDistance = Math.Abs (b.max.z - p.z);
			}

			return normal;
		}

		public static Vec3 ClosestPointAABB (Vec3 p, AABB b, Vec3 normal)
		{
			Vec3 proj = new Vec3 ();

			if (normal.x + normal.y + normal.z > 0) {
				proj = p - Vec3.Dot (p - b.max, normal) * normal;
			} else {
				proj = p - Vec3.Dot (p - b.min, normal) * normal;
			}

			return proj;
		}

		public static double SqDistPointAABB (Vec3 p, AABB b)
		{
			double sqDist = 0.0f;
			for (int i = 0; i < 3; i++) {
				// For each axis count any excess distance outside box extents
				double v = p [i];
				if (v < b.min [i])
					sqDist += (b.min [i] - v) * (b.min [i] - v);
				if (v > b.max [i])
					sqDist += (v - b.max [i]) * (v - b.max [i]);
			}
			return sqDist;
		}

		public static List<Intersection> Collides (Sphere sphere, PlaneCollider plane)
		{
			var intersections = new List<Intersection> ();

			double dist = Vec3.Dot (sphere.x, plane.normal) - plane.d;
			double d = Math.Abs (dist) - sphere.r;

			if (d < 0) {
				var p = sphere.x - (Vec3.Dot (plane.normal, sphere.x) + plane.d) * plane.normal;
				intersections.Add (new Intersection {
					entity = plane.self,
					distance = -d,
					normal = plane.normal,
					point = p
				});
			}

			return intersections;
		}

		public static List<Intersection> Collides (Sphere a, Sphere b)
		{
			var intersections = new List<Intersection> ();

			double sqdist = (a.x - b.x).SqLength;

			if (sqdist < Math.Pow (a.r + b.r, 2)) {
				intersections.Add (new Intersection {
					entity = b,
					distance = Math.Abs (Math.Sqrt (sqdist) - a.r - b.r),
					normal = (a.x - b.x).UnitVector,
					point = a.x - (a.x * a.r - b.x * b.r) / (a.r + b.r)
				});
			}

			return intersections;
		}
	}

	public class PlaneCollider : Collider
	{
		public Entity self;
		public Vec3 normal;
		public double d;
		public const double thickness = 0.3;

		public PlaneCollider (Entity self, Vec3 normal, double d)
		{
			this.self = self;
			this.normal = normal;
			this.d = d;
		}

		public override List<Intersection> Collides (IEnumerable<Particle> ps)
		{
			return Collides (ps, this);
		}

		public override List<Intersection> Collides (Sphere sphere)
		{
			return Collides (sphere, this);
		}
	}

	public class AABB : Collider
	{
		public Vec3 min;
		public Vec3 max;

		public AABB (Vec3 min, Vec3 max)
		{
			this.min = min;
			this.max = max;
		}

		public override List<Intersection> Collides (IEnumerable<Particle> ps)
		{
			return Collides (ps, this);
		}

		public override List<Intersection> Collides (Sphere b)
		{
			return CollisionNotImplemented;
		}
	}

	public class SphereCollider : Collider
	{
		public Sphere self;

		public SphereCollider (Sphere sphere)
		{
			this.self = sphere;
		}

		public override List<Intersection> Collides (IEnumerable<Particle> ps)
		{
			return CollisionNotImplemented;
		}

		public override List<Intersection> Collides (Sphere other)
		{
			return Collides (self, other);
		}
	}

}
