using UnityEngine;
using System.Collections;
using System;

using PE;

public abstract class Collider
{

    public static bool Collides(Point po, Plane pl)
    {
        return (Vec3.Dot(pl.normal, po.position) + pl.D) < 1;
    }

}

public class Point : Collider
{

    public Vec3 position;

    public Point(Vec3 position)
    {
        this.position = position;
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

}
