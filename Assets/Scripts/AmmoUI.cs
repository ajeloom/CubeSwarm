using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    private TextMeshProUGUI ammo;
    private GameObject pistol;
    private GameObject shotgun;
    private GameObject reloadSprite;
    private GameManager gm;
    private PlayerControls player;

    // Start is called before the first frame update
    void Start()
    {
        GameObject canvas = transform.GetChild(0).gameObject;
        GameObject ammoText = canvas.transform.GetChild(3).gameObject;
        ammo = ammoText.GetComponent<TextMeshProUGUI>();

        reloadSprite = canvas.transform.GetChild(4).gameObject;
        reloadSprite.SetActive(false);

        player = transform.parent.gameObject.GetComponent<PlayerControls>();
        GameObject gunHolder = transform.parent.gameObject.GetComponent<Player>().GetGunHolder();
        pistol = gunHolder.transform.GetChild(0).gameObject;
        shotgun = gunHolder.transform.GetChild(1).gameObject;

        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((!player.GetPlayerPaused() && !gm.isMultiplayer) || gm.isMultiplayer) {
            if (pistol.gameObject.activeSelf == true) {
                Pistol temp = pistol.GetComponent<Pistol>();
                PrintText(temp);

                Reloading(temp);
            }
            else if (shotgun.gameObject.activeSelf == true) {
                Shotgun temp = shotgun.GetComponent<Shotgun>();
                PrintText(temp);

                Reloading(temp);
            }
        }
    }

    private void PrintText(Gun temp)
    {
        ammo.SetText(temp.totalInMag.ToString() + "/" + temp.totalAmmoLeft.ToString());
    }

    private void Reloading(Gun temp)
    {
        if (temp.isReloading) {
            reloadSprite.SetActive(true);
            reloadSprite.transform.Rotate(0.0f, 0.0f, -5.0f, Space.Self);
        }
        else {
            reloadSprite.SetActive(false);
        }
    }
}
