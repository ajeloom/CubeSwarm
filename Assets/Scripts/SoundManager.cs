using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SoundManager : NetworkBehaviour
{
    public static SoundManager instance { get; private set; }
    [SerializeField] private GameObject audioSourcePrefab;

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
    public void PlaySound(AudioClip audioClip, Vector3 spawnLocation, float volume)
    {
        GameObject obj = Instantiate(audioSourcePrefab, spawnLocation, Quaternion.identity);

        // Assign the sound and volume then play it
        AudioSource audioSource = obj.GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();

        // Destroy after sound is done playing
        Destroy(obj.gameObject, audioSource.clip.length);
    }
}
