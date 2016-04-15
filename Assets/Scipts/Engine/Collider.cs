using UnityEngine;
using System.Collections;
using System;

using PE;

public abstract class Collider
{

    public abstract bool Collides(Collider x);

    protected abstract bool Collides(Point p);
    protected abstract bool Collides(Plane p);

    protected bool Collides(Point po, Plane pl)
    {
        return (Vec3.Dot(pl.normal, po.position) + pl.D < 0.01);
    }

}

public class Point : Collider
{

    public Vec3 position;

    public Point(Vec3 position)
    {
        this.position = position;
    }

    public override bool Collides(Collider x)
    {
        return x.Collides(this);
    }

    protected override bool Collides(Plane p)
    {
        return Collides(this, p);
    }

    protected override bool Collides(Point p)
    {
        return false;
    }
}

public class Plane : Collider
{

    public Vec3 normal;
    public double D;

    public Plane(Vec3 normal, double D)
    {
        this.normal = normal;
        this.D = D;
    }

    public override bool Collides(Collider x)
    {
        return x.Collides(this);
    }

    protected override bool Collides(Plane p)
    {
        return false;
    }

    protected override bool Collides(Point p)
    {
        return Collides(p, this);
    }
}
