using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuOnLoad : MonoBehaviour {

	void Start () {
        SoundManager.Instance.FxSource.Stop();
        SoundManager.Instance.MusicSource.Stop();
        SoundManager.Instance.MusicSource.clip = Resources.Load("track_agreatwelcome_longloop") as AudioClip;
        SoundManager.Instance.MusicSource.Play();
    }

    void Update () {
		
	}
}
