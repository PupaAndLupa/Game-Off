using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement
{
    protected bool Enabled;
    protected Vector2 Direction;

    public Movement()
    {
        Enabled = true;
    }

    public void Enable()
    {
        Enabled = true;
    }

    public void Disable()
    {
        Enabled = false;
    }

    private bool IsEnabled()
    {
        return Enabled;
    }

    public virtual void Move(GameObject obj, float movespeed)
    {
        if (IsEnabled())
        {
            obj.GetComponent<Rigidbody2D>().velocity = Direction * movespeed * Time.deltaTime;
        }
    }

    public virtual void SetDirection(Vector2 direction)
    {
        Direction = direction.normalized;
    }
}
