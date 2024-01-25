using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [SerializeField] private float aboveDist = 8f;
    
    private bool isControlled = false;
    private VehicleMove vehicleMovement;
    public GameObject turret;
    
    public DoorController leftDoor;
    public DoorController rightDoor;

    [SerializeField] private ParticleSystem particles;

    [SerializeField] private Transform radiusIndicator;
    private bool radiusIndicatorActive = true;
    [SerializeField] private float radiusScale = 1.0f;

    private bool turretActiveAlways = false;

    private void Start()
    {
        vehicleMovement = GetComponent<VehicleMove>();
        ToggleParticleSystem(false);
    }

    public void ToggleVehicleControl(bool toggle)
    {
        isControlled = toggle;

        if (!turretActiveAlways)
        {
            turret.GetComponent<TurretBehaviour>().SetActive(!toggle);
        }

        if (vehicleMovement != null)
        {
            vehicleMovement.enabled = isControlled;
        }
    }

    public Vector3 GetOutPosition()
    {
        // Get z positions
        float zLeft = leftDoor.position.z;
        float zRight = rightDoor.position.z;
        
        // Check if right door is open
        if ((rightDoor.IsOpen() && zRight < zLeft) || !leftDoor.IsOpen())
        {
            return rightDoor.position;
        }
        else if ((leftDoor.IsOpen() && zLeft < zRight) || !rightDoor.IsOpen())
        {
            return leftDoor.position;
        }
        else
        {
            return this.transform.position + Vector3.up * aboveDist;
        }
    }

    public void EnableAutoShoot()
    {
        turretActiveAlways = true;
        turret.GetComponent<TurretBehaviour>().SetActive(true);
    }

    public void ToggleParticleSystem(bool toggle)
    {
        var emissionModule = particles.emission;
        emissionModule.enabled = toggle;
    }
    
    // Fixed Update
    private void FixedUpdate() {
        // Keep y level constant
        if (this.transform.position.y < 0.04 || this.transform.position.y > 0.08) {
            this.transform.position = new Vector3(this.transform.position.x, 0.006f, this.transform.position.z);
        }
    }


    public void UpdateDetectionRadius()
    {
        radiusIndicator.localScale = new Vector3(
            turret.GetComponent<TurretBehaviour>().detectionRadius * radiusScale,
            radiusIndicator.localScale.y,
            turret.GetComponent<TurretBehaviour>().detectionRadius * radiusScale);
    }
}
