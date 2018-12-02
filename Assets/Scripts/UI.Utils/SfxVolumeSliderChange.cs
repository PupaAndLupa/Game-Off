using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SfxVolumeSliderChange : MonoBehaviour {
    public Slider Slider;

    public void OnChange()
    {
        SoundManager.Instance.FxSource.volume = Slider.value;
    }
}
