using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skeltal : MonoBehaviour {

    private bool keyPressed;
    private bool playingAnimation = false;
    private Animator animator;

    private int speed = 1;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        animator.SetBool("isMoving", false);
        animator.SetBool("isAttacking", false);

        if (Input.GetKey("w") && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Unstoppable"))
        {
            animator.SetBool("isMoving", true);
            Vector3 mover = transform.forward;
            mover = new Vector3(mover.x, mover.y);
            GetComponent<Transform>().Translate(0, 0, speed * Time.deltaTime);
            keyPressed = true;
        }

        if (Input.GetKey("space") && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Unstoppable"))
        {
            animator.SetBool("isAttacking", true);
        }
        
	}
}
