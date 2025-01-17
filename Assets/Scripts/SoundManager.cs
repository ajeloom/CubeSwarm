using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SoundManager : NetworkBehaviour
{
    public static SoundManager instance = null;
    [SerializeField] private AudioSource audioSource;

    void Awake()
    {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Will create a new AudioSource in the scene 
    // that plays the given audio clip at the spawn location
    public void PlaySound(AudioClip audio, Transform spawn, float volume)
    {
        AudioSource source = Instantiate(audioSource, spawn.position, Quaternion.identity);

        // Assign the sound and volume then play it
        source.clip = audio;
        source.volume = volume;
        source.Play();

        // Destroy after sound is done playing
        Destroy(source.gameObject, source.clip.length);
    }
}
