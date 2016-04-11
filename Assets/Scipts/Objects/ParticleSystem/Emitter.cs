using UnityEngine;
using System.Collections;

public class Emitter : MonoBehaviour {

    public float rate = 1.0f;

    public int resolution = 10;
    private ParticleSystem.Particle[] points;
    //public GameObject particlePrefab;

    private PE.ParticleSystem ps;

    private float lastEmitted = 0;
    
	// Use this for initialization
	void Start () {
        ps = new PE.ParticleSystem();
        PE.Engine.instance.addParticleSystem(ps);

        points = new ParticleSystem.Particle[resolution*8];
        float increment = 1f / (resolution - 1);
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                float p = x * increment;
                points[y*resolution + x].position = new Vector3(p, 0.2f*y, 0f);
                points[y*resolution + x].startColor = new Color(x, 0f, 0f);
                points[y*resolution + x].startSize = 0.1f;
            }
            
        }
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
                v = new PE.Vec3(0.1, 0, 0),
                f = new PE.Vec3(0),
                m = 0.0,
                age = 0.0
            };

            GetComponent<ParticleSystem>().SetParticles(points, points.Length);

            ps.AddParticle(p); 

            lastEmitted = t;

        }
        
	}
}
