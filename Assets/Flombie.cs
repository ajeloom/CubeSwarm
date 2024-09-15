using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flombie : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private DamageComponent damageComponent;
    private Rigidbody body;
    public GameObject projectile;

    private bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        damageComponent = GetComponent<DamageComponent>();
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

        // Get distance between player and monster
        float distance = Mathf.Sqrt(Mathf.Pow(xDist, 2) + Mathf.Pow(zDist, 2));
        if (distance > 7.5f) {
            Move();
        }
        else {            
            Attack();
        }
    }

    // Move to a position close to the player
    private void Move()
    {
        body.velocity = transform.forward * speed;
    }

    // Shoot a projectile towards the player
    private void Attack() 
    {
        if (!isAttacking) {
            isAttacking = true;

            GameObject ball = Instantiate(projectile, transform.position + transform.forward, transform.rotation);

            Projectile bullet = ball.GetComponent<Projectile>();
            DamageComponent dc = bullet.GetComponent<DamageComponent>();
            dc.hittableTag = "Player";
            bullet.Fire(transform.forward, 35.0f);

            // Wait a second before you can attack again
            StartCoroutine(Wait());
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1.5f);
        isAttacking = false;
    }
}
