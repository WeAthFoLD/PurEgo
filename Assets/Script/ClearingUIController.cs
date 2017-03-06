using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearingUIController : MonoBehaviour {
    public Text purityText, bestText;

    void Start() {
        var purity = PlayerPrefs.GetInt("LastScore", 0);
        var best = PlayerPrefs.GetInt("MaxScore", 0);

        purityText.text = purity.ToString();
        bestText.text = best.ToString();
    }

}
