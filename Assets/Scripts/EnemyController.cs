using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	void Start ()
    {
		
	}
	
	void Update ()
    {
        Vector3 objPos = GameObject.Find("Player").transform.position;
        //Vector3 pos = transform.position;

        objPos -= transform.position;

        float angle = Mathf.Atan2(objPos.y, objPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }
}
