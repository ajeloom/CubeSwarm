using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    [SerializeField] private AudioClip reloadSFX;
    private bool startReloadSFX = false;

    public override void Shoot()
    {
        base.Shoot();
        totalInMag--;
        for (int i = 0; i < 7; i++) {
            GameObject ball = Instantiate(projectile, transform.parent.position + (transform.parent.forward * 2.5f), transform.parent.rotation);

            ball.transform.localScale *= 0.8f;

            // Exclude this layers that the projectile can collide with
            CapsuleCollider collider = ball.GetComponent<CapsuleCollider>();
            LayerMask mask = LayerMask.GetMask("Player", "Projectile");
            collider.excludeLayers = mask;

            // Change the damage and knockback
            Projectile bullet = ball.GetComponent<Projectile>();
            DamageComponent dc = bullet.GetComponent<DamageComponent>();
            dc.ChangeDamage(damage);
            dc.ChangeKnockback(knockback);
            
            switch (i) {
                case 0:
                    bullet.Fire(GetRandomDirection(0.0f, 0.0f), 30.0f);
                    break;
                case 1:
                case 2:
                case 3:
                    bullet.Fire(GetRandomDirection(0.0f, 1.0f), 30.0f);
                    break;
                case 4:
                case 5:
                case 6:
                    bullet.Fire(GetRandomDirection(-1.0f, 0.0f), 30.0f);
                    break;
            }
        }

        StartCoroutine(ShotDelay(0.6f));
    }

    // Gets a random position for each shotgun pellet
    private Vector3 GetRandomDirection(float minRange, float maxRange)
    {
        Vector3 pos;
        pos.x = Random.Range(minRange, maxRange) + transform.forward.x * 2.5f;
        pos.y = 0.0f;
        pos.z = Random.Range(minRange, maxRange) + transform.forward.z * 2.5f;

        return pos;
    }

    protected override IEnumerator ReloadDelay(float time, int reloadedAmount)
    {
        if (!startReloadSFX) {
            startReloadSFX = true;
            for (int i = 0; i < reloadedAmount; i++) {
                StartCoroutine(PlayReloadSound(0.2f + (i * 0.5f), reloadSFX, 3.5f));
            }
            startReloadSFX = false;
        }

        yield return new WaitForSeconds(0.2f + (reloadedAmount * 0.5f));
        
        audioSource.clip = shootSFX;

        isReloading = false;
    }

    protected IEnumerator PlayReloadSound(float time, AudioClip sound, float volume)
    {
        yield return new WaitForSeconds(time);
        SoundManager.instance.PlaySound(sound, transform, volume);
        totalInMag++;
        totalAmmoLeft--;
    }
}
