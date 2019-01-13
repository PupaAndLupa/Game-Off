using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ProjectileStats : Stats
{
    public float Movespeed;
    public float Damage { get; set; }
    public float Range { get; set; }

    public ProjectileStats(float movespeed)
    {
        Movespeed = movespeed;
    }
}
