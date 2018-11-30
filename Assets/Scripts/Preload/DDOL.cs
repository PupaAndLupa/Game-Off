using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DDOL : MonoBehaviour {
    public GameObject[] Indestructible;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        foreach (var obj in Indestructible)
        {
            DontDestroyOnLoad(obj);
        }
        SceneManager.LoadScene(1);
    }
}
