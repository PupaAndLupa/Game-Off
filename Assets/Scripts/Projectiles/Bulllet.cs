using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulllet : Projectile {

	// Use this for initialization
	void Start () {
        Destroy(gameObject, 2f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        Movement.Move(Stats.Movespeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "projectile")
            Destroy(gameObject);
    }
}
