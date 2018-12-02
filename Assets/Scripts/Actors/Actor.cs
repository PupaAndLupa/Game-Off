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
            Movespeed = movespeed;
            MaxHealth = maxHealth;
            CurrentHealth = MaxHealth;
            DamageModifier = damageModifier;
        }
    }

    public GameObject WeaponPrefab;
    public ActorStats Stats = new ActorStats(100, 500f, 1f);
    public Movement Movement = new Movement();

    void Start()
    {
        WeaponPrefab.GetComponent<Weapon>().SetModifier(Stats.DamageModifier);
    }

    public virtual void Move(Vector2 direction)
    {
        Movement.SetDirection(direction);
        Movement.Move(gameObject, Stats.Movespeed);
    }

    public virtual void Attack(Vector3 direction)
    {
        WeaponPrefab.GetComponent<Weapon>().Attack(direction);
    }

    public virtual void Rotate(float angle)
    {
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    public virtual void LookTowards(Vector3 position)
    {
        Rotate(Mathf.Atan2(position.y - transform.position.y, position.x - transform.position.x) * Mathf.Rad2Deg);
    }
}
