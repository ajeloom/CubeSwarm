using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{
    protected override void Shoot()
    {
        totalInMag--;
        GameObject ball = Instantiate(projectile, transform.parent.parent.position + transform.parent.parent.forward, transform.parent.parent.rotation);

        // Exclude this layers that the projectile can collide with
        SphereCollider collider = ball.GetComponent<SphereCollider>();
        LayerMask mask = LayerMask.GetMask("Player", "Projectile");
        collider.excludeLayers = mask;

        Projectile bullet = ball.GetComponent<Projectile>();
    
        bullet.Fire(transform.forward, 30.0f);

        StartCoroutine(ShotDelay(0.1f));
    }
}
