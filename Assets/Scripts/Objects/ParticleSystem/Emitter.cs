using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Emitter : MonoBehaviour
{
	private PE.ParticleSystem ps = new PE.ParticleSystem ();

	public float rate = 10;
	public float radius = 0.4f;
	public float particleSize = 0.1f;

	private float lastTime = 0;

	// For debugging
	private int particleCount = 0;

	private const double MASS = 1e-5;

	private readonly Color startColor = Color.black;
	private readonly Color endColor = new Color (0, 0, 0, 0);

	private const int MAX_PARTICLES = 10000;
	private ParticleSystem.Particle[] unityParticles = new ParticleSystem.Particle[MAX_PARTICLES];

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
				var coords = Random.insideUnitCircle * radius;
				initialPosition += coords.x * transform.right + coords.y * transform.up;

				ps.Add (new PE.Particle (initialPosition, initialVelocity, MASS));
			}

			lastTime = t;
		}
	}

	private void UpdateUnityParticleSystem ()
	{
		var numParticles = ps.Count > MAX_PARTICLES ? MAX_PARTICLES : ps.Count;

		for (int i = 0; i < numParticles; i++) {
			var p = ps [i];

            unityParticles[i] = new ParticleSystem.Particle() {
                position = (Vector3)p.x - transform.position,
                startColor = startColor,//Color.Lerp (startColor, endColor, 0.5f*(float)p.age),
				startSize = particleSize,
			};
		}

		particleCount = ps.Count;

		GetComponent<ParticleSystem> ().SetParticles (unityParticles, numParticles);
	}
}
