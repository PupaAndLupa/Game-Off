using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCamera : MonoBehaviour {

    private float speed = 1f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("w"))
            transform.Translate(0, 0, speed * Time.deltaTime);

        if (Input.GetKey("s"))
            transform.Translate(0, 0, -speed * Time.deltaTime);

        if (Input.GetKey("a"))
            transform.Translate(-speed * Time.deltaTime, 0, 0);

        if (Input.GetKey("d"))
            transform.Translate(speed * Time.deltaTime, 0, 0);


        transform.Rotate(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
	}
}
