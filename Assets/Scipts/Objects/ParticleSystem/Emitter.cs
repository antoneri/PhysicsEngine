using UnityEngine;
using System.Collections;

using PE;

public class Emitter : MonoBehaviour {

    public float rate = 1.0f;

    //public GameObject particlePrefab;

    private PE.ParticleSystem ps;

    private float lastEmitted = 0;
    
	// Use this for initialization
	void Start () {
        ps = new PE.ParticleSystem();
        Engine.instance.addParticleSystem(ps);
	}
	
	// Update is called once per frame
	void Update () {
        float t = Time.fixedTime;

        Vector3 pos = transform.position;
        
        if (t - lastEmitted >= 1/rate)
        {
            /* Add particle to particleSystem */
            PE.Particle p = new PE.Particle()
            {
                x = PE.UnityAdapter.PEVector(pos),
                v = new Vec3(0.1, 0, 0),
                f = new Vec3(0),
                m = 0.0,
                age = 0.0
            };

            ps.AddParticle(p); 

            lastEmitted = t;

        }
        
	}
}
