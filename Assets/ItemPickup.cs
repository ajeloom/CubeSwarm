using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private GameObject item;
    private GameObject player;
    private GameObject weapon;

    private string[] weaponStrings = {"Pistol", "Shotgun"};

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        GameObject temp = player.transform.GetChild(1).gameObject;
        weapon = temp.transform.GetChild(FindWeaponSlot()).gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision) 
    {
        // Destroy when player touches the item
        if (collision.gameObject.layer == 6) {
            Gun ammo = weapon.GetComponent<Gun>();
            ammo.totalAmmoLeft += 5;

            Destroy(gameObject);
        }
    }

    public GameObject GetItem()
    {
        return item;
    }

    private int FindWeaponSlot()
    {
        for (int i = 0; i < weaponStrings.Length; i++) {
            if (weaponStrings[i] == item.name) {
                return i;
            }
        }

        return 0;
    }
}
