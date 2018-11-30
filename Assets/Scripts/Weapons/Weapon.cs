using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject projectile;

    [Serializable]
    public class WeaponStats
    {
        public float Damage;
        public float PlayerModifier { get; set; }

        public WeaponStats(float damage)
        {
            Damage = damage;
        }
    }

    public WeaponStats Stats = new WeaponStats(30f);

    public void Shoot(Vector3 direction)
    {
        GameObject bullet = Instantiate(projectile, transform.position, transform.rotation, transform.parent.parent);
        bullet.GetComponent<Projectile>().Movement.SetDirection(direction);
        bullet.GetComponent<Projectile>().SetDamage(Stats.Damage * Stats.PlayerModifier);
    }

    public void SetPlayerModifier(float playerModifier)
    {
        Stats.PlayerModifier = playerModifier;
    }
}
