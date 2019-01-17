using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {
	public void LoadByIndex(int index)
	{
        FindObjectOfType<GameManager>().ClearActors();
		SceneManager.LoadScene(index);
	}
}
