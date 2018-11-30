using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement
{
    public PlayerMovement() : base(){}

    public override void Move(float movespeed)
    {
        Direction = new Vector3(0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            Direction.y++;
        }

        if (Input.GetKey(KeyCode.S))
        {
            Direction.y--;
        }

        if (Input.GetKey(KeyCode.A))
        {
            Direction.x--;
        }

        if (Input.GetKey(KeyCode.D))
        {
            Direction.x++;
        }

        GetComponent<Rigidbody2D>().velocity = Direction * movespeed * Time.deltaTime;
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
