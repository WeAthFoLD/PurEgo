using UnityEngine;
using System;

public static class Util {

    public static void BroadcastEveryone(string message, object parameter, SendMessageOptions options) {
        foreach (var go in (GameObject[]) GameObject.FindObjectsOfType(typeof(GameObject))) {
            go.SendMessage(message, parameter, SendMessageOptions.DontRequireReceiver);
        }
    }

    public static void BroadcastEveryone(string message, SendMessageOptions options) {
        BroadcastEveryone(message, null, options);
    }

    public static Vector2 xy(this Vector3 v) {
        return new Vector2(v.x, v.y);
    }

    public static Vector3 MoveTowards(Vector3 from, Vector3 to, float max) {
        return new Vector3(
            Mathf.MoveTowards(from.x, to.x, max),
            Mathf.MoveTowards(from.y, to.y, max),
            Mathf.MoveTowards(from.z, to.z, max));
    }

    public static Vector2 MoveTowards(Vector2 from, Vector2 to, float max) {
        return new Vector2(
            Mathf.MoveTowards(from.x, to.x, max),
            Mathf.MoveTowards(from.y, to.y, max)
            );
    }
    
    public static T RandomItem<T>(T[] arr) {
        if (arr.Length == 0) {
            throw new Exception("Empty array");
        }
        return arr[(int) (arr.Length * UnityEngine.Random.value)];
    }

}