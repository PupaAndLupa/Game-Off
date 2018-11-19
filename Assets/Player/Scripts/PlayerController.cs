using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private float walkSpeed = 100f;

    Vector3 moveDirection;
    Rigidbody rb;


	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        moveDirection = (horizontalInput * transform.right + verticalInput * transform.forward).normalized;
	}

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector3 yComponent = new Vector3(0, rb.velocity.y);
       rb.velocity = moveDirection * walkSpeed * Time.deltaTime + yComponent;
    }
}
