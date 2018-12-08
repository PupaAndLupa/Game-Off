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
            Vector3 rotation = transform.rotation.eulerAngles;

            GameObject projectile = Instantiate(
                ProjectilePrefab,
                transform.Find("Gun barrel").position,
                Quaternion.Euler(rotation.x, rotation.y, rotation.z - 90),
                null
            );
            projectile.GetComponent<Projectile>().Movement.SetDirection(direction);
            projectile.GetComponent<Projectile>().SetDamage(Stats.Damage * Stats.Modifier);
            projectile.GetComponent<Projectile>().SetRange(Stats.Range);
            OnCooldown = true;
            ShotTime = Time.time;
            GetComponent<Animator>().SetTrigger("Shot");
        }
    }

    public void SetModifier(float modifier)
    {
        Stats.Modifier = modifier;
    }

    public Vector3 GetGunBarrel()
    {
        return transform.Find("Gun barrel").transform.position;
    }
}
