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

        if (Input.GetKey("up") && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Unstoppable"))
        {
            animator.SetBool("isMoving", true);
            GetComponent<Transform>().Translate(0, 0, speed * Time.deltaTime);
        }

        if (Input.GetKey("left") && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Unstoppable"))
        {
            animator.SetBool("isMoving", true);
            GetComponent<Transform>().Translate(-speed * Time.deltaTime, 0, 0);
        }

        if (Input.GetKey("right") && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Unstoppable"))
        {
            animator.SetBool("isMoving", true);
            GetComponent<Transform>().Translate(speed * Time.deltaTime, 0, 0);
        }

        if (Input.GetKey("down") && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Unstoppable"))
        {
            animator.SetBool("isMoving", true);
            GetComponent<Transform>().Translate(0, 0, -speed * Time.deltaTime);
        }

        if (Input.GetKey("space") && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Unstoppable"))
        {
            animator.SetBool("isAttacking", true);
        }
    }
}
