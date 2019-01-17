using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballSkill : Skill
{
    public GameObject fireball;
    public float damage;

    public override bool Use()
    {
        if (base.Use())
        {
            Vector3 rotation = player.transform.rotation.eulerAngles;
            Vector3 position = player.transform.Find("Weapon").position + player.transform.Find("Weapon").right * 0.3f;

            GameObject projectile = Instantiate(
            fireball,
            position,
            Quaternion.Euler(rotation.x, rotation.y, rotation.z - 90),
            null
            );
            Projectile projectileClass = projectile.GetComponent<Projectile>();
            projectileClass.Movement.SetDirection(player.transform.Find("Weapon").right);
            projectileClass.SetDamage(damage * player.GetComponentInChildren<Weapon>().Stats.Modifier);
            projectileClass.SetRange(player.GetComponentInChildren<Weapon>().Stats.Range);
            projectileClass.SetParentTag(player.GetComponentInChildren<Weapon>().parentTag);
            return true;
        }
        return false;
    }
}
