using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
	protected override void Start ()
    {
        base.Start();
	}
	
	void Update ()
    {
        GameObject player = FindObjectOfType<GameManager>().Player.gameObject;


        if (VectorTo(player.transform.position).magnitude <= Stats.DetectionRadius)
        {
            LookTowards(player.transform.position);
            if (CastRay(player))
            {
                WeaponPrefab.GetComponent<Weapon>().Attack(VectorTo(player.transform.position));
            }
        }
	}
}
