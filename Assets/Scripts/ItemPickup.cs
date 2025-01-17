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
    void Start()
    {
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        GameObject gunHolder = player.GetGunHolder();

        // Only pistol works right now
        weapon = gunHolder.transform.GetChild(0).gameObject;

        audioSource = GetComponent<AudioSource>();
        pickupSFX = audioSource.clip;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision) 
    {
        // Destroy when player touches the item
        if (collision.collider.tag == "Player" && !isPickedUp) {
            isPickedUp = true;

            // Hide the model
            MeshRenderer mesh = GetComponent<MeshRenderer>();
            mesh.enabled = false;

            // Add ammo to your total amount
            Gun ammo = weapon.GetComponent<Gun>();
            ammo.totalAmmoLeft += value;

            // Print how much ammo gained
            GameObject obj = collision.gameObject.transform.Find("HUD").gameObject;
            AmmoUI ui = obj.GetComponent<AmmoUI>();
            ui.GainedAmmo(value);

            // Play sound
            SoundManager.instance.PlaySound(pickupSFX, transform, 0.3f);

            StartCoroutine(Wait(2.0f));
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
    private IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.GetComponent<NetworkObject>().Despawn();
    }
}
