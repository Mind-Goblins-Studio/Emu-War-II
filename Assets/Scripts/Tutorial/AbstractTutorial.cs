using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractTutorial : MonoBehaviour
{
    protected bool isStarted = false;
    protected bool isFinished = false;
    
    public void PlayTutorial()
    {
        isStarted = true;
    }

    public bool IsFinished()
    {
        return isFinished;
    }
}
