using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulllet : Projectile {

	void Start () {
        Destroy(gameObject, 2f);
	}
	
	void Update () {
		
	}

    private void FixedUpdate()
    {
        Movement.Move(Stats.Movespeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "projectile" )
        {
            Debug.Log(collision.gameObject.tag);
            Destroy(gameObject);
        }
    }
}
