using UnityEngine;
using System.Collections;
using System;

using PE;

public abstract class Collider
{
    public abstract bool Collides(Point p);
    public abstract bool Collides(Plane p);

    public static bool Collides(Point po, Plane pl)
    {
        return Math.Abs(Vec3.Dot(pl.normal, pl.position - po.position)) < 0.2;
    }

    public static bool Collides(PE.Particle pa, Plane pl)
    {
        return Math.Abs(Vec3.Dot(pl.normal, pl.position - pa.x)) < 0.01;
    }

}

public class Point : Collider
{

    public Vec3 position;

    public Point(Vec3 position)
    {
        this.position = position;
    }

    public override bool Collides(Plane p)
    {
        return Collider.Collides(this, p);
    }

    public override bool Collides(Point p)
    {
        throw new NotImplementedException();
    }
}

public class Plane : Collider
{

    public Vec3 normal;
    public Vec3 position;

    public Plane(Vec3 normal, Vec3 position)
    {
        this.normal = normal;
        this.position = position;
    }

    public override bool Collides(Plane p)
    {
        throw new NotImplementedException();
    }

    public override bool Collides(Point p)
    {
        return Collider.Collides(p, this);
    }
}
