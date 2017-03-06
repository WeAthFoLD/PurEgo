using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanupCollider : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other) {
        print("TE2D");
        var sr = other.GetComponent<SpriteRenderer>();
        if (sr) {
            StartCoroutine(StartFadeOut(sr));
        }
    }

    IEnumerator StartFadeOut(SpriteRenderer sr) {
        var t = Time.time;
        var wait = new WaitForEndOfFrame();
        while (Time.time - t <= 0.2f) {
            yield return wait;
            var nc = sr.color;
            nc.a = 1 - Mathf.SmoothStep(0, 1, (Time.time - t) / 0.2f);
            sr.color = nc;
        }
    }

}
