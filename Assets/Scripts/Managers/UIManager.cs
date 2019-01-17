using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Text ScoreText;
    public Text HitPoints;
    public Text LevelText;
    public Text CoinsText;
    public Slider HealthSlider;
    public Slider ExpSlider;

    private long score = 0;

    private Actor player;
    private ActorStats playerStats;
    private GameObject[] slots;
    private int coins = 0;

    // Use this for initialization
    void Start () {
        player = FindObjectOfType<GameManager>().Player;
        playerStats = player.Stats;

        playerStats.OnHitPointsChanged += ActorStats_OnHitPointsChanged;
        playerStats.OnMaxHitPointsChanged += ActorStats_OnMaxHitPointsChanged;
        playerStats.OnLevelChanged += ActorStats_OnOnLevelChanged;
        playerStats.OnExpChanged += ActorStats_OnExpChanged;

        for (int i = 0; i < 4; i++)
        {
            (player as Player).Skills[i].OnReady += Skill_OnReady;
            (player as Player).Skills[i].OnUse += Skill_OnUse;
            (player as Player).Skills[i].OnCooldown += Skill_OnCooldown;
        }

        playerStats.CurrentHealth = playerStats.MaxHealth;

        slots = new GameObject[] { GameObject.Find("Slot1"), GameObject.Find("Slot2"), GameObject.Find("Slot3"), GameObject.Find("Slot4") };

        ExpSlider.value = 0;
        ExpSlider.maxValue = ExpPerLevel(0);
    }

    public void AddScore(long score)
    {
        this.score += score;
        ScoreText.text = string.Format("{0:D7}", this.score);
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        CoinsText.text = "Coins: " + coins;
    }

    private void OnDestroy()
    {
        playerStats.OnHitPointsChanged -= ActorStats_OnHitPointsChanged;
        playerStats.OnMaxHitPointsChanged -= ActorStats_OnMaxHitPointsChanged;
        playerStats.OnLevelChanged -= ActorStats_OnOnLevelChanged;
        playerStats.OnExpChanged -= ActorStats_OnExpChanged;

        for (int i = 0; i < 4; i++)
        {
            (player as Player).Skills[i].OnReady -= Skill_OnReady;
            (player as Player).Skills[i].OnUse -= Skill_OnUse;
            (player as Player).Skills[i].OnCooldown -= Skill_OnCooldown;
        }
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

    private void Skill_OnReady(int index, Texture2D image)
    {
        Sprite old = slots[index].GetComponentInChildren<Image>().sprite;
        slots[index].GetComponentInChildren<Image>().sprite = Sprite.Create(image, old.rect, old.pivot);
        slots[index].GetComponentInChildren<Image>().color = new Color(1f, 1f, 1f);
    }

    private void Skill_OnUse(int index, Texture2D image)
    {
        Sprite old = slots[index].GetComponentInChildren<Image>().sprite;
        slots[index].GetComponentInChildren<Image>().sprite = Sprite.Create(image, old.rect, old.pivot);
        slots[index].GetComponentInChildren<Image>().color = new Color(1f, 1f, 1f, 0.5f);
    }

    private void Skill_OnCooldown(int index, Texture2D image)
    {
        Sprite old = slots[index].GetComponentInChildren<Image>().sprite;
        slots[index].GetComponentInChildren<Image>().sprite = Sprite.Create(image, old.rect, old.pivot);
        slots[index].GetComponentInChildren<Image>().color = new Color(1f, 1f, 1f, 0f);
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            slot1.GetComponentInChildren<Image>().color = new Color(.9f, .9f, 1f, .4f);
            playerStats.CurrentHealth = playerStats.MaxHealth;
            firstTimer = 100f * (1 / playerStats.CooldownReduction);
        } else if (firstTimer > 0)
        {
            slot1.GetComponentInChildren<Image>().color = Color.Lerp(new Color(.9f, .9f, 1f, 1f), new Color(.9f, .9f, 1f, .4f), firstTimer/.3f);
            firstTimer -= Time.deltaTime;
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
                    (player as Player).ModifyWeaponSpeed(0.2f);

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
                    (player as Player).ModifyWeaponSpeed(5f);

                    speedChanged = false;
                }
            }

            fourthTimer -= Time.deltaTime;
        }*/
    }
}