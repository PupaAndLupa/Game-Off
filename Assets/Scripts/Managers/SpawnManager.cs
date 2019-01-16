using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] Enemies;

    public void Init()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector2 point = FindObjectOfType<BoardManager>().RandomFreePosition();
            GameObject obj = Instantiate(Enemies[Random.Range(0, Enemies.Length)], point, Quaternion.identity);
            FindObjectOfType<ActorRegistry>().AddActor(obj.GetComponent<Actor>());
        }
    }
}
