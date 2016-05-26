using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PE;
namespace Tests
{
    [TestClass()]
    public class ColliderTests
    {
        [TestMethod()]
        public void Point_PlaneTest()
        {
            PE.ParticleSystem ps = new PE.ParticleSystem();
            ps.Add(new Particle(new PE.Vec3(-2, 0, 0), new PE.Vec3(0, 0, 0), 0.0, 0, 0));
            PlaneCollider pl0 = new PlaneCollider(new Entity(), new PE.Vec3(0,0,1), 0.0);
            
            Assert.IsTrue(Collider.CheckIntersection(ps, pl0).Count != 0);

            
        }

        [TestMethod()]
        public void Point_PlaneTest1()
        {
            PE.ParticleSystem ps = new PE.ParticleSystem();
            ps.Add(new Particle(new PE.Vec3(1, 0, 2), new PE.Vec3(0, 0, 0), 0.0, 0, 0));
            PlaneCollider pl1 = new PlaneCollider(new Entity(), new PE.Vec3(0, 0, 1), 1.0);

            Assert.IsFalse(Collider.CheckIntersection(ps, pl1).Count != 0);
        }

        [TestMethod()]
        public void Point_AABBTest()
        {
            PE.ParticleSystem ps = new PE.ParticleSystem();
            ps.Add(new Particle(new PE.Vec3(0, 0.5, 0), new PE.Vec3(0, 0, 0), 0.0, 0, 0));
            AABB aabb = new AABB(new Vec3(-1.0, -1.0, -1.0), new Vec3(1.0, 1.0, 1.0));

            Assert.IsTrue(Collider.CheckIntersection(ps, aabb).Count != 0);
        }

        [TestMethod()]
        public void Sphere_SphereTest()
        {
            Sphere s1 = new Sphere(new Vec3(0), 1, 1);
            Sphere s2 = new Sphere(new Vec3(1.99, 0, 0), 1, 1);
            PE.SphereCollider sc1 = new PE.SphereCollider(s1);
            PE.SphereCollider sc2 = new PE.SphereCollider(s2);

            List<Intersection> intersections = sc1.CheckIntersection(s2);

            Intersection intersection = intersections[0];

            Vec3 normal = intersection.normal;
            Vec3 point = intersection.point;
            double penetration = intersection.distance;

            Console.WriteLine("Normal: " + normal);
            Console.WriteLine("Point: " + point);
            Console.WriteLine("Penetration: " + penetration);
            Assert.IsTrue(normal.Equals(new Vec3(-1, 0, 0)) &&
                          point.Equals(new Vec3(0.995, 0, 0)) &&
                          penetration < 0.012 && penetration > 0.008);
                
        }

        [TestMethod()]
        public void Sphere_PlaneTest()
        {
            Sphere s = new Sphere(new Vec3(0, 1, 0), 1.5, 1);
            PlaneCollider p = new PlaneCollider(new Entity(), new Vec3(0, 1, 0), 0);
            PE.SphereCollider sc = new PE.SphereCollider(s);

            List<Intersection> intersections = p.CheckIntersection(s);

            Intersection intersection = intersections[0];

            Vec3 normal = intersection.normal;
            Vec3 point = intersection.point;
            double penetration = intersection.distance;

            Console.WriteLine("Normal: " + normal);
            Console.WriteLine("Point: " + point);
            Console.WriteLine("Penetration: " + penetration);
            Assert.IsTrue(normal.Equals(new Vec3(0, 1, 0)) &&
                          point.Equals(new Vec3(0, 0, 0)) &&
                          penetration == 0.5);

        }
    }
}