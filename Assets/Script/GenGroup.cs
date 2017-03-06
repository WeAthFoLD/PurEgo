using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenGroup : MonoBehaviour {
    [System.Serializable]
    public struct Item {
        public string symbol;
        public GameObject prefab;
        public int width, height;
    }

    public List<Item> items;

    public Item? Find(char symbol) {
        foreach (var it in items) {
            if (it.symbol.Length != 0 && it.symbol[0] == symbol) {
                return it;
            }
        }
        return null;
    }
}
