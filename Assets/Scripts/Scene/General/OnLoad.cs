using System;
using UnityEngine;

public class OnLoad : MonoBehaviour {

    [Serializable]
    public class GameManagerSceneBinding
    {
        public float Timescale;
        public GameManager.GameStates GameState;

        public GameManagerSceneBinding(float timescale, GameManager.GameStates gameState)
        {
            Timescale = timescale;
            GameState = gameState;
        }
    }

    public GameManagerSceneBinding GameManagerOptions;

    public void Awake()
    {
        Time.timeScale = GameManagerOptions.Timescale;
        FindObjectOfType<GameManager>().CurrentState = GameManagerOptions.GameState;
    }
}
