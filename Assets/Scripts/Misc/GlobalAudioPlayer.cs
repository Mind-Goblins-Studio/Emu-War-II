using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class GlobalAudioPlayer : MonoBehaviour
{
    public static GlobalAudioPlayer instance;
    
    // Multi Audio Sources
    public MultiAudioSource[] musicMultiAudioSources;
    public MultiAudioSource[] effectsMultiAudioSources;
    
    // Audio Sources
    private List<AudioSource> musicSources = new List<AudioSource>();
    private List<AudioSource> effectsSources = new List<AudioSource>();
    
    // List to store default voumes
    private List<float> defaultMusicVolumes = new List<float>();
    private List<float> defaultEffectsVolumes = new List<float>();
    
    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            SetupAudioSources();
            SetupDefaultVolumes();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Set Volume Music
    public void SetVolumeMusic(float newVolume)
    {
        for (int i = 0; i < musicSources.Count; i++)
        {
            musicSources[i].volume = defaultMusicVolumes[i] * newVolume;
        }
        
    }
    
    // Set Volume Effects
    public void SetVolumeEffects(float newVolume)
    {
        for (int i = 0; i < effectsSources.Count; i++)
        {
            effectsSources[i].volume = defaultEffectsVolumes[i] * newVolume;
        }
    }
    
    
    // Setup Functions
    private void SetupAudioSources()
    {   
        // Setup Music Sources
        foreach (MultiAudioSource multiAudioSource in musicMultiAudioSources)
        {
            foreach (AudioSource audioSource in multiAudioSource.getAudioSources())
            {
                musicSources.Add(audioSource);
            }
        }
        
        // Setup Effects Sources
        foreach (MultiAudioSource multiAudioSource in effectsMultiAudioSources)
        {
            foreach (AudioSource audioSource in multiAudioSource.getAudioSources())
            {
                effectsSources.Add(audioSource);
            }
        }
    }

    private void SetupDefaultVolumes()
    {
        // Store default volumes
        foreach (AudioSource musicSource in musicSources)
        {
            defaultMusicVolumes.Add(musicSource.volume);
        }
        foreach (AudioSource effectsSource in effectsSources)
        {
            defaultEffectsVolumes.Add(effectsSource.volume);
        }
    }
}
