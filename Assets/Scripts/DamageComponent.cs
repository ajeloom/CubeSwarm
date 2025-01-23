using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class DamageComponent : NetworkBehaviour
{
    [SerializeField] private float damageNumber = 10.0f;
    [SerializeField] private float knockback = 5.0f;

    private bool hitted = false;
    public string hittableTag;

    public Collision col = null;

    public void Damage()
    {
        HealthComponent health = col.gameObject.GetComponent<HealthComponent>();
        health.currentHP.Value -= damageNumber;
        health.SetTakingDamage(true);
    }

    [ServerRpc(RequireOwnership = false)]
    public void DamageServerRpc()
    {
        Damage();
    }

    // This function will call the damage function from the enemy the projectile hits
    // Projectile will only collide with enemies through the use of tags
    private void OnCollisionEnter(Collision collision) 
    {
        if (collision.gameObject.tag == hittableTag && !hitted) {
            col = collision;
            hitted = true;

            if (IsServer) {
                Damage();
            }
            else {
                DamageServerRpc();
            }

            collision.rigidbody.AddForce(transform.forward * knockback, ForceMode.VelocityChange);
        }
    }

    // This function helps with stopping the damage function from being called multiple times when collision occurs
    private void OnCollisionExit()
    {
        col = null;
        hitted = false;
    }

    public void ChangeDamage(float newDamage)
    {
        damageNumber = newDamage;
    }

    public void ChangeKnockback(float newKnockback)
    {
        knockback = newKnockback;
    }
}
