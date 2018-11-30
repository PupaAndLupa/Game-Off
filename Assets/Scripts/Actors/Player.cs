using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{

	void Start () {

	}
	
	void Update () {

	}

    private void FixedUpdate()
    {
        movement.Move(Stats.Movespeed);
        movement.Rotate();
    }
}
