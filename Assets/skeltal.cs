using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skeltal : MonoBehaviour {

    private bool keyPressed;

    private int speed = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        keyPressed = false;
        if (Input.GetKey("w"))
        {
            GetComponent<Animator>().Play("Run");
            Vector3 mover = transform.forward;
            mover = new Vector3(mover.x, mover.y);
            GetComponent<Transform>().Translate(0, 0, speed * Time.deltaTime);
            keyPressed = true;
        }

        if (!keyPressed)
            GetComponent<Animator>().Play("Stand");

            
	}
}
