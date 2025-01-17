using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Pistol : Gun
{
    [SerializeField] private AudioClip reloadSFX1, reloadSFX2;

    public override void Shoot()
    {
        if (!IsOwner) {
            return;
        }

        base.Shoot();
        totalInMag--;

        SpawnBulletLocalBullet();
        if (IsHost) {
            SpawnBullet();
        }
        else {
            // Spawn a bullet on the client side with no hit-detection
            // GameObject instance = Instantiate(localProjectile, transform.parent.position + (transform.parent.forward * 2.5f), transform.parent.rotation);
            // ChangeBulletParameters(instance);
            SpawnBulletServerRpc();
        }

        StartCoroutine(ShotDelay(0.05f));
    }

    public override void Reload()
    {
        base.Reload();
        StartCoroutine(PlaySound(0.1f, reloadSFX1, 2.0f));
        StartCoroutine(PlaySound(0.6f, reloadSFX2, 1.5f));
    }

    // Control when to play each sound
    protected IEnumerator PlaySound(float time, AudioClip sound, float volume)
    {
        yield return new WaitForSeconds(time);
        SoundManager.instance.PlaySound(sound, transform, volume);
    }

    void SpawnBullet() {
        GameObject instance = Instantiate(projectile, transform.parent.position + (transform.parent.forward * 2.5f), transform.parent.rotation);
        NetworkObject instanceNetworkObject = instance.GetComponent<NetworkObject>();
        ChangeBulletParameters(instance);
        instanceNetworkObject.Spawn();
    }

    void SpawnBulletLocalBullet() {
        GameObject instance = Instantiate(localProjectile, transform.parent.position + (transform.parent.forward * 2.5f), transform.parent.rotation);
        // ChangeBulletParameters(instance);

        // Exclude this layers that the projectile can collide with
        CapsuleCollider collider = instance.GetComponent<CapsuleCollider>();
        LayerMask mask = LayerMask.GetMask("Player", "Projectile");
        collider.excludeLayers = mask;
    }

    void ChangeBulletParameters(GameObject instance)
    {
        // Exclude this layers that the projectile can collide with
        CapsuleCollider collider = instance.GetComponent<CapsuleCollider>();
        LayerMask mask = LayerMask.GetMask("Player", "Projectile");
        collider.excludeLayers = mask;

        // Change the damage and knockback
        Projectile bullet = instance.GetComponent<Projectile>();
        DamageComponent dc = bullet.GetComponent<DamageComponent>();
        dc.ChangeDamage(damage);
        dc.ChangeKnockback(knockback);
    }

    [ServerRpc]
    void SpawnBulletServerRpc()
    {
        SpawnBullet();
    }
}
