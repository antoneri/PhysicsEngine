using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Emitter : MonoBehaviour
{
	public float rate = 10;

	private PE.ParticleSystem ps = new PE.ParticleSystem ();

	private float lastTime = 0;

	// For debugging
	private int particleCount = 0;

	private const float particleSize = 0.1f;

	// Use this for initialization
	void Start ()
	{
		PE.Engine.instance.AddParticleSystem (ps);
	}
	
	// Update is called once per frame
	void Update ()
	{
		EmitParticles ();
		UpdateUnityParticleSystem ();
	}

	private void EmitParticles ()
	{
		var t = Time.fixedTime;
		var elapsed = t - lastTime;

		if (elapsed >= 1 / rate) {
			var particlesToEmit = Mathf.CeilToInt (elapsed / (1 / rate));

			for (var i = 0; i < particlesToEmit; i++) {
				// Add a random perturbation to initial position and velocity
				var initialVelocity = transform.forward + Random.onUnitSphere;

				var initialPosition = transform.position;
				var coords = Random.insideUnitCircle * 0.4f;
				initialPosition += coords.x * transform.right + coords.y * transform.up;

				ps.AddParticle (initialPosition, initialVelocity);
			}

			lastTime = t;
		}
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
				startSize = particleSize,
			};
		}

		particleCount = ps.Count;

		GetComponent<ParticleSystem> ().SetParticles (unityParticles, unityParticles.Length);
	}
}
