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

        public WeaponStats(float damage)
        {
            Damage = damage;
        }
    }

    public GameObject ProjectilePrefab;
    public WeaponStats Stats = new WeaponStats(30f);

    public void Attack(Vector3 direction)
    {
        GameObject projectile = Instantiate(
            ProjectilePrefab, 
            transform.position, 
            transform.rotation,
            null
        );
        projectile.GetComponent<Projectile>().Movement.SetDirection(direction);
        projectile.GetComponent<Projectile>().SetDamage(Stats.Damage * Stats.Modifier);
    }

    public void SetModifier(float modifier)
    {
        Stats.Modifier = modifier;
    }
}
