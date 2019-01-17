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

    private UIManager uimanager;
    private UIManager uiManager
    {
        get
        {
            return uimanager ?? FindObjectOfType<UIManager>();
        }

        set
        {
            uimanager = value;
        }
    }

    public void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
    }

    public void UpgradeMaxHealth(float value)
    {
        Actor.UpgradeStats(ActorStats.UpgradableStats.MAX_HP, value);
        uiManager.TryDisableButtons();
    }

    public void UpgradeDamageModifier(float value)
    {
        Actor.UpgradeStats(ActorStats.UpgradableStats.DMG_MOD, value);
        uiManager.TryDisableButtons();
    }

    public void UpgradeDamageReduction(float value)
    {
        Actor.UpgradeStats(ActorStats.UpgradableStats.DMG_RED, value);
        uiManager.TryDisableButtons();
    }

    public void UpgradeCooldownReduction(float value)
    {
        Actor.UpgradeStats(ActorStats.UpgradableStats.CD_RED, value);
        uiManager.TryDisableButtons();
    }
}
