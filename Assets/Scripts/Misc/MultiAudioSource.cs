using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiAudioSource : MonoBehaviour 
{
    [SerializeField] private AudioSource[] audioSources;
    
    public AudioSource[] getAudioSources()
    {
        return audioSources;
    }
}
