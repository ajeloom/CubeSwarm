using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5.0f;
    public Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Move(Vector3 direction)
    {
        body.velocity = direction * movementSpeed;
    }
}
