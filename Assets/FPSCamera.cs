using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCamera : MonoBehaviour {

    public GameObject parent;
    private Vector3 offset;

	// Use this for initialization
	void Start () {
        offset = transform.position - parent.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = parent.transform.position + offset;
	}
}
