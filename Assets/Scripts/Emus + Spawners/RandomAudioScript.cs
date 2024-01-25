using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudioScript : MonoBehaviour {
    
    public List<AudioSource> sounds;

    public void PlayRandomSound() {
        // check for sounds
        if (sounds.Count > 0) {
            // random index of sounds (non inclusive range)
            int rand = Random.Range(0, sounds.Count);
            // play sound
            sounds[rand].Play();
        }
    }
}
