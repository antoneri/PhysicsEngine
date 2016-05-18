using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PE
{
	public class ParticleSystem : List<Particle>
	{

        public ParticleSystem() { }

        public ParticleSystem(bool lifetime)
        {
            this.lifetime = lifetime;
        }

        public bool lifetime = false;

        public List<Constraint> constraints = new List<Constraint>();

        // Update is called once per frame
        public void Update ()
		{
            var dt = Time.deltaTime;

            if (lifetime)
            {
                for (int i = this.Count - 1; i >= 0; i--)
                {
                    var p = this[i];
                    p.age += dt;

                    if (p.age > p.lifetime)
                    {
                        this.RemoveAt(i);
                    }
                }
            }


		}

		public double Energy {
			get {
				return 0.5 * this.Sum (p => p.m * Vec3.Dot (p.v, p.v));
			}
		}
	}
}

