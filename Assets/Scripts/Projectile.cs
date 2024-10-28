using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    [SerializeField] private Rigidbody body;
    private Vector3 initialPos = Vector3.zero;
    public NetworkObject networkObject;

    private bool addedForce = false;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
        body.isKinematic = false;
    }

    public override void OnNetworkSpawn()
    {
        initialPos = transform.position;
        body.isKinematic = false;
        networkObject = GetComponent<NetworkObject>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move the projectile
        if (!addedForce) {
            addedForce = true;
            body.AddForce(transform.forward * 60.0f, ForceMode.VelocityChange);
        }

        // Delete the projectile when out of camera view
        if (Mathf.Abs(transform.position.x) > Mathf.Abs(initialPos.x) + 30.0f 
                || Mathf.Abs(transform.position.z) > Mathf.Abs(initialPos.z) + 30.0f) {
            DestroyObj();
        }
    }

    // This function will make the projectile move in the direction that the player is aiming at
    public void Fire(Vector3 direction, float bulletSpeed) 
    {
        // body.velocity = direction * bulletSpeed;
        body.AddForce(direction * bulletSpeed, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter() 
    {
        // DestroyObj();
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        StartCoroutine(Wait(10.0f));
    }

    private IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        DestroyObj();
    }

    private void DestroyObj()
    {
        if (networkObject != null) {
            DestroyObjServerRpc();
        }
        else {
            Destroy(gameObject);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void DestroyObjServerRpc()
    {
        networkObject.Despawn();
    }
}
