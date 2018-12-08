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

    public void Attack()
    {
        if (!OnCooldown)
        {
            Vector3 rotation = transform.rotation.eulerAngles;

            GameObject projectile = Instantiate(
                ProjectilePrefab,
                transform.position + transform.right * 0.3f,
                Quaternion.Euler(rotation.x, rotation.y, rotation.z - 90),
                null
            );

            Projectile projectileClass = projectile.GetComponent<Projectile>();
            projectileClass.Movement.SetDirection(transform.right);
            projectileClass.SetDamage(Stats.Damage * Stats.Modifier);
            projectileClass.SetRange(Stats.Range);
            OnCooldown = true;
            ShotTime = Time.time;
            GetComponent<Animator>().SetTrigger("Shot");
        }
    }

    public void SetModifier(float modifier)
    {
        Stats.Modifier = modifier;
    }
}
