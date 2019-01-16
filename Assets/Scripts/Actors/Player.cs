using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    public GameObject[] Weapons;
    private int currentWeaponIndex;
    
    protected override void Start()
    {
        IsDead = false;
        IsTotallyDead = false;
        damaged = false;

        currentWeaponIndex = 0;
        showWeapon(0);

        for (int i = 0; i <= Weapons.Length; i++)
        {
            Weapons[i].GetComponent<Weapon>().SetModifier(Stats.DamageModifier);
            Weapons[i].GetComponent<Weapon>().SetParentTag(tag);
        }

        for (int i = 1; i <= Weapons.Length; i++)
        {
            hideWeapon(i);
        }
    }

    public void ChangeWeapon()
    {
        hideWeapon(currentWeaponIndex);
        currentWeaponIndex = (currentWeaponIndex + 1) % Weapons.Length;
        showWeapon(currentWeaponIndex);
    }

    public override void Attack()
    {
        Weapons[currentWeaponIndex].GetComponent<Weapon>().Attack();
    }

    public override void Rotate(float angle)
    {
        Rotation = angle;
        Weapons[currentWeaponIndex].GetComponent<Weapon>().Rotate(angle);

        GetComponent<Animator>().SetFloat("Rotation", Rotation);

        bool flip = Mathf.Abs(Rotation) > 90;
        GetComponent<SpriteRenderer>().flipX = flip;
        Weapons[currentWeaponIndex].GetComponentInChildren<SpriteRenderer>().flipY = flip;
    }


    public override void Die()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Animator>().enabled = false;

        transform.Find("Camera").localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
        
        IsTotallyDead = true;
    }

    private void hideWeapon(int index)
    {
        Weapons[index].GetComponent<Animator>().enabled = false;
        Weapons[index].GetComponent<Weapon>().enabled = false;
        Weapons[index].GetComponentInChildren<SpriteRenderer>().enabled = false;
    }

    private void showWeapon(int index)
    {
        Weapons[index].GetComponent<Animator>().enabled = true;
        Weapons[index].GetComponent<Weapon>().enabled = true;
        Weapons[index].GetComponentInChildren<SpriteRenderer>().enabled = true;
    }
}
