using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterVolumeSliderChange : MonoBehaviour {
    public Slider Slider;

    public void OnChange()
    {
        AudioListener.volume = Slider.value;
    }
}
