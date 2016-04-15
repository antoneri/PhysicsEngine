using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Linq;

public class Emitter : MonoBehaviour
{
	public float rate = 10;

	private PE.ParticleSystem ps = new PE.ParticleSystem ();

	private float lastEmitted = 0;
	private int nrOfParticles = 0;

	private System.Random random = new System.Random ();
	private double PI2 = Math.PI / 2;

	// Use this for initialization
	void Start ()
	{
		PE.Engine.instance.AddParticleSystem (ps);
	}
	
	// Update is called once per frame
	void Update ()
	{
		var t = Time.fixedTime;
		var elapsed = t - lastEmitted;

		if (elapsed >= 1 / rate) {
			var particlesToEmit = (int)Math.Ceiling (elapsed / (1 / rate));

			foreach (var _ in Enumerable.Range(0, particlesToEmit)) {
				// Add a random perturbation to initial position and velocity
				var r = random.NextDouble () * (2 - 1) + 1;
				var initialVelocity = (PE.Vec3)transform.forward * r;

				// Uniform distribution on a circle http://stackoverflow.com/a/5838991
				var radius = 1;
				var random1 = random.NextDouble ();
				var random2 = random.NextDouble ();
				var rx = random2 * radius * Math.Cos (PI2 * random1 / random2);
				var ry = random2 * radius * Math.Sin (PI2 * random1 / random2); 

				var initialPosition = (PE.Vec3)transform.position + ry * (PE.Vec3)transform.up + rx * (PE.Vec3)transform.right;

				// Add particle to particleSystem
				ps.AddParticle (initialPosition, initialVelocity);
			}

			lastEmitted = t;
		}

		UpdateUnityParticleSystem ();
	}

	private void UpdateUnityParticleSystem ()
	{
		ParticleSystem.Particle[] unityParticles = new ParticleSystem.Particle[ps.ParticleCount];

		for (int i = 0; i < ps.ParticleCount; i++) {
			PE.Particle p = ps.particles [i];

			unityParticles [i] = new ParticleSystem.Particle () {
				position = (Vector3)p.x - transform.position,
				startColor = new Color (1, 0f, 0f),
				startSize = 0.1f,
			};
		}

		nrOfParticles = ps.ParticleCount; // For debugging

		GetComponent<ParticleSystem> ().SetParticles (unityParticles, unityParticles.Length);

	}
}
