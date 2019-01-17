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
            FindObjectOfType<ActorRegistry>().AddActor(SpawnRandomEnemy());
        }
    }

    public GameObject SpawnRandomEnemy()
    {
        Vector2 point;
        Vector3 position = FindObjectOfType<Player>().transform.position;
        do
        {
            point = FindObjectOfType<BoardManager>().RandomFreePosition();
        } while (new Vector2(position.x - point.x, position.y - point.y).magnitude < 5);

        return Instantiate(Enemies[Random.Range(0, Enemies.Length)], point, Quaternion.identity);
    }
}
