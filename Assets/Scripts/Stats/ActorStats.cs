using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ActorStats: Stats
{
    public event Action<float> OnHitPointsChanged;
    public event Action<float> OnMaxHitPointsChanged;

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

    public bool IsInvisible;
    public float Movespeed;
    public float DamageModifier;
    public float DetectionRadius;
    public float DamageReduction;

    public ActorStats(int maxHealth, float movespeed, float damageModifier, float damageReduction, float detectionRadius)
    {
        Movespeed = movespeed;
        MaxHealth = maxHealth;
        CurrentHealth = MaxHealth;
        DamageModifier = damageModifier;
        DamageReduction = damageReduction;
        DetectionRadius = detectionRadius;
    }
}
