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
            FindObjectOfType<ActorRegistry>().AddActor(SpawnRandomEnemy(FindObjectOfType<ActorRegistry>().level));
        }
    }

    public GameObject SpawnRandomEnemy(int level=0)
    {
        Vector2 point;
        Vector3 position = FindObjectOfType<Player>().transform.position;
        do
        {
            point = FindObjectOfType<BoardManager>().RandomFreePosition();
        } while (new Vector2(position.x - point.x, position.y - point.y).magnitude < 5);

        GameObject enemy = Instantiate(Enemies[Random.Range(0, Enemies.Length)], point, Quaternion.identity);


        if (level != 0)
        {
            float[] values = new float[] { 50, 0.3f, 0.3f };
            for (int i = 0; i < level; i++)
            {
                int k = Random.Range(0, 3);
                enemy.GetComponent<Actor>().UpgradeStats((ActorStats.UpgradableStats)k, values[k]);
            }
        }

        return enemy;
    }
}
