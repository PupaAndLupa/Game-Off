using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 5f;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        Vector2 movement = new Vector2(0f, 0f);

        if (Input.GetKey("w"))
        {
            movement.y += 1;
        }

        if (Input.GetKey("s"))
        {
            movement.y -= 1;
        }

        if (Input.GetKey("a"))
        {
            movement.x -= 1;
        }

        if (Input.GetKey("d"))
        {
            movement.x += 1;
        }

        Vector2 mousePosition = Input.mousePosition;

        movement *= speed * Time.deltaTime;

        transform.position += new Vector3(movement.x, movement.y);

        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);

        float angle = Mathf.Atan2(mousePosition.y - pos.y, mousePosition.x - pos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        if (Input.GetMouseButtonDown(0))
        {
            GameObject bullet = GameObject.Find("Bullet");

            GameObject clone = Instantiate(bullet, transform.position, transform.rotation, transform.parent);

            Vector3 mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mPos.z = 0f;

            clone.GetComponent<BulletController>().direction = (mPos - transform.position).normalized;
        }
    }
}
