using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplayController : MonoBehaviour {
    PlayerController player;
    Text text;

    void Start () {
        player = PlayerController.instance;
        text = GetComponent<Text>();

        text.text = "";
    }
	
    void Update () {
        var t = player.time;
        var str = t.ToString();
        string realstr;
        if (t < 10) realstr = "000" + str;
        else if (t < 100) realstr = "00" + str;
        else if (t < 1000) realstr = "0" + str;
        else realstr = str;

        text.text = realstr;
    }
}
