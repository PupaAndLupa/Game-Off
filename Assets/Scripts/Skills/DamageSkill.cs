using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSkill : Skill
{
    public override bool Use()
    {
        if (base.Use())
        {
            player.GetComponent<Player>().Stats.DamageModifier *= 2;
            return true;
        }
        return false;
    }

    protected override void setCooldown()
    {
        base.setCooldown();
        player.GetComponent<Player>().Stats.DamageModifier /= 2;
    }
}
