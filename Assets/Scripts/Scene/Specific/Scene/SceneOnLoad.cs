using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneOnLoad : MonoBehaviour {

    public GameObject PlayerPrefab;
    public GameObject PauseMenuPrefab;

    void Start () {
        GameManager gm = FindObjectOfType<GameManager>();
        gm.SetPlayer(PlayerPrefab);
        gm.SetPauseMenu(PauseMenuPrefab);
        gm.InitGame();
    }

    void Update () {
        
    }
}
