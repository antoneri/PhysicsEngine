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
            Matrix<Vec3> M = new Matrix<Vec3>(2, 2);
            M[0, 0] = new Vec3(1, 1, 1); M[0, 1] = new Vec3(0, 0, 0);
            M[1, 0] = new Vec3(0, 0, 0); M[1, 1] = new Vec3(1, 1, 1);

            Vector<Vec3> v = new Vector<Vec3>(2);
            v[0] = new Vec3(1, 1, 1); v[1] = new Vec3(2, 2, 2);

            Vector<Vec3> nv = M * v;
            Console.Write("v(" + v[0] + v[1] + ")  nv(" + nv[0] + nv[1] + ")");
            Assert.AreEqual(nv, nv);
        }

        [TestMethod]
        public void Vec3MatrixMatrixMultTest1()
        {
            Matrix<Vec3> M1 = new Matrix<Vec3>(2, 2);
            M1[0, 0] = new Vec3(1, 1, 1); M1[0, 1] = new Vec3(0, 0, 0);
            M1[1, 0] = new Vec3(0, 0, 0); M1[1, 1] = new Vec3(1, 1, 1);

            Matrix<Vec3> M2 = new Matrix<Vec3>(2, 2);
            M2[0, 0] = new Vec3(1, 2, 3); M2[0, 1] = new Vec3(3, 2, 1);
            M2[1, 0] = new Vec3(1, 2, 3); M2[1, 1] = new Vec3(3, 2, 1);

            Matrix<Vec3> M3 = M1 * M2;

            Assert.AreEqual(M2, M3);

        }
    }
}
