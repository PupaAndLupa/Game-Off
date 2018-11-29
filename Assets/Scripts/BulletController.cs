using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public Vector3 direction;
    private float speed = 1000f;
    private float createTime;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, 2f);
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    private void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().velocity = direction * speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "projectile")
            Destroy(gameObject);
    }
}
