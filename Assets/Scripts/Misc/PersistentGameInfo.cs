using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentGameInfo : MonoBehaviour
{
    public float timeRemaining = 0f;
    public float healthRemaining = 0f;
    public float scrapRemaining = 0f;
    
    // Awake, Persist across scenes
    private static PersistentGameInfo instance;

    private void Awake()
    {
        // If an instance already exists, destroy the new one.
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // If this is the first instance, set it as the singleton.
        instance = this;

        // Make this object persistent across scene changes.
        DontDestroyOnLoad(gameObject);
    }
    
    public static PersistentGameInfo GetInstance()
    {
        return instance;
    }
}
