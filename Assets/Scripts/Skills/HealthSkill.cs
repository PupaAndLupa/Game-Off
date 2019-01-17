using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSkill : Skill
{
    public override bool Use(GameObject player)
    {
        if (base.Use(player))
        {
            player.GetComponent<Player>().Stats.CurrentHealth += player.GetComponent<Player>().Stats.MaxHealth * 0.3f;
            return true;
        }
        return false;
    }
}
