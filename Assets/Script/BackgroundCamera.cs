using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundCamera : MonoBehaviour {
    public Transform rev;

    void Update() {
        transform.position = new Vector3(rev.position.x, -rev.position.y, 0);
    }

}
