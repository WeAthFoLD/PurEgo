using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiggler : MonoBehaviour {
    public float amplitude;
    public float speed = 10f;

    Vector2 direction;

    float phaseDelta;
	
    Vector3 initPos;

    void Start() {
        initPos = transform.position;
        phaseDelta = Random.value * Mathf.PI * 2;
        direction = amplitude * Random.insideUnitCircle;
    }

    void Update () {
        transform.position = initPos + 
            ((Vector3) direction * transform.localScale.x) * Mathf.Pow(Mathf.Sin(Time.time * speed - phaseDelta), 2);
    }

}
