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

    private AudioSource audioSource;
    private bool playingSound = false;

    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        healthComponent = GetComponent<HealthComponent>();
        body = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        
        if (IsHost) {
            player = NetworkManager.ConnectedClients[GetRandomPlayer()].PlayerObject.gameObject;
        }
    }

    void Update()
    {
        if (!playingSound) {
            playingSound = true;
            StartCoroutine(PlaySound(GetRandomTime()));
        }

        if (player != null) {
            // Change to a different player that is alive
            if (IsHost && player.GetComponent<HealthComponent>().currentHP.Value <= 0.0f) {
                player = NetworkManager.ConnectedClients[GetRandomPlayer()].PlayerObject.gameObject;
            }
        }
        else {
            player = NetworkManager.ConnectedClients[GetRandomPlayer()].PlayerObject.gameObject;
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!IsHost) {
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
            }
            else {
                animator.ResetTrigger("Attack");
                animator.SetBool("isTakingDamage", true);
            }
        }
    }

    // Get a random time before replaying sound
    private float GetRandomTime()
    {
        return Random.Range(1.0f, 20.0f);
    }

    private IEnumerator PlaySound(float time)
    {
        yield return new WaitForSeconds(time);
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        playingSound = false;
    }

    // Pick a random player
    private ulong GetRandomPlayer()
    {
        int clientId = Random.Range(0, NetworkManager.ConnectedClients.Count);
        return (ulong)clientId;
    }
}
