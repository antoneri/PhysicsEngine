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
	private float startSize = 0.1f;

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

			for (var i = 0; i < particlesToEmit; i++) {
				// Add a random perturbation to initial position and velocity
				var r = random.NextDouble () * (2 - 1) + 1;
				var initialVelocity = (PE.Vec3)transform.forward * r;

				// Uniform distribution on a circle http://stackoverflow.com/a/5838991
				var radius = 0.4;
				var random1 = random.NextDouble ();
				var random2 = random.NextDouble ();
				var rx = random2 * radius * Math.Cos (PI2 * random1 / random2);
				var ry = random2 * radius * Math.Sin (PI2 * random1 / random2); 
				var initialPosition = (PE.Vec3)transform.position + ry * (PE.Vec3)transform.up + rx * (PE.Vec3)transform.right;

				ps.AddParticle (initialPosition, initialVelocity);
			}

			lastEmitted = t;
		}

		UpdateUnityParticleSystem ();
	}

	private void UpdateUnityParticleSystem ()
	{
		ParticleSystem.Particle[] unityParticles = new ParticleSystem.Particle[ps.Count];

		var startColor = Color.black;
		var endColor = Color.black;
		endColor.a = 0;

		for (int i = 0; i < ps.Count; i++) {
			unityParticles [i] = new ParticleSystem.Particle () {
				position = (Vector3)ps [i].x - transform.position,
				startColor = Color.Lerp (startColor, endColor, (float)ps [i].age),
				startSize = startSize,
			};
		}

		nrOfParticles = ps.Count; // For debugging

		GetComponent<ParticleSystem> ().SetParticles (unityParticles, unityParticles.Length);
	}
}
