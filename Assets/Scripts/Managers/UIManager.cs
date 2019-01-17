using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Text HoverText;

    public Text MaxHPStatsOnScreen;
    public Text DMGModStatsOnScreen;
    public Text DMGRedStatsOnScreen;
    public Text CooldownReductionStatsOnScreen;

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

    void Start () {
        player = FindObjectOfType<GameManager>().Player;
        playerStats = player.Stats;

        playerStats.OnCooldownReductionChanged += ActorStats_ChangeCooldownReductionStatsOnScreen;
        playerStats.OnDamageModifierChanged += ActorStats_ChangeDMGModStatsOnScreen;
        playerStats.OnDamageReductionChanged += ActorStats_ChangeDMGRedStatsOnScreen;
        playerStats.OnHitPointsChanged += ActorStats_OnHitPointsChanged;
        playerStats.OnMaxHitPointsChanged += ActorStats_ChangeMaxHPStatOnScreen;
        playerStats.OnMaxHitPointsChanged += ActorStats_OnMaxHitPointsChanged;
        playerStats.OnLevelChanged += ActorStats_OnOnLevelChanged;
        playerStats.OnExpChanged += ActorStats_OnExpChanged;

        for (int i = 0; i < 4; i++)
        {
            (player as Player).Skills[i].GetComponent<Skill>().OnReady += Skill_OnReady;
            (player as Player).Skills[i].GetComponent<Skill>().OnUse += Skill_OnUse;
            (player as Player).Skills[i].GetComponent<Skill>().OnCooldown += Skill_OnCooldown;
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

	public Text GetScore()
	{
		return ScoreText;
	}

    private void OnDestroy()
    {
        playerStats.OnHitPointsChanged -= ActorStats_OnHitPointsChanged;
        playerStats.OnMaxHitPointsChanged -= ActorStats_OnMaxHitPointsChanged;
        playerStats.OnMaxHitPointsChanged -= ActorStats_ChangeMaxHPStatOnScreen;
        playerStats.OnLevelChanged -= ActorStats_OnOnLevelChanged;
        playerStats.OnExpChanged -= ActorStats_OnExpChanged;
        playerStats.OnDamageModifierChanged -= ActorStats_ChangeDMGModStatsOnScreen;
        playerStats.OnDamageReductionChanged -= ActorStats_ChangeDMGRedStatsOnScreen;
        playerStats.OnCooldownReductionChanged -= ActorStats_ChangeCooldownReductionStatsOnScreen;

        for (int i = 0; i < 4; i++)
        {
            (player as Player).Skills[i].GetComponent<Skill>().OnReady -= Skill_OnReady;
            (player as Player).Skills[i].GetComponent<Skill>().OnUse -= Skill_OnUse;
            (player as Player).Skills[i].GetComponent<Skill>().OnCooldown -= Skill_OnCooldown;
        }
    }

    private long ExpPerLevel(int level)
    {
        return (level + 1) * 100;
    }

    private void ActorStats_ChangeCooldownReductionStatsOnScreen(float modifier)
    {
        CooldownReductionStatsOnScreen.text = "CD Reduction: " + modifier;
    }

    private void ActorStats_ChangeDMGRedStatsOnScreen(float modifier)
    {
        DMGRedStatsOnScreen.text = "DMG Reduction: " + modifier;
    }

    private void ActorStats_ChangeDMGModStatsOnScreen(float modifier)
    {
        DMGModStatsOnScreen.text = "DMG Modifier: " + modifier;
    }

    private void ActorStats_ChangeMaxHPStatOnScreen(float HP)
    {
        MaxHPStatsOnScreen.text = "Max HP: " + HP;
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
        slots[index].GetComponentInChildren<Image>().color = new Color(1f, 1f, 1f, 1f);
    }

    private void Skill_OnUse(int index, Texture2D image)
    {
        Sprite old = slots[index].GetComponentInChildren<Image>().sprite;
        slots[index].GetComponentInChildren<Image>().sprite = Sprite.Create(image, old.rect, old.pivot);
        slots[index].GetComponentInChildren<Image>().color = new Color(1f, 1f, 1f, 0.75f);
    }

    private void Skill_OnCooldown(int index, Texture2D image)
    {
        Sprite old = slots[index].GetComponentInChildren<Image>().sprite;
        slots[index].GetComponentInChildren<Image>().sprite = Sprite.Create(image, old.rect, old.pivot);
        slots[index].GetComponentInChildren<Image>().color = new Color(1f, 1f, 1f, 0.25f);
    }
}