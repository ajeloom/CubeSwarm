using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : Entity
{
    private Camera cam;
    private Vector3 camPosition;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        camPosition = cam.transform.position + new Vector3(0.0f, -1.0f, 0.0f);

        body = GetComponent<Rigidbody>();
        direction = Vector3.zero;
    }

    void Update()
    {
        // Get player input
        direction = new Vector3(Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Move in the direction that the player presses
        Move(direction);

        // Make the camera follow the player
        cam.transform.position = transform.position + camPosition;
    }
}
