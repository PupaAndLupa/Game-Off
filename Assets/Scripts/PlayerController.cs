using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 500f;

    private GameObject playerCamera;

    public GameObject bullet;

	void Start ()
    {
        playerCamera = transform.Find("Camera").gameObject;
	}
	
	void Update ()
    {
        HandleMovement();
        HandleRotation();
        HandleControls();
        HandleCamera();
    }

    void HandleMovement()
    {
        Vector2 movement = new Vector2(0f, 0f);

        if (Input.GetKey("w")){
            movement.y += 1;
        }

        if (Input.GetKey("s")){
            movement.y -= 1;
        }

        if (Input.GetKey("a")){
            movement.x -= 1;
        }

        if (Input.GetKey("d")){
            movement.x += 1;
        }

        movement *= speed * Time.deltaTime;
        /*transform.position += new Vector3(movement.x, movement.y);*/
        GetComponent<Rigidbody2D>().velocity = movement;
    }

    void HandleRotation()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 position = playerCamera.GetComponent<Camera>().WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(mousePosition.y - position.y, mousePosition.x - position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);  
    }

    void HandleControls()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject clone = Instantiate(bullet, transform.position + transform.forward * 2, transform.rotation, transform.parent);   

            Vector3 mousePosition = playerCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;

            clone.GetComponent<BulletController>().direction = (mousePosition - transform.position).normalized;
        }
    }

    void HandleCamera()
    {
        playerCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
