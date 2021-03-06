﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Emitter : MonoBehaviour
{
	private PE.ParticleSystem ps = new PE.ParticleSystem (true);

	public float lifetime = 10;
	public float rate = 10;
	public float radius = 0.4f;
	public float particleSize = 0.1f;
	public float density = 2.0f;

	private float lastTime = 0;

	#pragma warning disable 0414
	// For debugging
	private int particleCount = 0;
	#pragma warning restore 0414

	private const int MAX_PARTICLES = 1000000;
	private ParticleSystem.Particle[] unityParticles = new ParticleSystem.Particle[MAX_PARTICLES];

	// Use this for initialization
	void Start ()
	{
		PE.Engine.instance.ParticleSystem = ps;
	}
	
	// Update is called once per frame
	void Update ()
	{
        ps.Update ();
		EmitParticles ();
		UpdateUnityParticleSystem ();
	}

	private void EmitParticles ()
	{
		var t = Time.fixedTime;
		var elapsed = t - lastTime;

		if (elapsed >= 1 / rate) {
			var particlesToEmit = Mathf.CeilToInt (elapsed / (1 / rate));

			float r = particleSize / 2;
			float mass = density * 4 * Mathf.PI * r * r * r / 3;
            
			for (var i = 0; i < particlesToEmit; i++) {
				// Add a random perturbation to initial position and velocity
				var initialVelocity = transform.forward + Random.onUnitSphere;

				var initialPosition = transform.position;
				var coords = Random.insideUnitCircle * radius;
				initialPosition += coords.x * transform.right + coords.y * transform.up;

				ps.Add (new PE.Particle (initialPosition, initialVelocity, mass, particleSize / 2, lifetime));
			}

			lastTime = t;
		}
	}

	private void UpdateUnityParticleSystem ()
	{
		var numParticles = ps.Count > MAX_PARTICLES ? MAX_PARTICLES : ps.Count;

		for (int i = 0; i < numParticles; i++) {
			var p = ps [i];

			unityParticles [i] = new ParticleSystem.Particle () {
				position = (Vector3)p.x - transform.position,
				startColor = Color.blue,
				startSize = particleSize,
			};
		}

		particleCount = ps.Count;

		GetComponent<ParticleSystem> ().SetParticles (unityParticles, numParticles);
	}
}
