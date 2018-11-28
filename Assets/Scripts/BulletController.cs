using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public Vector3 direction;
    private float speed = 10f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //transform.Translate(direction * speed * Time.deltaTime);
        transform.position = transform.position + direction * speed * Time.deltaTime;
	}
}
