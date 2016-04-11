using UnityEngine;
using System.Collections;

public class Engine : MonoBehaviour {


	// Use this for initialization
	void Start () {

        StartCoroutine(GameLoop());

    }
	
	private IEnumerator GameLoop()
    {
        /* Simulation loop */
        while (true)
        {

            /*Create particles at emitter
            (Remove particles at sinks or when they expire in time) */

            /*Do inter-particle collision detection and construct a
            neighbour list – or use a fixed interaction list (cloth). */

            
            /*Loop over neighbour lists and compute interaction
            forces.Accumulate the forces.Use Newton’s third law. */

            /*Accumulate external forces from e.g.gravity.*/

            /*Accumulate dissipative forces, e.g.drag and viscous drag. */

            /* Find contact sets with external boundaries, e.g.a plane.
            Handle external boundary conditions by reflecting the
            the velocities. */

            /*Take a timestep and integrate using e.g.Verlet / Leap Frog */

             yield return new WaitForFixedUpdate();
        }
    }
}



