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

    public virtual void Move(float movespeed) {}
    public virtual void Rotate() {}
    public virtual void LookTowards() {} 

    protected bool Enabled;
}
