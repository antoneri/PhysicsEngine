using UnityEngine;
using System.Collections;

public class Emitter : MonoBehaviour {

    public Vector3 position = new Vector3(0,0,0);
    public float rate = 1.0f;

    public GameObject particlePrefab;

    private PE.ParticleSystem ps;

    private float lastEmitted = 0;
    
	// Use this for initialization
	void Start () {
        ps = new PE.ParticleSystem();
	}
	
	// Update is called once per frame
	void Update () {
        float t = Time.fixedTime;
        
        if (t - lastEmitted >= 1/rate)
        {   
            /* Add particle to particleSystem */   
            lastEmitted = t;
        }
        
	}
}
