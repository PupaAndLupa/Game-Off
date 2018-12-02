using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeSliderChange : MonoBehaviour {
    public Slider Slider;

    public void OnChange()
    {
        SoundManager.Instance.MusicSource.volume = Slider.value;
    }
}
