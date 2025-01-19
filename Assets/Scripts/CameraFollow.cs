using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;
    private Vector3 offset;

    void LateUpdate()
    {
        if (player != null) {
            Vector3 targetPosition = player.transform.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }

    public void SetTarget(GameObject Player)
    {
        player = Player;
        offset = transform.position - player.transform.position;
    }
}
