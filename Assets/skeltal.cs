using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skeltal : MonoBehaviour {

    private bool keyPressed;
    private bool playingAnimation = false;

    private int speed = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        keyPressed = false;

        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            playingAnimation = false;

        if (Input.GetKey("w") && !playingAnimation)
        {
            GetComponent<Animator>().Play("Run");
            Vector3 mover = transform.forward;
            mover = new Vector3(mover.x, mover.y);
            GetComponent<Transform>().Translate(0, 0, speed * Time.deltaTime);
            keyPressed = true;
        }

        if (Input.GetKey("space") && !playingAnimation)
        {
            GetComponent<Animator>().Play("Attack");
            playingAnimation = true;
        }

        if (!keyPressed && !playingAnimation)
            GetComponent<Animator>().Play("Stand");

            
	}
}
