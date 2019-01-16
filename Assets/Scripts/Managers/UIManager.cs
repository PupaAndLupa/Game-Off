using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Text HitPoints;
    public Text LevelText;
    public Slider HealthSlider;
    public Slider ExpSlider;

    private Actor player;
    private Weapon playerWeapon;
    private SpriteRenderer playerSR;
    private ActorStats playerStats;
    private GameObject slot1;
    private GameObject slot2;
    private GameObject slot3;
    private GameObject slot4;
    private float fisrtTimer = 0;
    private float secondTimer = 0;
    private float thirdTimer = 0;
    private float fourthTimer = 0;
    private bool speedChanged = false;

    // Use this for initialization
    void Start () {
        player = FindObjectOfType<GameManager>().Player;
        playerSR = player.GetComponent<SpriteRenderer>();
        playerStats = player.Stats;

        playerStats.OnHitPointsChanged += ActorStats_OnHitPointsChanged;
        playerStats.OnMaxHitPointsChanged += ActorStats_OnMaxHitPointsChanged;
        playerStats.OnLevelChanged += ActorStats_OnOnLevelChanged;
        playerStats.OnExpChanged += ActorStats_OnExpChanged;

        playerStats.CurrentHealth = playerStats.MaxHealth;

        slot1 = GameObject.Find("Slot1");
        slot2 = GameObject.Find("Slot2");
        slot3 = GameObject.Find("Slot3");
        slot4 = GameObject.Find("Slot4");

        ExpSlider.value = 0;
        ExpSlider.maxValue = ExpPerLevel(0);
    }

    private void OnDestroy()
    {
        playerStats.OnHitPointsChanged -= ActorStats_OnHitPointsChanged;
        playerStats.OnMaxHitPointsChanged -= ActorStats_OnMaxHitPointsChanged;
        playerStats.OnLevelChanged -= ActorStats_OnOnLevelChanged;
        playerStats.OnExpChanged -= ActorStats_OnExpChanged;
    }

    private long ExpPerLevel(int level)
    {
        return (level + 1) * 100;
    }

    private void ActorStats_OnExpChanged(long experience)
    {
        if (experience >= ExpSlider.maxValue)
        {
            playerStats.Level++;
        } else
        {
            ExpSlider.value = experience;
        }
    }

    private void ActorStats_OnOnLevelChanged(int level)
    {
        LevelText.text = level.ToString();

        long expPerCurrLevel = ExpPerLevel(level - 2);
        ExpSlider.maxValue = ExpPerLevel(level - 1);

        long experienceLeft = playerStats.experience > expPerCurrLevel ? playerStats.experience - expPerCurrLevel : 0;

        ExpSlider.value = experienceLeft;
        playerStats.experience = experienceLeft;
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
            slot1.GetComponentInChildren<Image>().color = new Color(.9f, .9f, 1f, .4f);
            playerStats.CurrentHealth += 10;
            fisrtTimer = .3f * (1 / playerStats.CooldownReduction);
        } else if (fisrtTimer > 0)
        {
            slot1.GetComponentInChildren<Image>().color = Color.Lerp(new Color(.9f, .9f, 1f, 1f), new Color(.9f, .9f, 1f, .4f), fisrtTimer/.3f);
            fisrtTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (secondTimer <= 0)
            {
                slot2.GetComponentInChildren<Image>().color = new Color(1f, .9f, 1f, .4f);
                playerStats.DamageModifier *= 2;
                secondTimer = 20f * (1 / playerStats.CooldownReduction);
            }
        } else if (secondTimer > -1)
        {
            if (secondTimer <= 0)
            {
                slot2.GetComponentInChildren<Image>().color = new Color(.9f, .9f, 1f, 1f);
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
                slot3.GetComponentInChildren<Image>().color = new Color(1f, .9f, 1f, .4f);

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
                slot3.GetComponentInChildren<Image>().color = new Color(.9f, .9f, 1f, 1f);
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

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (fourthTimer <= 0)
            {
                slot4.GetComponentInChildren<Image>().color = new Color(1f, .9f, 1f, .4f);

                if (!speedChanged)
                {
                    playerStats.Movespeed *= 2;
                    player.GetComponent<Player>().Weapons[player.GetComponent<Player>().currentWeaponIndex].GetComponent<Weapon>().Stats.Cooldown /= 5;

                    speedChanged = true;
                }

                fourthTimer = 30f;
            }
        }
        else if (fourthTimer > -1)
        {
            if (fourthTimer <= 0)
            {
                slot4.GetComponentInChildren<Image>().color = new Color(.9f, .9f, 1f, 1f);
            }

            if (fourthTimer < 20f)
            {
                if (speedChanged)
                {
                    playerStats.Movespeed /= 2;
                    player.GetComponent<Player>().Weapons[player.GetComponent<Player>().currentWeaponIndex].GetComponent<Weapon>().Stats.Cooldown *= 5;

                    speedChanged = false;
                }
            }

            fourthTimer -= Time.deltaTime;
        }
    }
}