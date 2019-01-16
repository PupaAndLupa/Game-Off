using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    private AIDestinationSetter aIDestinationSetter;
    private AIPath aIPath;
    private GameObject player;
    private Weapon weapon;

    protected override void Start ()
    {
        base.Start();

        player = FindObjectOfType<GameManager>().Player.gameObject;  // TEMP
        aIDestinationSetter = gameObject.GetComponent<AIDestinationSetter>();

        aIPath = gameObject.GetComponent<AIPath>();
        aIPath.endReachedDistance = Stats.DetectionRadius * 0.75f;

        weapon = WeaponPrefab.GetComponent<Weapon>();
    }
	
	protected override void Update ()
    {
        base.Update();

        if (IsTotallyDead)
            return;

        if (VectorTo(player.transform.position).magnitude <= Stats.DetectionRadius)
        {
            aIDestinationSetter.target = player.transform;

            if (CastRay(player))
            {
                LookTowards(player.transform.position);
                if (VectorTo(player.transform.position).magnitude <= weapon.Stats.Range + 1)
                {
                    weapon.Attack();
                }

                aIPath.endReachedDistance = weapon.Stats.Range * 0.75f;
            }
            else
            {
                aIPath.endReachedDistance = Mathf.Max(aIPath.endReachedDistance - 0.1f, 0);
            }
        } else
        {
            aIDestinationSetter.target = null;
        }
	}

    public override void Die()
    {
        base.Die();

        GetComponent<Seeker>().enabled = false;
        GetComponent<AIPath>().enabled = false;
        GetComponent<AIDestinationSetter>().enabled = false;
    }
}
