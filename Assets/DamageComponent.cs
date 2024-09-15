using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageComponent : MonoBehaviour
{
    [SerializeField] private float damageNumber = 10.0f;
    [SerializeField] private float knockback = 5.0f;

    private bool hitted = false;
    public string hittableTag;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(Collision collision, Vector3 direction)
    {
        HealthComponent health = collision.gameObject.GetComponent<HealthComponent>();
        health.currentHP -= damageNumber;

        collision.rigidbody.AddForce(direction * knockback, ForceMode.Impulse);
    }

    // This function will call the damage function from the enemy the projectile hits
    // Projectile will only collide with enemies through the use of tags
    private void OnCollisionEnter(Collision collision) 
    {
        if (collision.gameObject.tag == hittableTag && !hitted) {
            hitted = true;
            Damage(collision, transform.forward);
        }
    }

    // This function helps with stopping the damage function from being called multiple times when collision occurs
    private void OnCollisionExit()
    {
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
