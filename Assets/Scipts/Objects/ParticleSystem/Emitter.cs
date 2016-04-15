using UnityEngine;
using UnityEditor;
using System.Collections;

public class Emitter : MonoBehaviour {

    public float rate = 1.0f;

    //public GameObject particlePrefab;

    private PE.ParticleSystem ps;

    private float lastEmitted = 0;
    private int nrOfParticles = 0;
    
	// Use this for initialization
	void Start () {
        ps = new PE.ParticleSystem();
        PE.Engine.instance.addParticleSystem(ps);
    }
	
	// Update is called once per frame
	void Update () {
        float t = Time.fixedTime;
        
        if (t - lastEmitted >= 1/rate)
        {
            /* Add particle to particleSystem */
            PE.Particle p = new PE.Particle()
            {
                x = PE.UnityAdapter.PEVector(transform.position),
                v = new PE.Vec3(1, 0, 0),
                f = new PE.Vec3(0),
                m = 1.0,
                age = 0.0
            };
            ps.AddParticle(p);
            lastEmitted = t;
        }

        ps.Update();

        ParticleSystem.Particle[] unityParticles = new ParticleSystem.Particle[ps.particles.Count];

        for (int i = 0; i < ps.particles.Count; i++)
        {
            PE.Particle p = ps.particles[i];
            ParticleSystem.Particle unityP = new ParticleSystem.Particle();
            unityP.position =  PE.UnityAdapter.UnityVector(p.x) - transform.position;
            unityP.startColor = new Color(1, 0f, 0f);
            unityP.startSize = 0.5f;

            unityParticles[i] = unityP;
        }

        nrOfParticles = ps.particles.Count;
        GetComponent<ParticleSystem>().SetParticles(unityParticles, unityParticles.Length);
    }
}
