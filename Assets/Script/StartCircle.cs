using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCircle : MonoBehaviour {
    public PlayerController player;

    public void StartGame() {
        player.enabled = true;
        Destroy(gameObject);
    }
    
}
