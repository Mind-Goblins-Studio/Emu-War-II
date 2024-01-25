using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFade : MonoBehaviour
{
    [SerializeField] private Fading fading;
    
    // Start is called before the first frame update
    void Start()
    {
        // delayed fade in
        Invoke("SceneFadeIn", 0.1f);
    }
    
    [ContextMenu("Fade In")]
    public void SceneFadeIn()
    {
        fading.FadeOut();
    }

    [ContextMenu("Fade Out")]
    public void SceneFadeOut()
    {
        fading.FadeIn();
    }
}