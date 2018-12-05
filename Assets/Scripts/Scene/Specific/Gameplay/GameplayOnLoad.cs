using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayOnLoad : MonoBehaviour {

    public GameObject PlayerPrefab;
    public GameObject EnemyPrefab;  // TEMP
    public GameObject PauseMenuPrefab;

    void Start () {
        GameManager gm = FindObjectOfType<GameManager>();
        gm.SetPlayer(PlayerPrefab);
        gm.SetEnemy(EnemyPrefab);   // TEMP
        gm.SetPauseMenu(PauseMenuPrefab);
        gm.InitGame();

        SoundManager.Instance.MusicSource.Stop();
        SoundManager.Instance.MusicSource.clip = Resources.Load("track_faithyorfaithless_loop") as AudioClip;
        SoundManager.Instance.MusicSource.volume = 0.1f;
        SoundManager.Instance.MusicSource.Play();
    }
}
