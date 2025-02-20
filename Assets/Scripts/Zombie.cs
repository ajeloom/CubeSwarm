using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.VisualScripting;
using UnityEngine;

public class Zombie : Entity
{
    private GameObject player;
    private HealthComponent healthComponent;
    [SerializeField] private Animator animator;

    [SerializeField] private AudioClip[] idleAudioClips;
    [SerializeField] private AudioClip[] hurtAudioClips;

    [SerializeField] private AudioClip deathAudioClip;

    private bool playingSound = false;

    private bool playedIdleSound = false;
    private bool playedHurtSound = false;

    private ulong targetId;

    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        healthComponent = GetComponent<HealthComponent>();
        body = GetComponent<Rigidbody>();
        
        if (NetworkManager.Singleton.IsServer) {
            ChooseTarget();
        }

        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
    }

    private void OnClientDisconnected(ulong clientId)
    {
        if (targetId == clientId) {
            player = null;
        }
    }

    void Update()
    {
        if (!playingSound) {
            playingSound = true;
            StartCoroutine(PlaySound(GetRandomTime()));
        }

        if (!NetworkManager.Singleton.IsServer) {
            return;
        }

        // Change to a different player that is alive
        if (player == null || player.GetComponent<HealthComponent>().currentHP.Value <= 0.0f) {
            ChooseTarget();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!NetworkManager.Singleton.IsServer) {
            return;
        }

        if (player != null) {
            // Turn towards the player
            float xDist = player.transform.position.x - transform.position.x;
            float zDist = -player.transform.position.z + transform.position.z;
            float turn = Mathf.Atan2(zDist, xDist) * Mathf.Rad2Deg + 90.0f;
            transform.rotation = Quaternion.Euler(0, turn, 0);

            float distance = Mathf.Sqrt(Mathf.Pow(xDist, 2) + Mathf.Pow(zDist, 2));
            if (!healthComponent.GetTakingDamage()) {
                Move(transform.forward);

                animator.SetBool("isTakingDamage", false);
                
                // Change to attack animation when close to the player
                if (distance <= 2.5f) {
                    animator.SetTrigger("Attack");
                }
                else {
                    animator.ResetTrigger("Attack");
                }

                playedHurtSound = false;
            }
            else {
                animator.ResetTrigger("Attack");
                animator.SetBool("isTakingDamage", true);
                if (!playedHurtSound && !playedIdleSound) {
                    playedHurtSound = true;
                    int i = Random.Range(0, idleAudioClips.Length);
                    SoundManager.instance.PlaySound(hurtAudioClips[i], transform.position, 0.5f);
                }
            }
        }
    }

    private void ChooseTarget()
    {
        targetId = GetRandomPlayer();
        player = NetworkManager.Singleton.ConnectedClients[targetId].PlayerObject.gameObject;
    }

    // Get a random time before replaying sound
    private float GetRandomTime()
    {
        return Random.Range(1.0f, 60.0f);
    }

    private IEnumerator PlaySound(float time)
    {
        yield return new WaitForSeconds(time);

        playedIdleSound = true;

        // Pick a random sound
        int i = Random.Range(0, idleAudioClips.Length);

        SoundManager.instance.PlaySound(idleAudioClips[i], transform.position, 0.5f);
        yield return new WaitForSeconds(idleAudioClips[i].length);
        playingSound = false;
        playedIdleSound = false;
    }

    // Pick a random player
    private ulong GetRandomPlayer()
    {
        int clientId = Random.Range(0, NetworkManager.Singleton.ConnectedClientsIds.Count);
        return (ulong)clientId;
    }

    // Play a sound when killed
    public void PlayDeathSound()
    {
        SoundManager.instance.PlaySound(deathAudioClip, transform.position, 0.5f);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
    }
}
