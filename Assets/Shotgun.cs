using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    // Gets a random position for each shotgun pellet
    private Vector3 GetRandomPosition()
    {
        Vector3 pos;
        pos.x = Random.Range(-1.2f, 1.2f) + transform.parent.position.x + (transform.parent.forward.x * 2);
        pos.y = Random.Range(-0.2f, 0.2f) + transform.parent.position.y + (transform.parent.forward.y * 2);
        pos.z = Random.Range(-1.2f, 1.2f) + transform.parent.position.z + (transform.parent.forward.z * 2);

        return pos;
    }

    protected override void Shoot()
    {
        totalInMag--;
        for (int i = 0; i < 7; i++) {
            GameObject ball = Instantiate(projectile, GetRandomPosition(), transform.parent.rotation);

            ball.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

            // Exclude this layers that the projectile can collide with
            SphereCollider collider = ball.GetComponent<SphereCollider>();
            LayerMask mask = LayerMask.GetMask("Player", "Projectile");
            collider.excludeLayers = mask;

            // Change the damage
            Projectile bullet = ball.GetComponent<Projectile>();
            DamageComponent dc = bullet.GetComponent<DamageComponent>();
            dc.ChangeDamage(7.0f);
            dc.ChangeKnockback(15.0f);

            bullet.Fire(transform.forward, 30.0f);
        }

        StartCoroutine(ShotDelay(0.6f));
    }
}
