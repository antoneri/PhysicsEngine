using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using PE;

public struct IntersectData {
    public PE.Particle particle;
    public double distance;
    public Vec3 normal;
    public Vec3 point;
}

public abstract class Collider
{
    public abstract List<IntersectData> Collides(PE.ParticleSystem ps);

    public static List<IntersectData> Collides(PE.ParticleSystem ps, Plane pl)
    {
        List<IntersectData> intersections = new List<IntersectData>();
        foreach (var p in ps)
        {
            double d = Vec3.Dot(p.x, pl.normal) - pl.d;
            if (Math.Abs(d) < Plane.thickness) {
                IntersectData data = new IntersectData
                {
                    particle = p,
                    distance = d,
                    normal = pl.normal,
                    point = (p.x - d * pl.normal) + Plane.thickness * pl.normal
                };
                intersections.Add(data);
            }
        }
        return intersections;
    }

    public static List<IntersectData> Collides(PE.ParticleSystem ps, AABB b)
    {
        List<IntersectData> intersections = new List<IntersectData>();
        foreach (var p in ps)
        {
            if (p.x.x < b.max.x && p.x.x > b.min.x &&
                p.x.y < b.max.y && p.x.y > b.min.y &&
                p.x.z < b.max.z && p.x.z > b.min.z) {

                Vec3 AABBPoint = ClosestPointAABB(p.x, b);
                IntersectData data = new IntersectData
                {
                    particle = p,
                    distance = SqDistPointAABB(p.x, b),
                    normal = PointNormalAABB(AABBPoint, b),
                    point = AABBPoint
                };
                intersections.Add(data);
            }
        }
        return intersections;
    }









    /* AABB intersect data */

    public static Vec3 PointNormalAABB(Vec3 p, AABB b)
    {
        Vec3 normal = new Vec3();
        double minDistance = double.MaxValue;

        if (Math.Abs(b.min.x - p.x) < minDistance) normal.set(-1, 0, 0);
        if (Math.Abs(b.max.x - p.x) < minDistance) normal.set(1, 0, 0);
        if (Math.Abs(b.min.y - p.y) < minDistance) normal.set(0, -1, 0);
        if (Math.Abs(b.max.y - p.y) < minDistance) normal.set(0, 1, 0);
        if (Math.Abs(b.min.z - p.z) < minDistance) normal.set(0, 0, -1);
        if (Math.Abs(b.max.z - p.z) < minDistance) normal.set(0, 0, 1);

        return normal;
    }

    public static Vec3 ClosestPointAABB(Vec3 p, AABB b)
    {
        Vec3 q = new Vec3();
        for (int i = 0; i < 3; i++)
        {
            double v = p[i];
            v = Math.Max(v, b.min[i]);
            v = Math.Min(v, b.max[i]);
            q[i] = v;
        }
        return q;
    }

    public static double SqDistPointAABB(Vec3 p, AABB b)
    {
        double sqDist = 0.0f;
        for (int i = 0; i < 3; i++)
        {
            // For each axis count any excess distance outside box extents
            double v = p[i];
            if (v < b.min[i]) sqDist += (b.min[i] - v) * (b.min[i] - v);
            if (v > b.max[i]) sqDist += (v - b.max[i]) * (v - b.max[i]);
        }
        return sqDist;
    }

}

public class Plane : Collider
{

    public Vec3 normal;
    public double d;

    public const double thickness = 0.3;

    public Plane(Vec3 normal, double d)
    {
        this.normal = normal;
        this.d = d;
    }

    public override List<IntersectData> Collides(PE.ParticleSystem ps)
    {
        return Collides(ps, this);
    }

}

public class AABB : Collider
{
    public Vec3 min;
    public Vec3 max;

    public AABB(Vec3 min, Vec3 max)
    {
        this.min = min;
        this.max = max;
    }

    public override List<IntersectData> Collides(PE.ParticleSystem ps)
    {
        return Collides(ps, this);
    }
}
