using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody body;
    private float bulletSpeed = 35.0f;

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

    public void Fire(Vector3 direction) {
        body.velocity = direction * bulletSpeed;
    }
}
