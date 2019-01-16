using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuOnLoad : MonoBehaviour {

    public AudioClip BackgroundMusic;

    public Texture2D cursorImage;

	void Start () {
        SoundManager.Instance.FxSource.Stop();
        SoundManager.Instance.MusicSource.Stop();
        SoundManager.Instance.MusicSource.clip = BackgroundMusic;
        SoundManager.Instance.MusicSource.Play();

        Cursor.SetCursor(cursorImage, Vector2.zero, CursorMode.Auto);
    }

    void Update () {
		
	}
}
