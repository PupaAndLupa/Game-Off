using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Text HitPoints;
    public Slider HealthSlider;

    private Actor player;
    private SpriteRenderer playerSR;
    private ActorStats playerStats;
    private GameObject slot1;
    private GameObject slot2;
    private GameObject slot3;
    private float fisrtTimer = 0;
    private float secondTimer = 0;
    private float thirdTimer = 0;

    // Use this for initialization
	void Start () {
        player = FindObjectOfType<GameManager>().Player;
        playerSR = player.GetComponent<SpriteRenderer>();
        playerStats = player.Stats;

        playerStats.OnHitPointsChanged += ActorStats_OnHitPointsChanged;
        playerStats.OnMaxHitPointsChanged += ActorStats_OnMaxHitPointsChanged;

        playerStats.CurrentHealth = playerStats.MaxHealth;

        slot1 = GameObject.Find("Slot1");
        slot2 = GameObject.Find("Slot2");
        slot3 = GameObject.Find("Slot3");
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
            playerStats.CurrentHealth += 10;
            fisrtTimer = .3f * (1 / playerStats.CooldownReduction);
        } else if (fisrtTimer > 0)
        {
            slot1.GetComponentInChildren<Image>().color = Color.Lerp(new Color(.47f, .46f, 1f, .4f), new Color(.47f, .46f, 1f, 1f), fisrtTimer/.3f);
            fisrtTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (secondTimer <= 0)
            {
                slot2.GetComponentInChildren<Image>().color = new Color(1f, .46f, 1f, 1f);
                playerStats.DamageModifier *= 2;
                secondTimer = 20f * (1 / playerStats.CooldownReduction);
            }
        } else if (secondTimer > -1)
        {
            if (secondTimer <= 0)
            {
                slot2.GetComponentInChildren<Image>().color = new Color(.47f, .46f, 1f, .4f);
            }

            if (secondTimer < 15f)
            {
                playerStats.DamageModifier = 1;
            }

            secondTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (thirdTimer <= 0)
            {
                slot3.GetComponentInChildren<Image>().color = new Color(1f, .46f, 1f, 1f);

                var playerColor = playerSR.color;
                playerColor.a = 0.3f;
                playerSR.color = playerColor;

                playerStats.IsInvisible = true;
                thirdTimer = 60f * (1 / playerStats.CooldownReduction);
            }
        }
        else if (thirdTimer > -1)
        {
            if (thirdTimer <= 0)
            {
                slot3.GetComponentInChildren<Image>().color = new Color(.47f, .46f, 1f, .4f);
            }

            if (thirdTimer < 40f)
            {
                playerStats.IsInvisible = false;

                var playerColor = playerSR.color;
                playerColor.a = 1;
                playerSR.color = playerColor;
            }

            thirdTimer -= Time.deltaTime;
        }
    }
}