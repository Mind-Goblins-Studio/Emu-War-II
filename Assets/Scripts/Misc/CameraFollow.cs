using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target; 
    [SerializeField] Vector3 offset;
    [SerializeField] private float zOffset;

    private void Update()
    {
        if (target != null)
        {
            // calculate the desired position
            Vector3 desiredPosition = target.position + offset;

            // set the camera position to the desired position
            transform.position = desiredPosition;

            // look at the player's position
            transform.LookAt(target.position + Vector3.back*zOffset);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetOffset(Vector3 newOffset)
    {
        offset = newOffset;
    }
}
