using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitLevel : MonoBehaviour
{
	private bool collided = false;

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collided && collision.gameObject.tag == "Player")
		{
			FindObjectOfType<GameManager>().NextLevel();
			FindObjectOfType<GameManager>().Player.Movement.Disable();
			foreach (var actor in FindObjectOfType<ActorRegistry>().Actors)
			{
				actor.GetComponent<Actor>().Movement.Disable();
			}
			collided = true;
		}
	}
}
