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
        if (!isAttacking && !isReloading && totalInMag > 0) {
            canShoot = true;
        }
        else if (isAttacking || isReloading || totalInMag <= 0) {
            canShoot = false;
        }

        if (totalInMag < magSize && totalAmmoLeft > 0 && !isReloading) {
            canReload = true;
        }
    }

    public IEnumerator ShotDelay(float time)
    {
        yield return new WaitForSeconds(time);
        isAttacking = false;
    }

    protected virtual IEnumerator ReloadDelay(float time, int reloadedAmount)
    {
        yield return new WaitForSeconds(time);
        
        if (reloadedAmount > totalAmmoLeft) {
            totalInMag += totalAmmoLeft;
            totalAmmoLeft = 0;
        }
        else {
            totalInMag += reloadedAmount;
            totalAmmoLeft -= reloadedAmount;
        }

        audioSource.clip = shootSFX;

        isReloading = false;
    }

    public virtual void Shoot()
    {
        audioSource.PlayOneShot(shootSFX);
    }

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
