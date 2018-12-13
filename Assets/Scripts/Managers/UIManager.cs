using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Text HitPoints;
    public Slider HealthSlider;

    private Actor.ActorStats playerStats;
    private GameObject slot1;
    private float timer = 0;

    // Use this for initialization
	void Start () {
        playerStats = FindObjectOfType<GameManager>().Player.Stats;

        playerStats.OnHitPointsChanged += ActorStats_OnHitPointsChanged;
        playerStats.OnMaxHitPointsChanged += ActorStats_OnMaxHitPointsChanged;

        slot1 = GameObject.Find("Slot1");
    }

    private void OnDestroy()
    {
        playerStats.OnHitPointsChanged -= ActorStats_OnHitPointsChanged;
        playerStats.OnMaxHitPointsChanged -= ActorStats_OnMaxHitPointsChanged;
    }

    private void ActorStats_OnHitPointsChanged(float HP)
    {
        HitPoints.text = "HP: " + HP + "/" + playerStats.MaxHealth;
        HealthSlider.value = HP;
    }
    private void ActorStats_OnMaxHitPointsChanged(float HP)
    {
        HitPoints.text = "HP: " + playerStats.CurrentHealth + "/" + HP;
        HealthSlider.maxValue = HP;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            slot1.GetComponentInChildren<Image>().color = new Color(.47f, .46f, 1f, 1f);
            timer = .3f;
        } else if (timer > 0)
        {
            slot1.GetComponentInChildren<Image>().color = Color.Lerp(new Color(.47f, .46f, 1f, .4f), new Color(.47f, .46f, 1f, 1f), timer/.3f);
            timer -= Time.deltaTime;
        }
    }
}