using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    private bool spawnedEnemy = false;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawnedEnemy) {
            spawnedEnemy = true;
            Instantiate(enemy, transform.position, transform.rotation);

            StartCoroutine(Wait(3.0f));
        }
    }

    private IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        spawnedEnemy = false;
    }
}
