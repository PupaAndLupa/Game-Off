using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsUpgrader : MonoBehaviour
{
    private Actor actor;
    public Actor Actor {
        private get {
            return actor ?? FindObjectOfType<GameManager>().Player;
        }

        set
        {
            actor = value;
        }
    }

    public void UpgradeMaxHealth(float value)
    {
        Actor.UpgradeStats(ActorStats.UpgradableStats.MAX_HP, value);
    }

    public void UpgradeDamageModifier(float value)
    {
        Actor.UpgradeStats(ActorStats.UpgradableStats.DMG_MOD, value);
    }

    public void UpgradeDamageReduction(float value)
    {
        Actor.UpgradeStats(ActorStats.UpgradableStats.DMG_RED, value);
    }

    public void UpgradeCooldownReduction(float value)
    {
        Actor.UpgradeStats(ActorStats.UpgradableStats.CD_RED, value);
    }
}
