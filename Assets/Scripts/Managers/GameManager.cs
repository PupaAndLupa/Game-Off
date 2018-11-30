using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public enum GameStates
    {
        MainMenu,
        Playing,
        Pause
    }

    [Tooltip("Initialized on runtime")]
    public BoardManager BoardManager;

    [Tooltip("Initialized on runtime")]
    public AudioManager AudioManager;

    [Tooltip("Initialized on runtime")]
    public ProjectileManager ProjectileManager;

    public GameObject Player { get; set; }
    public GameObject PauseMenu { get; set; }
    public GameStates CurrentState { get; set; }

    void Start ()
    {
        CurrentState = GameStates.MainMenu;
        BoardManager = FindObjectOfType<BoardManager>();
        AudioManager = FindObjectOfType<AudioManager>();
        ProjectileManager = FindObjectOfType<ProjectileManager>();
        StartCoroutine(HandleInput());
    }

    IEnumerator HandleInput()
    {
        while (true)
        {
            switch (CurrentState)
            {
                case GameStates.MainMenu:
                    break;
                case GameStates.Playing:
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        CurrentState = GameStates.Pause;
                        DisableCamera();
                        DisableMovement();
                        PauseGame();
                    }
                    break;
                case GameStates.Pause:
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        CurrentState = GameStates.Playing;
                        EnableCamera();
                        EnableMovement();
                        ContinueGame();
                    }
                    break;
            }
            yield return null;
        }

    }

    void Update()
    {
        
    }

    public void InitGame()
    {
        BoardManager.SetupScene();
        Instantiate(Player, BoardManager.StartPosition, Quaternion.identity);
    }

    public void SetPlayer(GameObject player)
    {
        Player = player;
    }

    public void SetPauseMenu(GameObject pausePanel)
    {
        PauseMenu = pausePanel;
        PauseMenu.SetActive(false);
    }

    public void DisableMovement()
    {

    }

    public void EnableMovement()
    {

    }

    public void DisableCamera()
    {

    }

    public void EnableCamera()
    {

    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        if (PauseMenu != null)
        {
            PauseMenu.SetActive(true);
        }
    }

    private void ContinueGame()
    {
        Time.timeScale = 1;
        if (PauseMenu != null)
        {
            PauseMenu.SetActive(false);
        }
    }
}
