using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorRegistry : MonoBehaviour
{
    public List<GameObject> Actors { get; set; }
    public List<GameObject> Corpses { get; set; }
    public Actor Player { get; set; }

    public GameObject coin;

    public int level { get; set; }

    public void AddActor(GameObject actor)
    {
        Actors.Add(actor);
    }

    public void SetPlayer(Actor actor)
    {
        Player = actor;
    }

    void Start()
    {
        Actors = new List<GameObject>();
        Corpses = new List<GameObject>();
        Player = null;
        level = 0;
    }

    void Update()
    {
        if (Player != null)
        {
            if (Player.IsDead && FindObjectOfType<GameManager>().CurrentState != GameManager.GameStates.End)
            {
                FindObjectOfType<GameManager>().FinishGame(GameManager.GameStates.Dead);
            }

            foreach (var actor in Actors.ToArray())
            {
                if (actor.GetComponent<Actor>().IsDead)
                {
                    long score = actor.GetComponent<Actor>().Stats.Experience + 
                        Mathf.RoundToInt(Random.Range(-actor.GetComponent<Actor>().Stats.Experience * 
                        0.1f, actor.GetComponent<Actor>().Stats.Experience * 0.1f));
                    Player.Stats.Experience += score;
                    FindObjectOfType<UIManager>().AddScore(score);

                    if (Random.Range(1, 5) > 3)
                    {
                        Instantiate(coin, actor.transform.position, Quaternion.Euler(Vector3.zero), null);
                    }

                    actor.GetComponent<Actor>().Die();
                    Actors.Remove(actor);
                    Corpses.Add(actor);
                    Actors.Add(FindObjectOfType<SpawnManager>().SpawnRandomEnemy(level));
                }
            }

            if (Actors.Count == 0 && FindObjectOfType<GameManager>().CurrentState != GameManager.GameStates.End)
            {
                FindObjectOfType<GameManager>().FinishGame(GameManager.GameStates.Win);
            }
        }
    }

    public void GetCoin()
    {
        long score = 100;
        Player.Stats.Experience += score;
        var uiManager = FindObjectOfType<UIManager>();
        uiManager.AddScore(score);
        uiManager.AddCoins(1);
    }
}
