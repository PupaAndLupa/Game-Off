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

    [Serializable]
    public struct SoundStruct
    {
        public AudioClip Shot;
    }


    public SoundStruct Sounds;

    public GameObject ProjectilePrefab;
    public WeaponStats Stats = new WeaponStats(30f, 0.5f, 3f);
    public bool OnCooldown { get; set; }
    public float AttackTime { get; set; }

    private string parentTag;

    protected virtual void Start()
    {
        OnCooldown = false;
    }

    protected virtual void Update()
    {
        if (OnCooldown)
        {
            if (Time.time - AttackTime >= Stats.Cooldown)
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
            Vector3 position = transform.Find("Weapon").position + transform.Find("Weapon").right * 0.3f;

            GameObject projectile = Instantiate(
                ProjectilePrefab,
                position,
                Quaternion.Euler(rotation.x, rotation.y, rotation.z - 90),
                null
            );

            Projectile projectileClass = projectile.GetComponent<Projectile>();
            projectileClass.Movement.SetDirection(transform.right);
            projectileClass.SetDamage(Stats.Damage * Stats.Modifier);
            projectileClass.SetRange(Stats.Range);
            projectileClass.SetParentTag(parentTag);
            OnCooldown = true;
            AttackTime = Time.time;
            GetComponent<Animator>().SetTrigger("Attack");
            FindObjectOfType<SoundManager>().PlayOnce(Sounds.Shot);
        }
    }


    public void Rotate(float angle)
    {
        if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            GetComponent<Animator>().SetFloat("Rotation", angle);
        }
    }

    public void SetModifier(float modifier)
    {
        Stats.Modifier = modifier;
    }

    public void SetParentTag(string tag)
    {
        parentTag = tag;
    }
}
