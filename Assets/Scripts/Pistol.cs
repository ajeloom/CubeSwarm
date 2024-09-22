using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{
    [SerializeField] private AudioClip reloadSFX1, reloadSFX2;

    protected override void Shoot()
    {
        base.Shoot();
        totalInMag--;
        GameObject ball = Instantiate(projectile, transform.parent.position + (transform.parent.forward * 2.5f), transform.parent.rotation);

        // Exclude this layers that the projectile can collide with
        CapsuleCollider collider = ball.GetComponent<CapsuleCollider>();
        LayerMask mask = LayerMask.GetMask("Player", "Projectile");
        collider.excludeLayers = mask;

        // Change the damage and knockback
        Projectile bullet = ball.GetComponent<Projectile>();
        DamageComponent dc = bullet.GetComponent<DamageComponent>();
        dc.ChangeDamage(damage);
        dc.ChangeKnockback(knockback);

        bullet.Fire(transform.forward, 60.0f);

        StartCoroutine(ShotDelay(0.05f));
    }

    protected override void Reload()
    {
        base.Reload();
        StartCoroutine(PlaySound(0.1f, reloadSFX1, 2.5f));
        StartCoroutine(PlaySound(0.6f, reloadSFX2, 2.0f));
    }

    protected IEnumerator PlaySound(float time, AudioClip sound, float volume)
    {
        yield return new WaitForSeconds(time);
        audioSource.PlayOneShot(sound, volume);
    }
}
