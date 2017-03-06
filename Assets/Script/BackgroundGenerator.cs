using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundGenerator : MonoBehaviour {
    public GameObject[] prefabs;

    public float yStepFrom, yStepTo;
    public float xStepFrom, xStepTo;

    public float yDelta;
    
    Transform player;

    float curY = 5f;

    void Start () {
        player = GameObject.FindWithTag("Player").transform;
        StartCoroutine(DoGeneration());
        StartCoroutine(DoCleanup());
    }

    IEnumerator DoGeneration() {
        var wait = new WaitForSeconds(0.1f);
        while (true) {
            yield return wait;

            while (15 > curY + transform.position.y) {
                GenerateOnce();
            }
        }
    } 

    IEnumerator DoCleanup() {
        var wait = new WaitForSeconds(5.0f);
        while (true) {
            yield return wait;

            for (int i = 0; i < transform.childCount; ++i) {
                var child = transform.GetChild(i);
                if (child.position.y < transform.position.y - 8) {
                    Destroy(child.gameObject);
                }
            }
        }
    }

    void GenerateOnce() {
        float x0 = -10 + Random.value * 5;
        var dx = player.transform.position.x;
        while (x0 < 10) {
            var genPos = new Vector2(x0 + dx, transform.position.y + curY + (Random.value - 0.5f) * yDelta);
            Instantiate(Util.RandomItem(prefabs), genPos, Quaternion.identity, transform);
            x0 += Random.Range(xStepFrom, xStepTo);
        }

        var stepY = Random.Range(yStepFrom, yStepTo);
        curY += stepY;
    }
}
