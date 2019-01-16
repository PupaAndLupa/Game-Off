using UnityEngine;
using UnityEngine.UI;

public class SetEndScreenCaptions : MonoBehaviour
{
	public Text ScoreCaption;

    void Start()
    {
		var gm = FindObjectOfType<GameManager>();
		ScoreCaption.text = "Score: " + gm.Score.text;
    }

}
