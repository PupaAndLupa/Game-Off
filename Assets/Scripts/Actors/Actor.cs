using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [Serializable]
    public class ActorStats
    {
        public int MaxHealth;
        public int CurrentHealth { get; set; }

        public float Movespeed;
        public float DamageModifier;
        
        public ActorStats(int maxHealth, float movespeed, float damageModifier)
        {
            MaxHealth = maxHealth;
            Movespeed = movespeed;
            DamageModifier = damageModifier;

            CurrentHealth = MaxHealth;
        }
    }

    public Movement Movement;
    public ActorStats Stats = new ActorStats(100, 500f, 1f);
}
