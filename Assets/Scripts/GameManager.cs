using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public BoardManager BoardManager;
    public GameObject playerPrefab;

    public GameObject PausePanel;

	void Start ()
    {
        BoardManager = GetComponent<BoardManager>();
        PausePanel = GameObject.FindGameObjectWithTag("PauseMenuPanel");
        PausePanel.SetActive(false);
        InitGame();
	}
	
    void InitGame()
    {
        BoardManager.SetupScene();
        Instantiate(playerPrefab, BoardManager.StartPosition, Quaternion.identity);
    }

	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!PausePanel.activeInHierarchy)
            {
                PauseGame();
            } else if (PausePanel.activeInHierarchy)
            {
                ContinueGame();
            }
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        PausePanel.SetActive(true);
        //Disable scripts that still work while timescale is set to 0
    }
    private void ContinueGame()
    {
        Time.timeScale = 1;
        PausePanel.SetActive(false);
        //enable the scripts again
    }
}
