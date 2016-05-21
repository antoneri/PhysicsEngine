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
        public void ShouldMultipleObjectsCollide()
        {

            PE.Sphere rb1 = new PE.Sphere(new PE.Vec3(1), 0.5, 1);
            PE.Sphere rb2 = new PE.Sphere(new PE.Vec3(2), 0.5, 1);
            PE.Sphere rb3 = new PE.Sphere(new PE.Vec3(3, 0, 0), 0.5, 1);
            PE.Sphere rb4 = new PE.Sphere(new PE.Vec3(0), 4, 1);

            PE.HGrid hgrid = new PE.HGrid();
            PE.HGridObject obj1 = new PE.HGridObject(rb1, rb1.x, (float)rb1.r);
            PE.HGridObject obj2 = new PE.HGridObject(rb2, rb2.x, (float)rb2.r);
            PE.HGridObject obj3 = new PE.HGridObject(rb3, rb3.x, (float)rb3.r);
            PE.HGridObject obj4 = new PE.HGridObject(rb4, rb4.x, (float)rb4.r);

            hgrid.AddObject(obj1);
            hgrid.AddObject(obj2);
            hgrid.AddObject(obj3);
            hgrid.AddObject(obj4);

            List<PE.Sphere> collisions = hgrid.CheckObjAgainstGrid(obj4);
            Console.Write(collisions.Count);
            Assert.IsTrue(collisions.Count == 3);
        }

        [TestMethod]
        public void ShouldCollideDifferentSize()
        {

            PE.Sphere rb1 = new PE.Sphere(new PE.Vec3(0), 0.5, 1);
            PE.Sphere rb2 = new PE.Sphere(new PE.Vec3(2,1,0), 4, 1);

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

        [TestMethod]
        public void ShouldNotCollideDifferentSize()
        {

            PE.Sphere rb1 = new PE.Sphere(new PE.Vec3(0), 0.5, 1);
            PE.Sphere rb2 = new PE.Sphere(new PE.Vec3(5, 0, 0), 4, 1);

            PE.HGrid hgrid = new PE.HGrid();
            PE.HGridObject obj1 = new PE.HGridObject(rb1, rb1.x, (float)rb1.r);
            PE.HGridObject obj2 = new PE.HGridObject(rb2, rb2.x, (float)rb2.r);

            hgrid.AddObject(obj1);
            hgrid.AddObject(obj2);

            List<PE.Sphere> collisions = hgrid.CheckObjAgainstGrid(obj1);
            Console.Write(collisions.Count);
            Assert.IsTrue(collisions.Count == 0);
        }

        [TestMethod]
        public void RemoveObject()
        {

            PE.Sphere rb1 = new PE.Sphere(new PE.Vec3(0), 0.5, 1);

            PE.HGrid hgrid = new PE.HGrid();
            PE.HGridObject obj1 = new PE.HGridObject(rb1, rb1.x, (float)rb1.r);

            hgrid.AddObject(obj1);

            hgrid.RemoveObject(obj1);
            bool empty = hgrid.isEmpty();
            Assert.IsTrue(empty);
        }

        [TestMethod]
        public void ShouldNotBeEmpty()
        {

            PE.Sphere rb1 = new PE.Sphere(new PE.Vec3(0), 0.5, 1);
            PE.Sphere rb2 = new PE.Sphere(new PE.Vec3(0), 0.5, 1);

            PE.HGrid hgrid = new PE.HGrid();
            PE.HGridObject obj1 = new PE.HGridObject(rb1, rb1.x, (float)rb1.r);
            PE.HGridObject obj2 = new PE.HGridObject(rb2, rb2.x, (float)rb2.r);

            hgrid.AddObject(obj1);
            hgrid.AddObject(obj2);

            hgrid.RemoveObject(obj1);
            bool empty = hgrid.isEmpty();
            Assert.IsTrue(!empty);
        }



    }
}
