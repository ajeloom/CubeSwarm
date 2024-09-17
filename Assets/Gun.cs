using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private int magSize;
    [SerializeField] private int maxAmmo;
    public int totalInMag;
    public int totalAmmoLeft;
    [SerializeField] protected GameObject projectile;
    
    public bool isAttacking = false;
    public bool isReloading = false;
    protected bool canReload = false;
    
    // Start is called before the first frame update
    protected void Start()
    {
        totalInMag = magSize;
        totalAmmoLeft = maxAmmo;
    }

    // Update is called once per frame
    protected void Update()
    {
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

        

        isReloading = false;
    }

    protected virtual void Shoot()
    {

    }

    protected void Reload()
    {
        isReloading = true;
        int reloadedAmount = magSize - totalInMag;
        StartCoroutine(ReloadDelay(1.0f, reloadedAmount));
    }
}
