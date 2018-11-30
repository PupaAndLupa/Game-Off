using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Serializable]
    public class ProjectileStats
    {
        public float Movespeed;
        public float Damage { get; set; }

        public ProjectileStats(float movespeed)
        {
            Movespeed = movespeed;
        }
    }

    public Movement Movement;
    public ProjectileStats Stats = new ProjectileStats(1000f);

    public void SetDamage(float damage)
    {
        Stats.Damage = damage;
    }
}
