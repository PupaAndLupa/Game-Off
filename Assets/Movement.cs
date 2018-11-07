using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public float speed = 1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        int horizontal = 0, vertical = 0;

        if (Input.GetKey("a"))
            horizontal -= 1;
            
        if (Input.GetKey("d"))
            horizontal += 1;

        if (Input.GetKey("w"))
            vertical += 1;

        if (Input.GetKey("s"))
            vertical -= 1;
   

        transform.Translate(horizontal * speed * Time.deltaTime, vertical * speed * Time.deltaTime, 0);
    }
}
