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

    public static List<IntersectData> Collides(PE.ParticleSystem ps, AABB box)
    {
        List<IntersectData> intersections = new List<IntersectData>();
        foreach (var p in ps)
        {
            
        }
        return intersections;
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
    

    public override List<IntersectData> Collides(PE.ParticleSystem ps)
    {
        return Collides(ps, this);
    }
}
