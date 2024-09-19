using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject player;
    private Vector3 camPosition;
    [SerializeField] private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        camPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = player.transform.position + camPosition;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);   
    }
}
