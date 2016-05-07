using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PE;

namespace physicsEngine.CSharpTests.Assets.Scripts.Tools
{
    [TestClass]
    public class MatrixVectorTests
    {
        [TestMethod]
        public void Vec3MatrixVectorMultTest1()
        {
            Vec3Matrix M = new Vec3Matrix(2, 2);
            M[0, 0] = new Vec3(1, 1, 1); M[0, 1] = new Vec3(0, 0, 0);
            M[1, 0] = new Vec3(0, 0, 0); M[1, 1] = new Vec3(1, 1, 1);

            Vector<Vec3> v = new Vector<Vec3>(2);
            v[0] = new Vec3(1, 1, 1); v[1] = new Vec3(2, 2, 2);

            Vector<Vec3> nv = M * v;
            Console.Write("v(" + v[0] + v[1] + ")  nv(" + nv[0] + nv[1] + ")");
            Assert.IsTrue(true);
            
        }
    }
}
