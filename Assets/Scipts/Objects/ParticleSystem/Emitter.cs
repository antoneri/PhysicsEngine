using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

public class Emitter : MonoBehaviour
{
	public float rate = 1.0f;

	private PE.ParticleSystem ps = new PE.ParticleSystem ();

	private float lastEmitted = 0;
	private int nrOfParticles = 0;
	private System.Random random = new System.Random ();


	// Use this for initialization
	void Start ()
	{
		PE.Engine.instance.AddParticleSystem (ps);
	}
	
	// Update is called once per frame
	void Update ()
	{
		float t = Time.fixedTime;
        
		if (t - lastEmitted >= 1 / rate) {
			// Add particle to particleSystem

			// Do magic
			var forward = (PE.Vec3)transform.forward;
			var r = random.NextDouble () * (2 - 1) + 1;
			var initialVelocity = forward * r;

			var initialPosition = (PE.Vec3)transform.position + r * (PE.Vec3)transform.up - r * (PE.Vec3)transform.right;

			ps.AddParticle (initialPosition, initialVelocity);
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
				startSize = 0.2f,
			};
		}

		nrOfParticles = ps.ParticleCount; // For debugging

		GetComponent<ParticleSystem> ().SetParticles (unityParticles, unityParticles.Length);

	}
}
