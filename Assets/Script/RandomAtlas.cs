using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAtlas : MonoBehaviour {

    public Sprite[] sprites;
    public float changeInterval = 0.5f;

    SpriteRenderer sr;

    void Start () {
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(DoChange());
    }
	
    IEnumerator DoChange() {
        while (true) {
            yield return new WaitForSeconds(changeInterval);
            var next = sprites[(int) (Random.value * sprites.Length)];
            sr.sprite = next;
        }
    }
}
