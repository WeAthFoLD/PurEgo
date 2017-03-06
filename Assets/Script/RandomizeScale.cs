using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeScale : MonoBehaviour {
    public AnimationCurve sampleCurve;

    void Start () {
        var scale = sampleCurve.Evaluate(Random.value);
        transform.localScale = new Vector3(scale, scale, 1);
        Destroy(this);
    }
	
}
