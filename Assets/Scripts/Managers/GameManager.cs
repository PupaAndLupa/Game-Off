using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public enum GameStates
    {
        MainMenu,
        Playing,
        Pause,
        Dead,
        Win,
        End
    }

    [Tooltip("Initialized on runtime")]
    public BoardManager BoardManager;

    [Tooltip("Initialized on runtime")]
    public SoundManager SoundManager;

    public GameObject PauseMenu { get; set; }
    public GameObject PlayerPrefab { get; set; }
    public GameObject EnemyPrefab { get; set; } // TEMP
    public GameStates CurrentState { get; set; }
    private GameObject fadeScreen { get; set; }
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
                                SoundManager.Instance.PlayOnce(Player.Sounds.Walking);
                            }
                        }

                        Player.Move(direction);

                        if (Input.GetMouseButton(0))
                        {
                            Player.Attack();
                        }

                        if (Input.GetKeyDown(KeyCode.Q))
                        {
                            (Player as Player).ChangeWeapon();
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

    public float timer { get; set; }
	public Text Score { get; set; }

    void Update()
    {
        if (CurrentState == GameStates.End && Time.time - timer > 2f)
        {
			Score = FindObjectOfType<UIManager>().GetScore();
            SceneManager.LoadScene(3);
        }
    }

    public void InitGame()
    {
		BoardManager.Board.SetBoardHolder(BoardManager.BoardHolder);
		BoardManager.Board.LoadLevel("First");
        BoardManager.Board.Generate();
        Player = Instantiate(PlayerPrefab, BoardManager.Board.StartPosition, Quaternion.identity).GetComponent<Actor>();
        FindObjectOfType<ActorRegistry>().SetPlayer(Player.GetComponent<Actor>());
        FindObjectOfType<SpawnManager>().Init();
    }

    public void FinishGame(GameStates state)
    {
        switch (state)
        {
            case GameStates.Win:
                break;
            case GameStates.Dead:
                Player.Die();
                break;
        }
        CurrentState = GameStates.End;
        timer = Time.time;
    }

    public void SetPlayer(GameObject player)
    {
        PlayerPrefab = player;
    }

    public void SetEnemy(GameObject enemy)
    {                                          
        EnemyPrefab = enemy;               
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
