using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeathController : MonoBehaviour {

    PlayerController pc;

    void Start() {
        pc = GetComponent<PlayerController>();
    }

    public void PostDeathCleanup() {
        PlayerPrefs.SetInt("LastScore", pc.time);
        PlayerPrefs.SetInt("MaxScore", System.Math.Max(PlayerPrefs.GetInt("MaxScore", 0), pc.time));
        SceneManager.LoadScene("clearing");
    }

}
