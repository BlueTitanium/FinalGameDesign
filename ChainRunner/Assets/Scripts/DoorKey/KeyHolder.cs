using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHolder : MonoBehaviour {

    public static KeyHolder k;

    private List<int> keyList;

    private void Start() {
        k = this;
        keyList = new List<int>();
    }

    public List<int> GetKeyList() {
        return keyList;
    }

    public bool AddKey(int i) {
        if (!keyList.Contains(i))
        {
            keyList.Add(i);
            return true;
        } 
        return false;

    }

    public bool RemoveKey(int i)
    {
        if (keyList.Contains(i))
        {
            keyList.Remove(i);
            return true;
        }
        return false;
    }

    public bool ContainsKey(int i) {
        return keyList.Contains(i);
    }
}
