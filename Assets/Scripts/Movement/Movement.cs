using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Movement()
    {
        Enabled = true;
    }

    public bool IsEnabled()
    {
        return Enabled;
    }

    public void Enable()
    {
        Enabled = true;
    }

    public void Disable()
    {
        Enabled = false;
    }

    public virtual void Move(float movespeed)
    {
        GetComponent<Rigidbody2D>().velocity = Direction * movespeed * Time.deltaTime;
    }

    public virtual void Rotate() {}
    public virtual void LookTowards() {} 
    public virtual void SetDirection(Vector3 direction)
    {
        Direction = direction.normalized;
    }

    protected bool Enabled;
    protected Vector3 Direction;
}
