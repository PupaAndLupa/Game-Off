using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fading : MonoBehaviour {

	public Texture2D fadeOutTexture;
	public float fadeSpeed = 0.8f;

	private int drawDepth = -1000;
	private float alpha = 1.0f;
	private int fadeDir = -1;
	private AsyncOperation Async;

	void OnGUI()
	{
		alpha += fadeDir * fadeSpeed * Time.fixedDeltaTime;
		alpha = Mathf.Clamp01(alpha);
		GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = drawDepth;
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
	}

	public float BeginFade(int direction)
	{
		fadeDir = direction;
		return fadeSpeed;
	}

	void OnLevelWasLoaded()
	{
		BeginFade(-1);
	}

	public void LoadScene(int index, float waitFor = 0.6f)
	{
		StartCoroutine(ChangeScene(index, waitFor));
	}

	IEnumerator ChangeScene(int index, float waitFor = 0.6f)
	{
		yield return StartCoroutine(WaitForRealSeconds(waitFor));
		float fadeTime = BeginFade(1);
		yield return StartCoroutine(WaitForRealSeconds(fadeTime));
		SceneManager.LoadScene(index);
	}

	public static IEnumerator WaitForRealSeconds(float time)
	{
		float start = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < start + time)
		{
			yield return null;
		}
	}
}
