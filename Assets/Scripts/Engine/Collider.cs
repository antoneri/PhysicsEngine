using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using PE;

public struct IntersectData {
    public PE.Particle particle;
    public Vec3 distance;
    public Vec3 normal;
    public Vec3 point;
}

public abstract class Collider
{
    public abstract List<IntersectData> Collides(PE.ParticleSystem ps);
    public abstract IntersectData Collides(Plane p);

    public static List<IntersectData> Collides(PE.ParticleSystem ps, Plane pl)
    {
        List<IntersectData> intersections = new List<IntersectData>();
        foreach (var p in ps)
        {
            if (Math.Abs(Vec3.Dot(pl.normal, pl.position - p.x)) < Plane.thickness) {
                IntersectData data = new IntersectData
                {
                    particle = p,
                    distance = pl.position - p.x,
                    normal = pl.normal,
                    point = pl.position + 0.2 * pl.normal
                };
                intersections.Add(data);
            }
        }
        return intersections;
    }

}

public class Plane : Collider
{

    public Vec3 normal;
    public Vec3 position;

    public const double thickness = 0.3;

    public Plane(Vec3 normal, Vec3 position)
    {
        this.normal = normal;
        this.position = position;
    }

    public override List<IntersectData> Collides(PE.ParticleSystem ps)
    {
        return Collides(ps, this);
    }

    public override IntersectData Collides(Plane p)
    {
        throw new NotImplementedException();
    }

}
