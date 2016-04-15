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
            Point p0 = new Point(new PE.Vec3(-2, 0, 0));
            Plane pl0 = new Plane(new PE.Vec3(0,0,1), new PE.Vec3(0, 0, 0));
            
            Assert.IsTrue(Collider.Collides(p0, pl0), (PE.Vec3.Dot(pl0.normal, pl0.position-p0.position).ToString()));

            
        }

        [TestMethod()]
        public void Point_PlaneTest1()
        {
            Point p1 = new Point(new PE.Vec3(1, 0, 2));
            Plane pl1 = new Plane(new PE.Vec3(0, 0, 1), new PE.Vec3(0, 0, 1));

            Assert.IsFalse(Collider.Collides(p1, pl1), (PE.Vec3.Dot(pl1.normal, pl1.position - p1.position).ToString()));
        }
    }
}