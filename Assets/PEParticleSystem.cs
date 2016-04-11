using UnityEngine;
using System.Collections;

namespace PE
{
	public class ParticleSystem : MonoBehaviour
	{
		public int N = 100;
		Particle[] particles;
		public double[] x_init = { 0, 1, 0 };
		public double[] v_init = { 10, 10, 0 };

		// Use this for initialization
		void Start ()
		{
			particles = new Particle[N];

			for (int i = 0; i < N; i++) {
				particles [i].x = x_init;
				particles [i].v = v_init;
				particles [i].f = new double[3] { 0, 0, 0 };
				particles [i].m = 1e-4;
			}
		}

		// Update is called once per frame
		void Update ()
		{

		}
	}
}

