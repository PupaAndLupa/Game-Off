using UnityEngine;

public class ResumeGame : MonoBehaviour {

    public void Resume()
    {
        FindObjectOfType<GameManager>().ContinueGame();
    }
}
