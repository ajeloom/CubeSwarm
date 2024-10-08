using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : Entity
{
    private Camera cam;
    private Camera screen;
    private Vector3 camPosition;
    private Vector3 direction;
    [SerializeField] private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        camPosition = cam.transform.position;

        screen = GameObject.FindGameObjectWithTag("Screen").GetComponent<Camera>();

        body = GetComponent<Rigidbody>();
        direction = Vector3.zero;
    }

    void Update()
    {
        // Get player input
        direction = new Vector3(Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));
        if (direction != Vector3.zero) {
            animator.SetBool("isMoving", true);
        }
        else {
            animator.SetBool("isMoving", false);
        }

        // Make the camera follow the player
        cam.transform.position = screen.transform.position = transform.position + camPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Move in the direction that the player presses
        Move(direction);
    }
}
