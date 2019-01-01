using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ActorStats
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

    public float Movespeed;
    public float DamageModifier;
    public float DetectionRadius;

    public ActorStats(int maxHealth, float movespeed, float damageModifier, float detectionRadius)
    {
        Movespeed = movespeed;
        MaxHealth = maxHealth;
        CurrentHealth = MaxHealth;
        DamageModifier = damageModifier;
        DetectionRadius = detectionRadius;
    }
}
