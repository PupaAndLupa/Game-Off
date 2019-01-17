using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReductionSkill : Skill
{
    public override bool Use()
    {
        if (base.Use())
        {
            player.GetComponent<Player>().Stats.DamageReduction *= 6;
            return true;
        }
        return false;
    }

    protected override void setCooldown()
    {
        base.setCooldown();
        player.GetComponent<Player>().Stats.DamageReduction /= 6;
    }
}
