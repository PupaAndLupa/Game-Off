using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;  // TEMP

public class Actor : MonoBehaviour
{
    [Serializable]
    public struct SoundStruct
    {
        public AudioClip Walking;
    }

    public GameObject WeaponPrefab;

    public SoundStruct Sounds;
    public ActorStats Stats = new ActorStats(100, 500f, 1f, 5f);

    public Movement Movement = new Movement();
    public bool IsDead { get; set; }
    public float Rotation { get; set; }


    public bool IsTotallyDead { get; set; }

    protected virtual void Start()
    {
        WeaponPrefab.GetComponent<Weapon>().SetModifier(Stats.DamageModifier);
        WeaponPrefab.GetComponent<Weapon>().SetParentTag(tag);
        IsDead = false;
        IsTotallyDead = false;
    }

    protected virtual void Update()
    {

    }

    protected virtual void FixedUpdate()
    {
        GetComponent<Animator>().SetBool("isMoving", GetComponent<Rigidbody2D>().velocity != Vector2.zero);
    }

    public virtual void Move(Vector2 direction)
    {
        Movement.SetDirection(direction);
        Movement.Move(gameObject, Stats.Movespeed);
    }

    public virtual void Attack()
    {
        WeaponPrefab.GetComponent<Weapon>().Attack();
    }

    public virtual void Rotate(float angle)
    {
        Rotation = angle;
        WeaponPrefab.GetComponent<Weapon>().Rotate(angle);

        GetComponent<Animator>().SetFloat("Rotation", Rotation);

        bool flip = Mathf.Abs(Rotation) > 90;
        GetComponent<SpriteRenderer>().flipX = flip;
        WeaponPrefab.GetComponentInChildren<SpriteRenderer>().flipY = flip;
    }

    public virtual void LookTowards(Vector3 position)
    {
        Rotate(Mathf.Atan2(position.y - transform.position.y, position.x - transform.position.x) * Mathf.Rad2Deg);
    }

    public virtual bool CastRay(GameObject gameObject)
    {
        LayerMask mask = LayerMask.GetMask("Wall", "Player");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, gameObject.transform.position - transform.position, Stats.DetectionRadius, mask);
        if (!hit || hit.collider.gameObject.tag == "Wall")
            return false;
        return true;
    }

    public virtual Vector3 VectorTo(Vector3 position)
    {
        return position - transform.position;
    }

    public virtual Vector3 VectorTo(Vector3 position, Vector3 from)
    {
        return position - from;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            Projectile projectile = collision.gameObject.GetComponent<Projectile>();
            Stats.CurrentHealth -= projectile.Stats.Damage;
            if (Stats.CurrentHealth <= 0)
            {
                IsDead = true;
            }
        }
    }

    public virtual void Die()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Animator>().enabled = false;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
        IsTotallyDead = true;
    }
}
