using UnityEngine;
using System.Collections;

public class cameraController : MonoBehaviour {

    private Vector3 lastMouse;

    public float speed = 5.0f;
    public float sensitivity = 0.25f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            lastMouse = Input.mousePosition;
        }

		if (Input.GetMouseButton(0)) {
            Vector3 delta = Input.mousePosition - lastMouse;

            delta *= sensitivity;
            Vector3 rot = new Vector3(transform.eulerAngles.x - delta.y, transform.eulerAngles.y + delta.x, 0);
            transform.eulerAngles = rot;

            lastMouse = Input.mousePosition;

        }

        float forward = 0; //Input.GetAxis ("Vertical");
        float side = 0; // Input.GetAxis ("Horizontal");
        float up = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            forward = 1.0f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            forward = -1.0f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            side = 1.0f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            side = -1.0f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            up = 1.0f;
        } else if (Input.GetKey(KeyCode.Q))
        {
            up = -1.0f;
        }

        Vector3 dir = new Vector3(side, up, forward);
        dir.Normalize();

        Vector3 movement = dir * speed * Time.deltaTime;

		transform.Translate (movement);

	}


}
