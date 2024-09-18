using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Zombie : Entity
{
    [SerializeField] private GameObject player;
    private HealthComponent healthComponent;
    [SerializeField] private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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

        float distance = Mathf.Sqrt(Mathf.Pow(xDist, 2) + Mathf.Pow(zDist, 2));
        if (!healthComponent.GetTakingDamage()) {
            Move(transform.forward);

            animator.SetBool("isTakingDamage", false);
            
            // Change to attack animation when close to the player
            if (distance <= 2.5f) {
                animator.SetTrigger("Attack");
            }
            else {
                animator.ResetTrigger("Attack");
            }
        }
        else {
            animator.ResetTrigger("Attack");
            animator.SetBool("isTakingDamage", true);
        }
    }
}
