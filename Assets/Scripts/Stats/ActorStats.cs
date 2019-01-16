using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ActorStats: Stats
{
    public bool IsInvisible;
    public float DetectionRadius;

    public event Action<float> OnMaxHitPointsChanged;
    public event Action<int> OnLevelChanged;
    public event Action<long> OnExpChanged;

    private float maxHealth;
    public float MaxHealth
    {
        get { return maxHealth; }
        set
        {
            maxHealth = value;

            if (OnMaxHitPointsChanged != null)
            {
                OnMaxHitPointsChanged(value);
            }
        }
    }

    public event Action<float> OnHitPointsChanged;
    private float currentHealth;
    public float CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            currentHealth = value;

            if (OnHitPointsChanged != null)
            {
                OnHitPointsChanged(value);
            }
        }
    }

    private int level = 1;
    public int Level
    {
        get { return level; }
        set
        {
            level = value;

            if (OnLevelChanged != null)
            {
                OnLevelChanged(value);
            }
        }
    }

    public long experience = 0;
    public long Experience
    {
        get { return experience; }
        set
        {
            experience = value;

            if (OnExpChanged != null)
            {
                OnExpChanged(value);
            }
        }
    }

    public event Action<float> OnMovespeedChanged;
    private float movespeed;
    public float Movespeed
    {
        get { return movespeed; }
        set
        {
            movespeed = value;

            if (OnMovespeedChanged != null)
            {
                OnMovespeedChanged(value);
            }
        }
    }

    public event Action<float> OnDamageModifierChanged;
    private float damageModifier;
    public float DamageModifier
    {
        get { return damageModifier; }
        set
        {
            damageModifier = value;

            if (OnDamageModifierChanged != null)
            {
                OnDamageModifierChanged(value);
            }
        }
    }

    public event Action<float> OnDamageReductionChanged;
    private float damageReduction;
    public float DamageReduction
    {
        get { return damageReduction; }
        set
        {
            damageReduction = value;

            if (OnDamageReductionChanged != null)
            {
                OnDamageReductionChanged(value);
            }
        }
    }

    public event Action<float> OnCooldownReductionChanged;
    private float cooldownReduction;
    public float CooldownReduction
    {
        get { return cooldownReduction; }
        set
        {
            cooldownReduction = value;

            if (OnCooldownReductionChanged != null)
            {
                OnCooldownReductionChanged(value);
            }
        }
    }

    public ActorStats(int maxHealth=100, float movespeed=300, float damageModifier=1f, 
        float damageReduction=1f, float detectionRadius=5f, float cooldownReduction=1f)
    {
        Movespeed = movespeed;
        MaxHealth = maxHealth;
        CurrentHealth = MaxHealth;
        DamageModifier = damageModifier;
        DamageReduction = damageReduction;
        DetectionRadius = detectionRadius;
        CooldownReduction = cooldownReduction;
    }
}
