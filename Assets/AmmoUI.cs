using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    private TextMeshProUGUI ammo;
    private GameObject pistol;
    private GameObject shotgun;
    private GameObject player;
    private GameObject reloadSprite;

    // Start is called before the first frame update
    void Start()
    {
        GameObject canvas = transform.GetChild(0).gameObject;
        GameObject ammoText = canvas.transform.GetChild(3).gameObject;
        ammo = ammoText.GetComponent<TextMeshProUGUI>();

        reloadSprite = canvas.transform.GetChild(4).gameObject;
        reloadSprite.SetActive(false);

        player = GameObject.FindGameObjectWithTag("Player");
        GameObject temp = player.transform.GetChild(1).gameObject;
        pistol = temp.transform.GetChild(0).gameObject;
        shotgun = temp.transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
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
