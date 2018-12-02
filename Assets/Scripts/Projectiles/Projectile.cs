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

    public float LifeTime;
    public ProjectileStats Stats = new ProjectileStats(700f);
    public Movement Movement = new Movement();

    void Start()
    {
        Destroy(gameObject, LifeTime);
    }

    public void SetDamage(float damage)
    {
        Stats.Damage = damage;
    }

    public void Move()
    {
        Movement.Move(gameObject, Stats.Movespeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "projectile")
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        Move();
    }
}
