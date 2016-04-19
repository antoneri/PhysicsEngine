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
            ps.Add(new Particle(new PE.Vec3(-2, 0, 0), new PE.Vec3(0, 0, 0), 0.0));
            Plane pl0 = new Plane(new PE.Vec3(0,0,1), 0.0);
            
            Assert.IsTrue(Collider.Collides(ps, pl0).Count != 0);

            
        }

        [TestMethod()]
        public void Point_PlaneTest1()
        {
            PE.ParticleSystem ps = new PE.ParticleSystem();
            ps.Add(new Particle(new PE.Vec3(1, 0, 2), new PE.Vec3(0, 0, 0), 0.0));
            Plane pl1 = new Plane(new PE.Vec3(0, 0, 1), 1.0);

            Assert.IsFalse(Collider.Collides(ps, pl1).Count != 0);
        }

        [TestMethod()]
        public void Point_AABBTest()
        {
            PE.ParticleSystem ps = new PE.ParticleSystem();
            ps.Add(new Particle(new PE.Vec3(0, 0.5, 0), new PE.Vec3(0, 0, 0), 0.0));
            AABB aabb = new AABB(new Vec3(-1.0, -1.0, -1.0), new Vec3(1.0, 1.0, 1.0));

            Assert.IsTrue(Collider.Collides(ps, aabb).Count != 0);
        }
    }
}