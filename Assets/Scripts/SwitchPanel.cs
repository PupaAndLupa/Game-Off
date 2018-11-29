using UnityEngine;

public class SwitchPanel : MonoBehaviour {
    public GameObject Current;
    public GameObject ToSwitch;

    public void Switch()
    {
        ToSwitch.SetActive(true);
        Current.SetActive(false);
    }
}
