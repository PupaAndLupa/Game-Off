using UnityEngine;

public class ResumeGame : MonoBehaviour {
    [SerializeField]
    private GameObject pausePanel;

    public void Resume()
    {
        if (pausePanel.activeInHierarchy)
        {
            Time.timeScale = 1;
            pausePanel.SetActive(false);
        }
    }
}
