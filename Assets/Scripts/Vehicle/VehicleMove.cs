using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleMove : MonoBehaviour
{
    public float maxSpeed;
    public float acceleration;
    public float brakingForce;
    public float reverseSpeed;
    public float turnSpeed;
    public float downForce;
    
    private Rigidbody rb;
    private bool isAccelerating;
    private bool isGrounded = true;
    private int currCollisions = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = Vector3.down * 0.5f; // Adjust the center of mass to be slightly lower
    }

    private void Update()
    {
        float inputVertical = Input.GetAxis("Vertical");
        float inputHorizontal = Input.GetAxis("Horizontal");
        bool isBraking = Input.GetKey(KeyCode.Space);

        Vector3 forwardForce = transform.forward * inputVertical * acceleration;
        
        // use collision count to determine if grounded
        if (currCollisions == 0) {
            isGrounded = false;
        } else {
            isGrounded = true;
        }
        //Debug.Log(isGrounded);
        
        // Apply braking or reverse force
        if (isBraking && isGrounded) {
            rb.AddForce(-rb.velocity.normalized * brakingForce * Time.deltaTime, ForceMode.Acceleration);
        } else {
            // not braking, allow acceleration
            if (isGrounded && rb.velocity.magnitude < maxSpeed) {
                if (inputVertical < 0) {
                    rb.AddForce(forwardForce * reverseSpeed * Time.deltaTime, ForceMode.Acceleration);
                } else {
                    rb.AddForce(forwardForce * Time.deltaTime, ForceMode.Acceleration);
                }
            } else {
                // extra downforce when airborne
                rb.AddForce(-transform.up * downForce * rb.velocity.magnitude * Time.deltaTime);
            }
            // steering
            if (inputVertical < 0) {
                // Invert turning controls when reversing
                inputHorizontal *= -1; 
            }
            Quaternion deltaRotation = Quaternion.Euler(Vector3.up * inputHorizontal * turnSpeed * (float)Math.Sqrt(rb.velocity.magnitude) * Time.deltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }

        // Apply downforce to improve stability
        rb.AddForce(-transform.up * downForce * rb.velocity.magnitude * Time.deltaTime);
        
        //Debug.DrawRay(transform.position, rb.velocity.normalized * 10);
        //Debug.DrawRay(transform.position, transform.forward * 10, Color.blue);
        
    }
    
    
    // count number of collisions enter/exits that have happened
    private void OnCollisionEnter(Collision collision) {
        currCollisions += 1;
    }
    private void OnCollisionExit(Collision collision) {
        currCollisions -= 1;
    }

}
