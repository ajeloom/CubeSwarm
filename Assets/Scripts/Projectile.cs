using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody body;

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
    public void Fire(Vector3 direction, float bulletSpeed) 
    {
        body.velocity = direction * bulletSpeed;
    }

    private void OnCollisionEnter() 
    {
        Destroy(gameObject);
    }
}
