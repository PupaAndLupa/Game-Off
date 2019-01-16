﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Serializable]
    public struct SoundStruct
    {
        public AudioClip Hit;
    }


    public SoundStruct Sounds;

    public ProjectileStats Stats = new ProjectileStats(700f);
    public Movement Movement = new Movement();

    public string parentTag { get; set; }

    protected Vector3 startPoint;

    private bool isDead;


    public void SetDamage(float damage)
    {
        Stats.Damage = damage;
    }

    public void SetRange(float range)
    {
        Stats.Range = range;
    }

    public void SetParentTag(string tag)
    {
        parentTag = tag;
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
        if (collision.gameObject.tag != "Projectile" && collision.gameObject.tag != parentTag)
        {
            FindObjectOfType<SoundManager>().PlayOnce(Sounds.Hit);
            GetComponent<Animator>().SetBool("Hit", true);
            GetComponent<Collider2D>().enabled = false;
            Stats.Movespeed = 0;
            isDead = true;
        }
    }

    private void Start()
    {
        startPoint = transform.position;
        isDead = false;
    }

    private void FixedUpdate()
    {
        if (isDead && GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            Destroy(gameObject);
        }


        if (distanceTraveled() > Stats.Range)
        {
            Destroy(gameObject);
        }
        Move();
    }
}
