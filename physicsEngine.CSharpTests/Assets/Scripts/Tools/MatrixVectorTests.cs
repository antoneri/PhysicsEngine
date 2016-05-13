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

            Vec3Vector v = new Vec3Vector(2);
            v[0] = new Vec3(1, 1, 1); v[1] = new Vec3(2, 2, 2);

            Vec3Vector nv = M * v;
            Console.Write("v(" + v[0] + v[1] + ")  nv(" + nv[0] + nv[1] + ")");
            Assert.AreEqual(v, nv);
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

        [TestMethod]
        public void DoubleVec3VectorMultTest1()
        {
            double k = 2.0;

            Vec3Vector v = new Vec3Vector(2);
            v[0] = new Vec3(1, 1, 1); v[1] = new Vec3(2, 2, 2);

            Vec3Vector correct = new Vec3Vector(2);
            correct[0] = new Vec3(2, 2, 2); correct[1] = new Vec3(4, 4, 4);

            Vec3Vector nv = k * v;
            Console.Write("correct(" + correct[0] + correct[1] + ")  nv(" + nv[0] + nv[1] + ")");
            Assert.AreEqual(correct, nv);

        }

        [TestMethod]
        public void Vec3MatrixTransposeTest1()
        {
            Matrix<Vec3> M = new Matrix<Vec3>(2, 2);
            M[0, 0] = new Vec3(1, 1, 1); M[0, 1] = new Vec3(1, 2, 1);
            M[1, 0] = new Vec3(2, 1, 2); M[1, 1] = new Vec3(1, 1, 1);

            Matrix<Vec3> C = new Matrix<Vec3>(2, 2);
            C[0, 0] = new Vec3(1, 1, 1); C[0, 1] = new Vec3(2, 1, 2);
            C[1, 0] = new Vec3(1, 2, 1); C[1, 1] = new Vec3(1, 1, 1);

            Matrix<Vec3> T = M.Transpose;
            Console.Write("C(" + C + ")  T(" + T + ")");
            Assert.AreEqual(C, T);
        }

        [TestMethod]
        public void Vec3VectorNorm1()
        {
            Vec3Vector v = new Vec3Vector(2);
            v[0] = new Vec3(1, 1, 1); v[1] = new Vec3(2, 2, 2);

            double correct = Math.Sqrt(15);

            double norm = v.Norm;
            Console.Write("Correct: " + correct + " norm: " + v.Norm);

            Assert.IsTrue(correct - v.Norm < 0.00001);

        }

        [TestMethod]
        public void Vec3VectorCopy()
        {
            Vec3Vector v = new Vec3Vector(2);
            v[0] = new Vec3(1, 1, 1); v[1] = new Vec3(2, 2, 2);

            Vec3Vector correct = new Vec3Vector(2);
            correct[0] = new Vec3(1, 1, 1); correct[1] = new Vec3(2, 2, 2);

            Vec3Vector v2 = new Vec3Vector(v);

            v[0] = new Vec3(0, 0, 0);
            v[1] = new Vec3(5, 5, 5);

            Console.Write("Correct: " + correct + " vcopy: " + v2);

            Assert.AreEqual(correct, v2);

        }
    }
}
