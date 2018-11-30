using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleObjectVisibility : MonoBehaviour {

    public string Tag;

    public void Awake()
    {
        Handle();
    }

    public void Handle()
    {
        GameObject obj = GameObject.FindGameObjectWithTag(Tag);
        if (obj.activeInHierarchy)
        {
            obj.SetActive(false);
        } else
        {
            obj.SetActive(true);
        }
    }
}
