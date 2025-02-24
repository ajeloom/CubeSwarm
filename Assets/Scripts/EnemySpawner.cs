using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemySpawner : NetworkBehaviour
{
    [SerializeField] private GameObject enemy;
    public bool spawnedEnemy = false;

    private int enemiesSpawnedInRound = 0;

    private bool startSpawning = false;
    private float countDownTimer = 4.0f;

    private bool waveStarted = false;

    private void Awake()
    {
        GameManager.instance.OnWaveStart += NewWaveStarted;
    }

    private void NewWaveStarted(object sender, EventArgs e)
    {
        waveStarted = true;
        enemiesSpawnedInRound = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!NetworkManager.Singleton.IsServer) {
            return;
        }

        // Wait some time before spawner starts spawning enemies
        if (waveStarted) {
            countDownTimer -= Time.deltaTime;
            int time = (int)countDownTimer;

            if (time <= 0) {
                waveStarted = false;
                startSpawning = true;
                countDownTimer = 4.0f;
            }
        }

        if (startSpawning) {
            if (!spawnedEnemy) {
                spawnedEnemy = true;
                var instance = Instantiate(enemy, transform.position, transform.rotation);
                var instanceNetworkObject = instance.GetComponent<NetworkObject>();
                instanceNetworkObject.Spawn();

                enemiesSpawnedInRound++;

                StartCoroutine(Wait());
            }
        }

        // Stop spawning enemies when all the enemies for that spawner has been spawned
        if (enemiesSpawnedInRound == GameManager.instance.waveNumber.Value + GameManager.instance.enemiesPerSpawn) {
            startSpawning = false;
        }
    }

    private IEnumerator Wait()
    {
        int wave = GameManager.instance.waveNumber.Value - 1;
        float waitTime = wave * 0.15f;
        if (waitTime > 5.0f) {
            waitTime = 5.0f;
        }
        yield return new WaitForSeconds(GetRandomTime(5.0f - waitTime, 12.0f - waitTime));
        spawnedEnemy = false;
    }

    private float GetRandomTime(float min, float max)
    {
        float time = UnityEngine.Random.Range(min, max);
        return time;
    }
}
