using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleSkill : Skill
{
    public override bool Use()
    {
        if (base.Use())
        {
            player.GetComponent<Player>().Stats.IsInvisible = true;
            player.GetComponent<SpriteRenderer>().color /= 2;
            return true;
        }
        return false;
    }

    protected override void setCooldown()
    {
        base.setCooldown();
        player.GetComponent<Player>().Stats.IsInvisible = false;
        player.GetComponent<SpriteRenderer>().color *= 2;
    }
}
