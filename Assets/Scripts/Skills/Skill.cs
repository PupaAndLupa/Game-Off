using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public event Action<int, Texture2D> OnReady;
    protected void throwOnReady(int index, Texture2D image)
    {
        OnReady(index, image);
    }

    public event Action<int, Texture2D> OnUse;
    protected void throwOnUse(int index, Texture2D image)
    {
        OnUse(index, image);
    }

    public event Action<int, Texture2D> OnCooldown;
    protected void throwOnCooldown(int index, Texture2D image)
    {
        OnCooldown(index, image);
    }

    public Texture2D ReadyImage;
    public Texture2D UsedImage;
    public Texture2D CooldownImage;

    public float BaseCooldown;
    public float BaseDuration;
    private float cooldownReduction;
    private float timer;

    public int Index { get; set; }


    protected enum States
    {
        ready,
        used,
        cooldown
    }
    protected States state;

    protected virtual void Start()
    {
        state = States.ready;
    }

    protected virtual void Update()
    {
        switch (state)
        {
            case States.ready:
                break;
            case States.used:
                if (Time.time - timer > BaseDuration)
                {
                    setCooldown();
                }
                break;
            case States.cooldown:
                if (Time.time - timer > BaseCooldown * (1f / cooldownReduction))
                {
                    setReady();
                }
                break;
        }
    }

    public virtual void Use(float CooldownReduction)
    {
        if (state == States.ready)
        {
            cooldownReduction = CooldownReduction;

            timer = Time.time;
            state = States.used;
            throwOnUse(Index, UsedImage);
        }
    }

    protected virtual void setCooldown()
    {
        timer = Time.time;
        state = States.cooldown;
        throwOnCooldown(Index, CooldownImage);
    }

    protected virtual void setReady()
    {
        state = States.ready;
        throwOnReady(Index, CooldownImage);
    }

    protected virtual void Set(int index)
    {
        Index = index;
        throwOnReady(Index, ReadyImage);
    }
}
