using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public BoardManager BoardManager;

	void Start () {
        BoardManager = GetComponent<BoardManager>();
        InitGame();
	}
	
    void InitGame()
    {
        BoardManager.SetupScene();
    }

	void Update () {
		
	}
}
