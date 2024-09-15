using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public float maxHP = 50.0f;
    public float currentHP;

    private bool takingDamage = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHP <= 0.0f) {
            Destroy(gameObject);
        }

        if (takingDamage) {
            StartCoroutine(Wait(1.0f));
        }
    }

    private IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        takingDamage = false;
    }

    public void SetTakingDamage(bool value)
    {
        takingDamage = value;
    }

    public bool GetTakingDamage()
    {
        return takingDamage;
    }
}
