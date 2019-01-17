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

    [SerializeField] public float BaseCooldown;
    [SerializeField] public float BaseDuration;
    private float timer;

    public int Index { get; set; }

    protected GameObject player;


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

    public virtual void Update()
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
                if (Time.time - timer > BaseCooldown * (1f / player.GetComponent<Player>().Stats.CooldownReduction))
                {
                    setReady();
                }
                break;
        }
    }

    public virtual bool Use()
    {
        if (state == States.ready)
        {
            timer = Time.time;
            state = States.used;
            Debug.Log(Index);
            throwOnUse(Index, UsedImage);
            return true;
        }
        return false;
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

    public virtual void Set(int index, GameObject Player)
    {
        Index = index;
        throwOnReady(Index, ReadyImage);
        player = Player;
    }
}
