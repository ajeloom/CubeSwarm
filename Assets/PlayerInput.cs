using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private GameObject pistol;
    private GameObject shotgun;

    // Start is called before the first frame update
    void Start()
    {
        Player player = GetComponent<Player>();
        GameObject gunHolder = player.GetGunHolder();
        pistol = gunHolder.transform.GetChild(0).gameObject;
        shotgun = gunHolder.transform.GetChild(1).gameObject;

        pistol.SetActive(true);
        shotgun.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            Gun gun = shotgun.GetComponent<Gun>();
            gun.isAttacking = false;
            gun.isReloading = false;
            pistol.SetActive(true);
            shotgun.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            Gun gun = pistol.GetComponent<Gun>();
            gun.isAttacking = false;
            gun.isReloading = false;
            pistol.SetActive(false);
            shotgun.SetActive(true);
        }
    }
}
