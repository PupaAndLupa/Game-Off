using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairScript : MonoBehaviour {

    public Texture2D crosshairTexture;
    public float scale = 0.5f;

    private Rect position;
    private bool isOn = true;

	// Use this for initialization
	void Start () {
        position = new Rect((Screen.width - crosshairTexture.width * scale) / 2, (Screen.height - 
            crosshairTexture.height * scale) / 2, crosshairTexture.width * scale, crosshairTexture.height * scale);
    }

    private void OnGUI()
    {
        if (isOn)
            GUI.DrawTexture(position, crosshairTexture);
    }

    public void Enable()
    {
        isOn = true;
    }

    public void Disable()
    {
        isOn = false;
    }
}
