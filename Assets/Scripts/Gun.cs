using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] protected int magSize;
    [SerializeField] protected int maxAmmo;
    public int totalInMag;
    public int totalAmmoLeft;
    [SerializeField] protected GameObject projectile;
    
    public bool isAttacking = false;
    public bool isReloading = false;
    protected bool canReload = false;

    private GameManager gm;

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
    }

    // Update is called once per frame
    protected void Update()
    {
        if (!gm.CheckIfPaused()) {
            if (Input.GetMouseButtonDown(0) && !isAttacking && !isReloading && totalInMag > 0) {
                isAttacking = true;
                Shoot();
            }

            if (totalInMag < magSize && totalAmmoLeft > 0 && !isReloading) {
                canReload = true;
            }

            if ((Input.GetKeyDown(KeyCode.R) && canReload) || (totalInMag == 0 && canReload)) {
                canReload = false;
                Reload();
            }
        }
    }

    protected IEnumerator ShotDelay(float time)
    {
        yield return new WaitForSeconds(time);
        isAttacking = false;
    }

    protected IEnumerator ReloadDelay(float time, int reloadedAmount)
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

    protected virtual void Shoot()
    {
        audioSource.PlayOneShot(shootSFX);
    }

    protected virtual void Reload()
    {
        isReloading = true;
        int reloadedAmount = magSize - totalInMag;
        StartCoroutine(ReloadDelay(1.0f, reloadedAmount));
    }
}
