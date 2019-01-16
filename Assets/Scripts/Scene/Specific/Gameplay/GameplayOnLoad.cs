using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayOnLoad : MonoBehaviour {

    public GameObject PlayerPrefab;
    public GameObject EnemyPrefab;  // TEMP
    public GameObject PauseMenuPrefab;

    public AudioClip BackgroundMusic;

    void Start () {
        GameManager gm = FindObjectOfType<GameManager>();
        gm.SetPlayer(PlayerPrefab);
        gm.SetEnemy(EnemyPrefab);
        gm.SetPauseMenu(PauseMenuPrefab);
        gm.InitGame();

        SoundManager.Instance.MusicSource.Stop();
        SoundManager.Instance.MusicSource.clip = BackgroundMusic;
        SoundManager.Instance.MusicSource.volume = 0.2f;
        SoundManager.Instance.MusicSource.Play();
    }
}
