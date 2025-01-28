using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HealthComponent : NetworkBehaviour
{
    public NetworkVariable<float> maxHP = new NetworkVariable<float>(50.0f);
    public NetworkVariable<float> currentHP;

    public GameObject ammoPrefab;
    private bool spawnedAmmo = false;

    private bool hpSet = false;
    private bool takingDamage = false;

    public override void OnNetworkSpawn()
    {
        if (IsServer) {
            currentHP.Value = maxHP.Value;
            hpSet = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hpSet) {
            if (currentHP.Value <= 0.0f) {
                if (gameObject.tag == "Player") {
                }
                else {
                    UpdateScoreServerRpc();
                }
            }

            if (takingDamage) {
                StartCoroutine(Wait(1.0f));
            }
        }
    }

    // Tells the server to update score to other clients
    // because the client killed a zombie
    [ServerRpc]
    private void UpdateScoreServerRpc()
    {
        UpdateScore();
    }

    [ClientRpc]
    private void PlayDeathSoundClientRpc()
    {
        Zombie zombie = GetComponent<Zombie>();
        zombie.PlayDeathSound();
    }

    private void UpdateScore()
    {
        // Drop an ammo box
        Entity entity = GetComponent<Entity>();
        GameManager.instance.AddScore(entity.score);

        if (GetRandomDrop()) {
            if (!spawnedAmmo) {
                spawnedAmmo = true;
                GameObject instance = Instantiate(Resources.Load<GameObject>("Prefabs/Itembox"), gameObject.transform);
                NetworkObject instanceNetworkObject = instance.GetComponent<NetworkObject>();
                instanceNetworkObject.Spawn();
            }
        }

        gameObject.GetComponent<NetworkObject>().Despawn();
        PlayDeathSoundClientRpc();
    }

    private IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        takingDamage = false;
    }

    public void SetTakingDamage(bool value)
    {
        takingDamage = value;
    }

    public bool GetTakingDamage()
    {
        return takingDamage;
    }

    // Gives the enemy a random chance to drop ammo
    private bool GetRandomDrop()
    {
        float num = UnityEngine.Random.Range(0, 2);
        bool drop = (num >= 1) ? true : false;
        return drop;
    }
}
