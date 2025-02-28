using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    private TextMeshProUGUI ammo;
    private TextMeshProUGUI gainedAmmo;
    private GameObject pistol;
    private GameObject reloadSprite;
    private PlayerInput player;

    private int currentValue = 0;
    private bool isFading = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject canvas = transform.Find("Canvas").gameObject;
        GameObject ammoText = canvas.transform.Find("AmmoNumber").gameObject;
        ammo = ammoText.GetComponent<TextMeshProUGUI>();

        GameObject gainedText = canvas.transform.Find("GainedAmmo").gameObject;
        gainedAmmo = gainedText.GetComponent<TextMeshProUGUI>();

        reloadSprite = canvas.transform.Find("Reload").gameObject;
        reloadSprite.SetActive(false);

        player = transform.parent.gameObject.GetComponent<PlayerInput>();
        GameObject gunHolder = transform.parent.gameObject.GetComponent<Player>().GetGunHolder();
        pistol = gunHolder.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.GetPlayerPaused()) {
            if (pistol.gameObject.activeSelf == true) {
                Pistol temp = pistol.GetComponent<Pistol>();
                PrintText(temp);

                Reloading(temp);
            }

        }
    }

    private void PrintText(Gun temp)
    {
        ammo.SetText(temp.totalInMag.ToString() + "/" + temp.totalAmmoLeft.ToString());
    }

    public void GainedAmmo(int value)
    {
        // Add the amount of ammo collected if the text is still showing
        currentValue += value;
        gainedAmmo.SetText("+" + currentValue.ToString());

        if (!isFading) {
            // Fade In
            StartCoroutine(Fade(true));

            StartCoroutine(Wait());
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(2.0f);

        // Fade Out
        StartCoroutine(Fade(false));

        currentValue = 0;
        isFading = false;
    }

    // Fade the text in or out
    private IEnumerator Fade(bool fadeIn)
    {
        isFading = true;
        if (fadeIn) {
            for (float i = 0; i <= 1; i += Time.deltaTime) {
                gainedAmmo.color = new Color(gainedAmmo.color.r, gainedAmmo.color.g, gainedAmmo.color.b, i);
                yield return null;
            }
        }
        else {
            for (float i = 1; i >= 0; i -= Time.deltaTime) {
                gainedAmmo.color = new Color(gainedAmmo.color.r, gainedAmmo.color.g, gainedAmmo.color.b, i);
                yield return null;
            }
        }
        
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
