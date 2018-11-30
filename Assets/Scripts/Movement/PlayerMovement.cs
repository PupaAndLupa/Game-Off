using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement
{
    public PlayerMovement() : base(){}

    public override void Move(float movespeed)
    {
        Vector2 axis = new Vector2(0f, 0f);

        if (Input.GetKey(KeyCode.W))
        {
            axis.y++;
        }

        if (Input.GetKey(KeyCode.S))
        {
            axis.y--;
        }

        if (Input.GetKey(KeyCode.A))
        {
            axis.x--;
        }

        if (Input.GetKey(KeyCode.D))
        {
            axis.x++;
        }

        GetComponent<Rigidbody2D>().velocity = axis * movespeed * Time.deltaTime;
    }

    public override void Rotate()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 position = transform.Find("Camera").GetComponent<Camera>().WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(mousePosition.y - position.y, mousePosition.x - position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        transform.Find("Camera").rotation = Quaternion.Euler(0, 0, 0);
    }
}
