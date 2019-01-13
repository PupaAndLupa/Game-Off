using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    private AIDestinationSetter aIDestinationSetter;
    private AIPath aIPath;

    protected override void Start ()
    {
        base.Start();
        aIDestinationSetter = gameObject.GetComponent(typeof(AIDestinationSetter)) as AIDestinationSetter;
        aIPath = gameObject.GetComponent(typeof(AIPath)) as AIPath;

        aIPath.endReachedDistance = Stats.DetectionRadius * 0.75f;
	}
	
	protected override void Update ()
    {
        base.Update();

        GameObject player = FindObjectOfType<GameManager>().Player.gameObject;  // TEMP
        aIDestinationSetter.target = player.transform;


        if (VectorTo(player.transform.position).magnitude <= Stats.DetectionRadius)
        {
            if (CastRay(player))
            {
                LookTowards(player.transform.position);
                WeaponPrefab.GetComponent<Weapon>().Attack();

                aIPath.endReachedDistance = Stats.DetectionRadius * 0.75f;
            }
            else
            {
                aIPath.endReachedDistance = Mathf.Max(aIPath.endReachedDistance - 0.1f, 0);
            }
        }
	}
}
