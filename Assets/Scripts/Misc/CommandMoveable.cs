using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandMoveable : MonoBehaviour
{
    private bool isMoving;
    private Vector3 destination;
    private float currentSpeed;
    private float maxDistance;
    private float distanceTravelled;
    
    // Update
    void Update()
    {
        if (isMoving)
        {
            // Move towards destination
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, 
                destination, currentSpeed * Time.deltaTime);
            distanceTravelled += currentSpeed * Time.deltaTime;
            
            // Check if reached destination
            if (distanceTravelled >= maxDistance)
            {
                isMoving = false;
                distanceTravelled = 0;
                this.transform.localPosition = destination;
            }
        }
    }
    
    // Command
    public void CommandMove(Vector3 direction, float speed)
    {
        // Get destination
        currentSpeed = speed;
        destination = transform.localPosition + direction;
        isMoving = true;
        maxDistance = Vector3.Distance(transform.localPosition, destination);
    }

    public void MoveOffScreenToRight(float speed)
    {
        CommandMove(Vector3.right * 1000, speed);
    }
}
