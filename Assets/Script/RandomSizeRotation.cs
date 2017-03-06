using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSizeRotation : MonoBehaviour {

    public float sizeFrom = 0.6f, sizeTo = 1.0f;

    void Start () {
        var size = Random.Range(sizeFrom, sizeTo);
        transform.localScale = new Vector3(size, size, 1);
        transform.rotation = Quaternion.Euler(0, 0, Random.value * 360f);

        Destroy(this);
    }
	
}
