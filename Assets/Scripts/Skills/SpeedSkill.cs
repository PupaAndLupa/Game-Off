using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedSkill :Skill
{
    public override bool Use(GameObject player)
    {
        if (base.Use(player))
        {
            player.GetComponent<Player>().ModifyWeaponSpeed(2f);
            player.GetComponent<Player>().Stats.Movespeed *= 2;
            return true;
        }
        return false;
    }

    protected override void setCooldown()
    {
        base.setCooldown();
        player.GetComponent<Player>().ModifyWeaponSpeed(1f / 2f);
        player.GetComponent<Player>().Stats.Movespeed /= 2;
    }
}
