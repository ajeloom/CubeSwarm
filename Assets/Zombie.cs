using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Zombie : Entity
{
    [SerializeField] private GameObject player;
    [SerializeField] private DamageComponent damageComponent;
    private HealthComponent healthComponent;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        damageComponent = GetComponent<DamageComponent>();
        healthComponent = GetComponent<HealthComponent>();
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Turn towards the player
        float xDist = player.transform.position.x - transform.position.x;
        float zDist = -player.transform.position.z + transform.position.z;
        float turn = Mathf.Atan2(zDist, xDist) * Mathf.Rad2Deg + 90.0f;
        transform.rotation = Quaternion.Euler(0, turn, 0);

        // Move towards the player
        if (!healthComponent.GetTakingDamage()) {
            Move(transform.forward);
        }
    }
}
