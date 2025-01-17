using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Entity : NetworkBehaviour
{
    [SerializeField] private float movementSpeed = 5.0f;
    public Rigidbody body;

    [SerializeField] private int score;

    [SerializeField] private AudioClip[] footstepAudioClips;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Move(Vector3 direction)
    {
        body.velocity = direction * movementSpeed;
    }

    public int GetScore()
    {
        return score;
    }

    // Plays a footstep sound during the movement animations
    private void PlayFootstepSound()
    {
        int i = Random.Range(0, footstepAudioClips.Length);
        SoundManager.instance.PlaySound(footstepAudioClips[i], transform, 0.2f);
    }
}
