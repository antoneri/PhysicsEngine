using UnityEngine;
using System.Collections;

namespace PE
{
    public class Solver
    {

        public static Vector<Vec3> GaussSeidel(Matrix<Vec3> S, Vector<Vec3> B, uint iterations)
        {
            Vector<Vec3> lambda = new Vector<Vec3>(B.Size);
            for (int i = 0; i < iterations; i++)
            {
                GaussSeidelIterate(S, lambda, B);
            }
            return lambda;
        }

        private static void GaussSeidelIterate(Matrix<Vec3> S, Vector<Vec3> lambda, Vector<Vec3> B)
        {
            for (int i = 0; i < lambda.Length; i++)
            {

                Vec3 sum = new Vec3();
                for (int j = 0; j < i; j++)
                {
                    sum += S[i, j] * lambda[j];
                }

                for (int j = i+1; j < lambda.Length; j++)
                {
                    sum += S[i,j] * lambda[j];
                }

                lambda[i] = (1.0 / S[i, i]) * (B[i] - sum); 
            }
        }
    }
}
