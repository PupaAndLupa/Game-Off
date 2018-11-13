using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private readonly float minimumX = -60f;
    private readonly float maximumX = 60f;
    private readonly float minimumY = -360f;
    private readonly float maximumY = 360f;

    private readonly float sensitivityX = 15f;
    private readonly float sensitivityY = 15f;

    private float rotationX = 0f;
    private float rotationY = 0f;

    public Camera cam;
    private Vector3 offset;

	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        offset = cam.transform.position - transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        rotationY += Input.GetAxis("Mouse X") * sensitivityY;
        rotationX += Input.GetAxis("Mouse Y") * sensitivityX;

        rotationX = Mathf.Clamp(rotationX, minimumX, maximumX);
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

        transform.localEulerAngles = new Vector3(0, rotationY);
        cam.transform.localEulerAngles = new Vector3(-rotationX, rotationY);

        if (Input.GetKey("escape"))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        cam.transform.position = transform.position - offset;
    }
}
