using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image healthBar;
    private HealthComponent healthComponent;

    // Start is called before the first frame update
    void Start()
    {
        // Get the health bar image
        GameObject canvas = transform.Find("Canvas").gameObject;
        GameObject bar = canvas.transform.Find("HPBar").gameObject;
        healthBar = bar.GetComponent<Image>();

        // Get the player's health component
        GameObject player = transform.parent.gameObject;
        healthComponent = player.GetComponent<HealthComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = healthComponent.currentHP.Value / healthComponent.maxHP.Value;
    }
}
