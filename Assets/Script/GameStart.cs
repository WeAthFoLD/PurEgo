using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour {

    bool buttonClicked = false;

    public void StartGame() {
        SceneManager.LoadSceneAsync("default");
    }

    public void ButtonClicked() {
        if (!buttonClicked) {
            buttonClicked = true;
            GetComponent<Animator>().Play("BeginSwitch");
        }
    }

}
