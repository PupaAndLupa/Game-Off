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
    public SoundManager SoundManager;

    public GameObject PauseMenu { get; set; }
    public GameObject PlayerPrefab { get; set; }
    public GameStates CurrentState { get; set; }

    public Actor Player { get; set; }

    void Start ()
    {
        CurrentState = GameStates.MainMenu;
        BoardManager = FindObjectOfType<BoardManager>();
        SoundManager = FindObjectOfType<SoundManager>();
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
                        DisableMovement();
                        PauseGame();
                    } else {
                        Vector2 direction = new Vector3(0, 0);
                        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        mousePosition.z = 0f;

                        Player.LookTowards(mousePosition);
                        Camera.main.transform.rotation = Quaternion.Euler(0, 0, 0);
                        if (Input.GetKey(KeyCode.W))
                        {
                            ++direction.y;
                        }
                        if (Input.GetKey(KeyCode.S))
                        {
                            --direction.y;
                        }
                        if (Input.GetKey(KeyCode.A))
                        {
                            --direction.x;
                        }
                        if (Input.GetKey(KeyCode.D))
                        {
                            ++direction.x;
                        }

                        if (direction != Vector2.zero)
                        {
                            if (!SoundManager.Instance.FxSource.isPlaying)
                            {
                                SoundManager.Instance.PlayOnce(Player.WalkingSound);
                            }
                        }

                        Player.Move(direction);

                        if (Input.GetMouseButtonDown(0))
                        {
                            Player.Attack(mousePosition - Player.transform.position);
                        }
                    }
                    break;
                case GameStates.Pause:
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
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
        Player = Instantiate(PlayerPrefab, BoardManager.StartPosition, Quaternion.identity).GetComponent<Actor>();
    }

    public void SetPlayer(GameObject player)
    {
        PlayerPrefab = player;
    }

    public void SetPauseMenu(GameObject pausePanel)
    {
        PauseMenu = pausePanel;
        PauseMenu.SetActive(false);
    }

    public void DisableMovement()
    {
        Player.Movement.Disable();
    }

    public void EnableMovement()
    {
        Player.Movement.Enable();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        CurrentState = GameStates.Pause;
        if (PauseMenu != null)
        {
            PauseMenu.SetActive(true);
        }
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        CurrentState = GameStates.Playing;
        if (PauseMenu != null)
        {
            PauseMenu.SetActive(false);
        }
    }
}
