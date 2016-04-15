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
        public void CollidesTest()
        {
            Point p = new Point(new PE.Vec3(1, 0, 0));
            Plane pl = new Plane(new PE.Vec3(1,0,0), 1.0);
            
            Console.Write(Vec3.Dot(pl.normal, p.position) + pl.D);
            Assert.IsTrue(Collider.Collides(p, pl), (Vec3.Dot(pl.normal, p.position) + pl.D).ToString());
        }
    }
}