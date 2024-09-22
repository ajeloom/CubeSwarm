using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // [SerializeField] private GameObject[] inventory = new GameObject[2];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision) 
    {
        // if (collision.gameObject.layer == 10) {
        //     ItemPickup item = collision.gameObject.GetComponent<ItemPickup>();
        //     inventory[0] = item.GetItem();
        // }
    }

    public GameObject GetGunHolder()
    {
        return transform.Find("Gunholder").gameObject;
    }
}
