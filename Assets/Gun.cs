using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public int magSize;
    public int maxAmmo;
    public GameObject projectile;
    public bool isAttacking;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator ShotDelay(float time)
    {
        yield return new WaitForSeconds(time);
        isAttacking = false;
    }
}
