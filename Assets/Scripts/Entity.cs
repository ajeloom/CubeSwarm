using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Entity : NetworkBehaviour
{
    [SerializeField] private float movementSpeed = 5.0f;
    public Rigidbody body;

    [SerializeField] public int score { get; private set; }

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

    // Plays a footstep sound during the movement animations
    private void PlayFootstepSound()
    {
        int i = Random.Range(0, footstepAudioClips.Length);
        SoundManager.instance.PlaySound(footstepAudioClips[i], transform.position, 0.2f);
    }
}
