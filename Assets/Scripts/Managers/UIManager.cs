using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Text HP;

    // Use this for initialization
	void Start () {
        FindObjectOfType<GameManager>().Player.Stats.OnHPChanged += ActorStats_OnHPChanged;
    }

    private void ActorStats_OnHPChanged(float HP)
    {
        this.HP.text = "HP: " + HP;
    }
}