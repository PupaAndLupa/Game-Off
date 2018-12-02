using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public AudioSource FxSource;
    public AudioSource MusicSource;
    public static SoundManager Instance = null;

    public float LowPitchRange = 0.95f;
    public float HighPitchRange = 1.05f;

	void Awake() {
        if (Instance == null)
        {
            Instance = this;
        } else if (Instance != this) {
            Destroy(gameObject);
        }
	}

    public void PlayOnce(AudioClip clip)
    {
        FxSource.clip = clip;
        FxSource.Play();
    }

    public void RandomizeFx(params AudioClip[] clips)
    {
        FxSource.pitch = Random.Range(LowPitchRange, HighPitchRange);
        PlayOnce(clips[Random.Range(0, clips.Length)]);
    }
}
