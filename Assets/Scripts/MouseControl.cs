using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    private Camera screen;
    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private Vector2 hotSpot = Vector2.zero;
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;
    public LayerMask mask;
    public GameObject projectile;
    private LineRenderer line;
    private Rigidbody body;
    private Vector3 lineOrigin;
    private Ray ray;

    // Start is called before the first frame update
    void Start()
    {
        screen = GameObject.FindGameObjectWithTag("Screen").GetComponent<Camera>();
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        line = GetComponent<LineRenderer>();
        body = GetComponent<Rigidbody>();
        lineOrigin = line.GetPosition(0);
    }

    // Update is called once per frame
    void Update()
    {
        // Use a raycast to point the player towards the mouse pointer
        ray = screen.ScreenPointToRay(Input.mousePosition);

        // Draws a visual line that shows where the player shoots
        line.SetPosition(0, transform.position + lineOrigin);
        Vector3 vec = new Vector3(transform.forward.x * 15 + transform.position.x, transform.position.y + lineOrigin.y, transform.forward.z * 15 + transform.position.z);
        line.SetPosition(1, vec);
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(screen.transform.position, ray.direction, out hit, Mathf.Infinity, mask)) {
            // Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.yellow, 0, false);

            // Find how much to rotate the player in order to look at crosshair
            float turn = Mathf.Atan2(-ray.origin.z + transform.position.z, ray.origin.x - transform.position.x) * Mathf.Rad2Deg + 90.0f - 5.0f;
            body.MoveRotation(Quaternion.Euler(0, turn, 0));
        }
    }
}
