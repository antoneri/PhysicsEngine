using UnityEngine;
using System.Collections;

public class ParticleSystem : MonoBehaviour {
	public int N = 100;
	Particle[] particles;
	public double[] x_init = { 0, 1, 0 };
	public double[] v_init = { 10, 10, 0 };

	// Use this for initialization
	void Start () {
		particles = new Particle[N];

		foreach (var p in particles) {
			p.x = x_init;
			p.v = v_init;
			p.f = { 0, 0, -9.82 };
			p.m = 1e-4;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
