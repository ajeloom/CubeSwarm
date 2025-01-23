using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ItemPickup : NetworkBehaviour
{
    [SerializeField] private GameObject item;
    private GameObject weapon;

    private int value = 5;

    private string[] weaponStrings = {"Pistol", "Shotgun"};

    private AudioSource audioSource;
    private AudioClip pickupSFX;

    private bool isPickedUp = false;

    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        audioSource = GetComponent<AudioSource>();
        pickupSFX = audioSource.clip;
    }

    private void OnCollisionEnter(Collision collision) 
    {
        // Destroy when player touches the item
        if (collision.collider.tag == "Player" && !isPickedUp) {
            isPickedUp = true;

            GameObject playerObj = collision.gameObject;
            Player player = playerObj.GetComponent<Player>();
            GameObject gunHolder = player.GetGunHolder();
            weapon = gunHolder.transform.GetChild(0).gameObject;

            // Add ammo to your total amount
            Gun ammo = weapon.GetComponent<Gun>();
            ammo.totalAmmoLeft += value;

            // Print how much ammo gained
            GameObject obj = playerObj.transform.Find("HUD").gameObject;
            AmmoUI ui = obj.GetComponent<AmmoUI>();
            if (ui != null) {
                ui.GainedAmmo(value);
            }

            // Play sound
            SoundManager.instance.PlaySound(pickupSFX, transform.position, 0.3f);

            DestroyBoxServerRpc();
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

    // For despawning the item after getting picked up
    [ServerRpc(RequireOwnership = false)]
    private void DestroyBoxServerRpc()
    {
        gameObject.GetComponent<NetworkObject>().Despawn();
    }
}
