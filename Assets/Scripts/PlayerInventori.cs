using UnityEngine;
using System.Collections.Generic;

public class PlayerInventori : MonoBehaviour
{
    public static PlayerInventori instance;

    private List<string> keys = new List<string>();

    void Awake()
    {
        instance = this;
    }

    public void AddKey(string keyID)
    {
        if (!keys.Contains(keyID))
            keys.Add(keyID);
    }

    public bool HasKey(string keyID)
    {
        return keys.Contains(keyID);
    }
}