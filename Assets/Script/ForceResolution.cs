using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceResolution : MonoBehaviour {

    void Start () {
        #if UNITY_STANDALONE
        Screen.SetResolution(300, 512, false);
        #endif

        Destroy(this);
    }

}
