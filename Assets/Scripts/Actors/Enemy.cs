﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
	protected override void Start ()
    {
        base.Start();
	}
	
	protected override void Update ()
    {
        base.Update();

        GameObject player = FindObjectOfType<GameManager>().Player.gameObject;  // TEMP


        if (VectorTo(player.transform.position).magnitude <= Stats.DetectionRadius)
        {
            if (CastRay(player))
            {
                LookTowards(player.transform.position);
                WeaponPrefab.GetComponentInChildren<Weapon>().Attack();
            }
        }
	}
}
