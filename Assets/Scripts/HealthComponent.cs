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
        if (IsHost) {
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
                    if (IsHost) {
                        UpdateScore();
                    }
                    else {
                        UpdateScoreServerRpc();
                    }
                }
            }

            if (takingDamage) {
                StartCoroutine(Wait(1.0f));
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdateScoreServerRpc()
    {
        UpdateScore();
    }

    private void UpdateScore()
    {
        GameManager gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();        
        Entity entity = GetComponent<Entity>();
        gm.score.Value += entity.GetScore();

        Zombie zombie = GetComponent<Zombie>();
        zombie.PlayDeathSound();

        // Drop an ammo box
        if (GetRandomDrop()) {
            if (!spawnedAmmo) {
                spawnedAmmo = true;
                GameObject instance = Instantiate(Resources.Load<GameObject>("Prefabs/Itembox"), gameObject.transform);
                NetworkObject instanceNetworkObject = instance.GetComponent<NetworkObject>();
                instanceNetworkObject.Spawn();
            }
        }

        StartCoroutine(Wait(1.0f));
        gameObject.GetComponent<NetworkObject>().Despawn();
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
        float num = Random.Range(0, 2);
        bool drop = (num >= 1) ? true : false;
        return drop;
    }
}
