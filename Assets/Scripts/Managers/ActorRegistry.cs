using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorRegistry : MonoBehaviour
{
    List<Actor> Actors { get; set; }

    public void AddActor(Actor actor)
    {
        Actors.Add(actor);
    }

    void Start()
    {
        Actors = new List<Actor>();
    }

    void Update()
    {
        foreach(var actor in Actors)
        {
            if (actor.IsDead)
            {
                actor.Die();
                Actors.Remove(actor);
            }
        }
    }
}
