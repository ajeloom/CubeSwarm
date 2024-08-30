using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody body;
    [SerializeField] private float bulletSpeed = 35.0f;
    [SerializeField] private DamageComponent damageComponent;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Delete the projectile when out of camera view
        if (Mathf.Abs(transform.position.x) > Mathf.Abs(player.transform.position.x) + 20.0f 
            || Mathf.Abs(transform.position.z) > Mathf.Abs(player.transform.position.z) + 20.0f) {
            Destroy(gameObject);
        }
    }

    // This function will make the projectile move in the direction that the player is aiming at
    public void Fire(Vector3 direction) {
        body.velocity = direction * bulletSpeed;
    }

    // This function will call the damage function from the enemy the projectile hits
    // Projectile will only collide with enemies through the use of tags
    private void OnCollisionEnter(Collision collision) {
        damageComponent.Damage(collision);
        Destroy(gameObject);
    }
}
