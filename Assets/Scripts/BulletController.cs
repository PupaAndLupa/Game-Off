using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public Vector3 direction;
    private float speed = 10f;
    private float createTime;

	// Use this for initialization
	void Start () {
        createTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Rigidbody2D>().velocity = direction * speed * Time.deltaTime;

        if (Time.time - createTime > 2)
        {
            Destroy(gameObject);
        }
	}
}
