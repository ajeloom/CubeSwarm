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
        GameObject canvas = transform.GetChild(0).gameObject;
        GameObject bar = canvas.transform.GetChild(1).gameObject;
        healthBar = bar.GetComponent<Image>();

        // Get the player's health component
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        healthComponent = player.GetComponent<HealthComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = healthComponent.currentHP / healthComponent.maxHP;
    }
}