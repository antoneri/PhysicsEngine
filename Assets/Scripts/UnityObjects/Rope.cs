using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PE
{
	public class Rope : MonoBehaviour
	{
		public int numParticles = 20;

		public double massOfLastParticle = 1;
		public double springK = 1000;

		ParticleSystem particles = new ParticleSystem ();

		private Vector3[] lineVertices;
		private LineRenderer lineRenderer;

		// Use this for initialization
		void Start ()
		{
			lineVertices = new Vector3[numParticles];
			lineRenderer = GetComponent<UnityEngine.LineRenderer> ();
			lineRenderer.SetVertexCount (numParticles);
			lineRenderer.SetWidth (0.1f, 0.1f);

			var startPos = new Vec3 (-3, 15, 0);
			var dx = new Vec3 (-0.25, -0.25, 0);
			var mass = 1.0;

			for (int i = 0; i < numParticles; i++) {
				particles.Add (new Particle (startPos + i * dx, mass));
			}

			particles [numParticles - 1].Mass = massOfLastParticle;

			List<Constraint> constraints = new List<Constraint> ();
			constraints.Add (new PositionConstraint (0, startPos, springK));

			for (int i = 0; i < numParticles - 1; i++) {
				constraints.Add (new DistanceConstraint (i, i + 1, dx.Length, springK));
			}

			particles.constraints = constraints;

			Engine.instance.Rope = particles;
		}

		// Update is called once per frame
		void Update ()
		{
			particles [numParticles - 1].Mass = massOfLastParticle;

			foreach (var constraint in particles.constraints) {
				constraint.k = springK;
			}

			for (int i = 0; i < numParticles; i++) {
				lineVertices [i] = particles [i].x;
			}

			lineRenderer.SetPositions (lineVertices);
		}
	}

}

