using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private Fading box;
    private string nextName;
    [SerializeField] private float delay = 0.55f;

    private void Start()
    {
        Invoke("DelayStart", 0.1f);
    }

    private void DelayStart()
    {
        box.FadeOut();
    }
    
    public void ChangeScene(string name)
    {
        nextName = name;
        box.FadeIn();
        Invoke("ChangeToScene", delay);
    }
    
    private void ChangeToScene()
    {
        SceneManager.LoadScene(nextName);
    }
}
