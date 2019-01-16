using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorRegistry : MonoBehaviour
{
    public List<Actor> Actors { get; set; }
    public Actor Player { get; set; }

    public void AddActor(Actor actor)
    {
        Actors.Add(actor);
    }

    public void SetPlayer(Actor actor)
    {
        Player = actor;
    }

    void Start()
    {
        Actors = new List<Actor>();
        Player = null;
    }

    void Update()
    {
        if (Player != null)
        {
            if (Player.IsDead)
            {
                FindObjectOfType<GameManager>().FinishGame(GameManager.GameStates.Dead);
            }

            foreach (var actor in Actors.ToArray())
            {
                if (actor.IsDead)
                {
                    actor.Die();
                    Actors.Remove(actor);
                }
            }

            if (Actors.Count == 0)
            {
                FindObjectOfType<GameManager>().FinishGame(GameManager.GameStates.Win);
            }
        }
    }
}
