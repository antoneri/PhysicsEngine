using UnityEngine;
using System.Collections;
using System;

using PE;

public abstract class Collider
{

    public static bool Collides(Point po, Plane pl)
    {
        return Math.Abs(Vec3.Dot(pl.normal, pl.position - po.position)) < 0.01;
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
    public Vec3 position;

    public Plane(Vec3 normal, Vec3 position)
    {
        this.normal = normal;
        this.position = position;
    }

}
