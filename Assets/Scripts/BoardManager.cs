using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Amount
    {
        public int Min;
        public int Max;

        public Amount(int min, int max)
        {
            Min = min;
            Max = max;
        }
    }

    [Serializable]
    public class SpritePool
    {
        public GameObject[] sprites;

        public int Length
        {
            get { return sprites.Length; }
        }

        public SpritePool(GameObject[] sprites)
        {
            this.sprites = sprites;
        }

        public GameObject Get(int index)
        {
            return sprites[index];
        }

        public GameObject GetRandomSprite()
        {
            return sprites[Random.Range(0, sprites.Length)];
        }
    }

    public int Cols = 64;
    public int Rows = 64;
    public Amount WallsAmount = new Amount(16, 32);

    public SpritePool Floor;
    public SpritePool Walls;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    void Init()
    {
        gridPositions.Clear();

        for (var i = 1; i < Cols - 1; ++i)
        {
            for (var j = 1; j < Rows - 1; ++j)
            {
                gridPositions.Add(new Vector3(i, j, 0.0f));
            }
        }

        boardHolder = new GameObject("Board").transform;

        for (var i = 0; i < Cols; ++i)
        {
            for (var j = 0; j < Rows; ++j)
            {
                GameObject sprite = Floor.GetRandomSprite();
                if (i == 0 || i == Cols - 1 || j == 0 || j == Rows - 1)
                {
                    sprite = Walls.GetRandomSprite();
                }

                GameObject instance = Instantiate(sprite, new Vector3(i, j, 0.0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    void LayoutSprites(SpritePool sprites, Amount amount)
    {
        for (var i = 0; i < Random.Range(amount.Min, amount.Max + 1); ++i)
        {
            int j = Random.Range(0, gridPositions.Count);
            Vector3 randomPos = gridPositions[j];
            gridPositions.RemoveAt(j);

            GameObject sprite = sprites.GetRandomSprite();
            Instantiate(sprite, randomPos, Quaternion.identity);
        }
    }

    public void SetupScene () {
        Init();
        LayoutSprites(Walls, WallsAmount);
    }
}
