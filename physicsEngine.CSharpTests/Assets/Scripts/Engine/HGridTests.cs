using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace physicsEngine.CSharpTests.Assets.Scripts.Engine
{
    [TestClass]
    public class HGridTests
    {
        [TestMethod]
        public void ShouldCollide()
        {

            PE.Sphere rb1 = new PE.Sphere(new PE.Vec3(0), 0.5, 1);
            PE.Sphere rb2 = new PE.Sphere(new PE.Vec3(0), 0.5, 1);

            PE.HGrid hgrid = new PE.HGrid();
            PE.HGridObject obj1 = new PE.HGridObject(rb1, rb1.x, (float)rb1.r);
            PE.HGridObject obj2 = new PE.HGridObject(rb2, rb2.x, (float)rb2.r);

            hgrid.AddObject(obj1);
            hgrid.AddObject(obj2);

            List<PE.Sphere> collisions = hgrid.CheckObjAgainstGrid(obj1);
            Console.Write(collisions.Count);
            Assert.IsTrue(collisions.Count == 1);
        }

        [TestMethod]
        public void ShouldCollideDifferentSize()
        {

            PE.Sphere rb1 = new PE.Sphere(new PE.Vec3(0), 0.5, 1);
            PE.Sphere rb2 = new PE.Sphere(new PE.Vec3(2,0,0), 4, 1);

            PE.HGrid hgrid = new PE.HGrid();
            PE.HGridObject obj1 = new PE.HGridObject(rb1, rb1.x, (float)rb1.r);
            PE.HGridObject obj2 = new PE.HGridObject(rb2, rb2.x, (float)rb2.r);

            hgrid.AddObject(obj1);
            hgrid.AddObject(obj2);

            List<PE.Sphere> collisions = hgrid.CheckObjAgainstGrid(obj1);
            Console.Write(collisions.Count);
            Assert.IsTrue(collisions.Count == 1);
        }

        [TestMethod]
        public void ShouldNotCollide()
        {

            PE.Sphere rb1 = new PE.Sphere(new PE.Vec3(10,0,0), 0.5, 1);
            PE.Sphere rb2 = new PE.Sphere(new PE.Vec3(0), 0.5, 1);

            PE.HGrid hgrid = new PE.HGrid();
            PE.HGridObject obj1 = new PE.HGridObject(rb1, rb1.x, (float)rb1.r);
            PE.HGridObject obj2 = new PE.HGridObject(rb2, rb2.x, (float)rb2.r);

            hgrid.AddObject(obj1);
            hgrid.AddObject(obj2);

            List<PE.Sphere> collisions = hgrid.CheckObjAgainstGrid(obj1);
            Console.Write(collisions.Count);
            Assert.IsTrue(collisions.Count == 0);
        }
    }
}
