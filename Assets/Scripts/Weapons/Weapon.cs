using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Serializable]
    public class WeaponStats
    {
        public float Damage;
        public float Modifier { get; set; }
        public float Cooldown;
        public float Range;

        public WeaponStats(float damage, float cooldown, float range)
        {
            Damage = damage;
            Cooldown = cooldown;
            Range = range;
        }
    }

    public GameObject ProjectilePrefab;
    public WeaponStats Stats = new WeaponStats(30f, 0.5f, 3f);
    public bool OnCooldown { get; set; }
    public float ShotTime { get; set; }

    protected virtual void Start()
    {
        OnCooldown = false;
    }

    protected virtual void Update()
    {
        if (OnCooldown)
        {
            if (Time.time - ShotTime >= Stats.Cooldown)
            {
                OnCooldown = false;
            }
        }
    }

    public void Attack(Vector3 direction)
    {
        if (!OnCooldown)
        {
            GameObject projectile = Instantiate(
                ProjectilePrefab,
                transform.position,
                transform.rotation,
                null
            );
            projectile.GetComponent<Projectile>().Movement.SetDirection(direction);
            projectile.GetComponent<Projectile>().SetDamage(Stats.Damage * Stats.Modifier);
            projectile.GetComponent<Projectile>().SetRange(Stats.Range);
            OnCooldown = true;
            ShotTime = Time.time;
        }
    }

    public void SetModifier(float modifier)
    {
        Stats.Modifier = modifier;
    }
}
