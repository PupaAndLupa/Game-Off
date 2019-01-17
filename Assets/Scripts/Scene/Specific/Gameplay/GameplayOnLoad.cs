using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayOnLoad : MonoBehaviour {

    public GameObject PlayerPrefab;
    public GameObject EnemyPrefab;
    public GameObject PauseMenuPrefab;
    public GameObject StatsUpgraderPrefab;

    public AudioClip BackgroundMusic;

    public Texture2D cursorImage;

    void Start () {
        GameManager gm = FindObjectOfType<GameManager>();
        gm.SetEnemy(EnemyPrefab);
        gm.SetPauseMenu(PauseMenuPrefab);
        gm.InitGame();

        SoundManager.Instance.MusicSource.Stop();
        SoundManager.Instance.MusicSource.clip = BackgroundMusic;
        SoundManager.Instance.MusicSource.volume = 0.2f;
        SoundManager.Instance.MusicSource.Play();

        Cursor.SetCursor(cursorImage, new Vector2(cursorImage.width / 2, cursorImage.height / 2), CursorMode.Auto);

        Instantiate(StatsUpgraderPrefab).GetComponent<StatsUpgrader>().Actor = gm.Player;
    }
}
