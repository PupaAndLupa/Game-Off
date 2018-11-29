using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public BoardManager BoardManager;
    public GameObject playerPrefab;

	void Start () {
        BoardManager = GetComponent<BoardManager>();
        InitGame();
	}
	
    void InitGame()
    {
        BoardManager.SetupScene();
        Instantiate(playerPrefab, BoardManager.GetCenter(), Quaternion.identity);
    }

	void Update () {
		
	}
}
