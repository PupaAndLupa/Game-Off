using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 5f;

    private GameObject playerCamera;

    public GameObject bullet;

	void Start ()
    {
        playerCamera = transform.Find("Camera").gameObject;
        playerCamera.GetComponent<Camera>().enabled = false;
	}
	
	void Update ()
    {
        HandleMovement();
        HandleRotation();
        HandleControls();
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
        transform.position += new Vector3(movement.x, movement.y);
    }

    void HandleRotation()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 position = playerCamera.GetComponent<Camera>().WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(mousePosition.y - position.y, mousePosition.x - position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        playerCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    void HandleControls()
    {
        if (Input.GetKeyDown("space"))
        {
            GameObject.Find("UI Main Menu").transform.Find("Main Camera").GetComponent<Camera>().enabled = !GameObject.Find("UI Main Menu").transform.Find("Main Camera").GetComponent<Camera>().enabled;
            GameObject.Find("Canvas").GetComponent<Canvas>().enabled = !GameObject.Find("Canvas").GetComponent<Canvas>().enabled;
            playerCamera.GetComponent<Camera>().enabled = !playerCamera.GetComponent<Camera>().enabled;
        }

        if (Input.GetMouseButtonDown(0))
        {
            //GameObject bullet = GameObject.Find("Bullet");

            GameObject clone = Instantiate(bullet, transform.position, transform.rotation, transform.parent);
            

            Vector3 mousePosition = playerCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;

            clone.GetComponent<BulletController>().direction = (mousePosition - transform.position).normalized;
        }
    }
}
