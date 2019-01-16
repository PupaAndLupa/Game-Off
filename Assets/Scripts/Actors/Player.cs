using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    public override void Die()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Animator>().enabled = false;

        transform.Find("Camera").localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
        
        IsTotallyDead = true;
    }
}
