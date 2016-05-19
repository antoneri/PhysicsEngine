using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using PE;

public struct Intersection
{
	public PE.Particle particle;
	public RigidBody body;
	public double distance;
	public Vec3 normal;
	public Vec3 point;

	public override string ToString ()
	{
		return distance + "\n" + normal.ToString () + "\n" + point.ToString ();
	}
}

public abstract class Collider
{
	public readonly List<Intersection> CollisionNotImplemented = new List<Intersection> ();

	public abstract List<Intersection> Collides (IEnumerable<PE.Particle> ps);

	public abstract List<Intersection> Collides (RigidBody b);

	public static List<Intersection> Collides (IEnumerable<PE.Particle> ps, Plane pl)
	{
		List<Intersection> intersections = new List<Intersection> ();
		foreach (var p in ps) {
			double d = Math.Abs (Vec3.Dot (p.x, pl.normal) - pl.d);
			if (d < Plane.thickness) {
				Intersection data = new Intersection {
					particle = p,
					distance = Plane.thickness - d,
					normal = pl.normal,
					point = (p.x - (Vec3.Dot (pl.normal, p.x) + pl.d) * pl.normal) + Plane.thickness * pl.normal
				};
				intersections.Add (data);
			}
		}
		return intersections;
	}

	public static List<Intersection> Collides (IEnumerable<PE.Particle> ps, AABB b)
	{
		List<Intersection> intersections = new List<Intersection> ();
		foreach (var p in ps) {
			if (p.x.x < b.max.x && p.x.x > b.min.x &&
			    p.x.y < b.max.y && p.x.y > b.min.y &&
			    p.x.z < b.max.z && p.x.z > b.min.z) {

				Vec3 n = PointNormalAABB (p.x, b);
				Vec3 AABBPoint = ClosestPointAABB (p.x, b, n);
				Intersection data = new Intersection {
					particle = p,
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

	public static List<Intersection> Collides (RigidBody body, Plane plane)
	{
		var intersections = new List<Intersection> ();

		double dist = Vec3.Dot (body.x, plane.normal) - plane.d;
		double d = Math.Abs (dist) - body.r;

		if (d < 0) {
			intersections.Add (new Intersection {
				distance = - d,
				normal = plane.normal,
				point = body.x - (dist - body.r) * plane.normal
			});
		}

		return intersections;
	}

	public static List<Intersection> Collides (RigidBody a, RigidBody b)
	{
		var intersections = new List<Intersection> ();

		double sqdist = (a.x - b.x).SqLength;

		if (sqdist < Math.Pow (a.r + b.r, 2)) {
			intersections.Add (new Intersection {
				body = b,
				distance = Math.Sqrt(sqdist) - a.r - b.r,
				normal = (a.x - b.x).UnitVector,
				point = (a.x * a.r - b.x * b.r)/(a.r + b.r)
			});
		}

		return intersections;
	}
}

public class Plane : Collider
{
	public Vec3 normal;
	public double d;

	public const double thickness = 0.3;

	public Plane (Vec3 normal, double d)
	{
		this.normal = normal;
		this.d = d;
	}

	public override List<Intersection> Collides (IEnumerable<PE.Particle> ps)
	{
		return Collides (ps, this);
	}

	public override List<Intersection> Collides (RigidBody b)
	{
		return Collides (b, this);
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

	public override List<Intersection> Collides (IEnumerable<PE.Particle> ps)
	{
		return Collides (ps, this);
	}

	public override List<Intersection> Collides (RigidBody b)
	{
		return CollisionNotImplemented;
	}
}
