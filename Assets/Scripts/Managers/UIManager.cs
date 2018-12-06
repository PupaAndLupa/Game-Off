using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Text HP;
    public Slider HealthSlider;

    private Actor.ActorStats playerStats;

    // Use this for initialization
	void Start () {
        playerStats = FindObjectOfType<GameManager>().Player.Stats;

        playerStats.OnHPChanged += ActorStats_OnHPChanged;
        playerStats.OnMaxHPChanged += ActorStats_OnMaxHPChanged;
    }

    private void OnDestroy()
    {
        playerStats.OnHPChanged -= ActorStats_OnHPChanged;
        playerStats.OnMaxHPChanged -= ActorStats_OnMaxHPChanged;
    }

    private void ActorStats_OnHPChanged(float HP)
    {
        this.HP.text = "HP: " + HP + "/" + playerStats.MaxHealth;
        HealthSlider.value = HP;
    }
    private void ActorStats_OnMaxHPChanged(float HP)
    {
        this.HP.text = "HP: " + playerStats.CurrentHealth + "/" + HP;
        HealthSlider.maxValue = HP;
    }
}