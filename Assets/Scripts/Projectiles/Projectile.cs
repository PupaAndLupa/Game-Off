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
        public float Range { get; set; }

        public ProjectileStats(float movespeed)
        {
            Movespeed = movespeed;
        }
    }

    public ProjectileStats Stats = new ProjectileStats(700f);
    public Movement Movement = new Movement();

    protected Vector3 startPoint;


    public void SetDamage(float damage)
    {
        Stats.Damage = damage;
    }

    public void SetRange(float range)
    {
        Stats.Range = range;
    }

    protected float distanceTraveled()
    {
        return (transform.position - startPoint).magnitude;
    }

    public void Move()
    {
        Movement.Move(gameObject, Stats.Movespeed);
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
            if (collision.gameObject.tag != "Projectile")
            {
                Destroy(gameObject);
            }
    }

    private void Start()
    {
        startPoint = transform.position;
    }

    private void FixedUpdate()
    {
        if (distanceTraveled() > Stats.Range)
        {
            Destroy(gameObject);
        }
        Move();
    }
}
