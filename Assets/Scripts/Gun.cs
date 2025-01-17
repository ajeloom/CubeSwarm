using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Gun : NetworkBehaviour
{
    [SerializeField] protected int magSize;
    [SerializeField] protected int maxAmmo;
    public int totalInMag;
    public int totalAmmoLeft;
    [SerializeField] protected GameObject projectile;
    [SerializeField] protected GameObject localProjectile;
    
    public bool isAttacking = false;
    public bool isReloading = false;
    public bool canReload = false;
    public bool canShoot = false;

    public bool swapWeapons = false;

    protected GameManager gm;
    public NetworkManager networkManager;

    [SerializeField] protected float damage = 10.0f;
    [SerializeField] protected float knockback = 10.0f;

    protected AudioSource audioSource;
    protected AudioClip shootSFX;

    // Start is called before the first frame update
    protected void Start()
    {
        totalInMag = magSize;
        totalAmmoLeft = maxAmmo;

        GameObject obj = GameObject.FindGameObjectWithTag("GameManager");
        gm = obj.GetComponent<GameManager>();

        audioSource = GetComponent<AudioSource>();
        shootSFX = audioSource.clip;

        if (gm.isMultiplayer)
            networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>();
    }

    // Update is called once per frame
    protected void Update()
    {
        /* 
         * Can shoot when not reloading and you have ammo
         * Can't shoot when reloading or have no loaded ammo
        */
        if (!isAttacking && !isReloading && totalInMag > 0) {
            canShoot = true;
        }
        else if (isAttacking || isReloading || totalInMag <= 0) {
            canShoot = false;
        }

        // Can reload when mag is not fully loaded and there's ammo
        if (totalInMag < magSize && totalAmmoLeft > 0 && !isReloading) {
            canReload = true;
        }

        // Don't gain more ammo than the max amount
        if (totalAmmoLeft > maxAmmo) {
            totalAmmoLeft = maxAmmo;
        }
    }

    // Delays your shot so you can't spam your shots
    public IEnumerator ShotDelay(float time)
    {
        yield return new WaitForSeconds(time);
        isAttacking = false;
    }

    // Handles how long a reload will take
    protected virtual IEnumerator ReloadDelay(float time, int reloadedAmount)
    {
        yield return new WaitForSeconds(time);
        
        // Reload the ammo into mag
        // Make sure you do not go below 0 ammo
        if (reloadedAmount > totalAmmoLeft) {
            totalInMag += totalAmmoLeft;
            totalAmmoLeft = 0;
        }
        else {
            totalInMag += reloadedAmount;
            totalAmmoLeft -= reloadedAmount;
        }

        // Set the sound back to the shooting one
        audioSource.clip = shootSFX;

        isReloading = false;
    }

    // Play sound when you shoot
    public virtual void Shoot()
    {
        SoundManager.instance.PlaySound(shootSFX, transform, 0.6f);
    }

    // Reload ammo
    public virtual void Reload()
    {
        isReloading = true;
        int reloadedAmount = magSize - totalInMag;
        StartCoroutine(ReloadDelay(1.0f, reloadedAmount));
    }

    public void SwapWeapons()
    {
        isAttacking = false;
        isReloading = false;
    }
}
