using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;
    [SerializeField] private Vector2 hotSpot = Vector2.zero;
    public LayerMask mask;
    public GameObject projectile;
    private LineRenderer line;
    private Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        line = GetComponent<LineRenderer>();
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Use a raycast to point the player towards the mouse pointer
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, ray.direction, out hit, Mathf.Infinity, mask)) {
            // Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.yellow, 0, false);

            // Draws a visual line that shows where the player shoots
            line.SetPosition(0, transform.position);
            Vector3 vec = new Vector3(transform.forward.x * hit.distance + transform.position.x, transform.position.y, transform.forward.z * hit.distance + transform.position.z);
            line.SetPosition(1, vec);

            // Use a raycast to point the player towards the mouse pointer
            float turn = Mathf.Atan2(-ray.origin.z + transform.position.z, ray.origin.x - transform.position.x) * Mathf.Rad2Deg + 90.0f;
            body.MoveRotation(Quaternion.Euler(0, turn, 0));
        }
    }
}
