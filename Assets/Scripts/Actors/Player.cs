using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
	void Start () {
        Weapon.GetComponent<Weapon>().SetPlayerModifier(Stats.DamageModifier);
	}
	
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            Weapon.GetComponent<Weapon>().Shoot(mousePosition - transform.position);
        }
	}

    private void FixedUpdate()
    {
        Movement.Move(Stats.Movespeed);
        Movement.Rotate();
    }
}
