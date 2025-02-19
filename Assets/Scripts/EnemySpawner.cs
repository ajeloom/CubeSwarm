using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemySpawner : NetworkBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private float timer = 3.0f;
    public bool spawnedEnemy = false;

    // Start is called before the first frame update
    void Start() 
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!NetworkManager.Singleton.IsServer) {
            return;
        }

        if (!spawnedEnemy) {
            spawnedEnemy = true;
            var instance = Instantiate(enemy, transform.position, transform.rotation);
            var instanceNetworkObject = instance.GetComponent<NetworkObject>();
            instanceNetworkObject.Spawn();

            StartCoroutine(Wait(timer));
        }
    }

    private IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        spawnedEnemy = false;
    }
}
