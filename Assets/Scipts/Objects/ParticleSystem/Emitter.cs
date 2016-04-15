using UnityEngine;
using UnityEditor;
using System.Collections;

public class Emitter : MonoBehaviour
{

	public float rate = 1.0f;

	//public GameObject particlePrefab;

	private PE.ParticleSystem ps;

	private float lastEmitted = 0;
	private int nrOfParticles = 0;
    
	// Use this for initialization
	void Start ()
	{
		ps = new PE.ParticleSystem ();
		PE.Engine.instance.addParticleSystem (ps);
	}
	
	// Update is called once per frame
	void Update ()
	{
		float t = Time.fixedTime;
        
		if (t - lastEmitted >= 1 / rate) {
			/* Add particle to particleSystem */
			ps.AddParticle (transform.position, new PE.Vec3 (1, 0, 0));
			lastEmitted = t;
		}

		UpdateUnityParticleSystem ();
	}

	private void UpdateUnityParticleSystem ()
	{
		ParticleSystem.Particle[] unityParticles = new ParticleSystem.Particle[ps.particles.Count];

		for (int i = 0; i < ps.particles.Count; i++) {
			PE.Particle p = ps.particles [i];

			unityParticles [i] = new ParticleSystem.Particle () {
				position = (Vector3)p.x - transform.position,
				startColor = new Color (1, 0f, 0f),
				startSize = 0.2f,
			};
		}

		nrOfParticles = ps.particles.Count; // For debugging

		GetComponent<ParticleSystem> ().SetParticles (unityParticles, unityParticles.Length);

	}
}
