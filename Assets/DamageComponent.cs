using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageComponent : MonoBehaviour
{
    [SerializeField] private float damageNumber = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(Collision collision)
    {
        HealthComponent health = collision.gameObject.GetComponent<HealthComponent>();
        health.healthNumber -= damageNumber;
    }
}
