using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    public GameObject test;


	void Start () {

	}
	
	void Update ()
    {
        //TEMPORARY
        if (Input.GetMouseButtonDown(0))
        {
            GameObject clone = Instantiate(test, transform.position, transform.rotation, transform.parent);

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;

            clone.GetComponent<Projectile>().Movement.SetDirection(mousePosition - transform.position);

            clone.transform.position += (mousePosition - transform.position).normalized * (GetComponent<CircleCollider2D>().radius + 0.1f);
        }
        //TEMPORARY
	}

    private void FixedUpdate()
    {
        Movement.Move(Stats.Movespeed);
        Movement.Rotate();
    }
}
