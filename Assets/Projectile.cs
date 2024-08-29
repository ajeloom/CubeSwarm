using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody body;
    private float bulletSpeed = 35.0f;

    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        // Delete the projectile when out of camera view
        if (Mathf.Abs(transform.position.x) > Mathf.Abs(cam.transform.position.x) + 10.0f 
            || Mathf.Abs(transform.position.z) > Mathf.Abs(cam.transform.position.z) + 10.0f) {
            Destroy(gameObject);
        }
    }

    public void Fire(Vector3 direction) {
        body.velocity = direction * bulletSpeed;
    }
}
