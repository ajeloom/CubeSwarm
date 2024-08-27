using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed = 8;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        // Movement in four directions
        if (Input.GetKey(KeyCode.W)) {
            transform.position += new Vector3(1.0f * speed, 0.0f, 0.0f) * Time.deltaTime;   // Forward
        }
        if (Input.GetKey(KeyCode.S)) {
            transform.position += new Vector3(-1.0f * speed, 0.0f, 0.0f) * Time.deltaTime;  // Backward
        }
        if (Input.GetKey(KeyCode.A)) {
            transform.position += new Vector3(0.0f, 0.0f, 1.0f * speed) * Time.deltaTime;   // Left
        }
        if (Input.GetKey(KeyCode.D)) {
            transform.position += new Vector3(0.0f, 0.0f, -1.0f * speed) * Time.deltaTime;  // Right
        }

        // Make the camera follow the player
        cam.transform.position = transform.position + new Vector3(0, 10, 0);
    }
}
