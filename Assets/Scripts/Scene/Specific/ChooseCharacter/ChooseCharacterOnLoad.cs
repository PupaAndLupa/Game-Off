using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseCharacterOnLoad : MonoBehaviour
{
    public GameObject class1;
    public GameObject class2;

    void Start()
    {
        GameObject one = Instantiate(class1, new Vector3(-3f, 0.2f, 0f), Quaternion.identity, null);
        one.transform.localScale = new Vector3(8, 8, 1);
        one.GetComponent<Collider2D>().enabled = false;
        one.GetComponent<Player>().enabled = false;
        one.GetComponentInChildren<Camera>().enabled = false;

        GameObject two = Instantiate(class2, new Vector3(3f, 0.2f, 0f), Quaternion.identity, null);
        two.transform.localScale = new Vector3(8, 8, 1);
        two.GetComponent<Collider2D>().enabled = false;
        two.GetComponent<Player>().enabled = false;
        two.GetComponentInChildren<Camera>().enabled = false;
    }

    public void ChosenOne()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        gm.SetPlayer(class1);
        SceneManager.LoadScene(2);
    }

    public void ChosenTwo()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        gm.SetPlayer(class2);
        SceneManager.LoadScene(2);
    }
}
