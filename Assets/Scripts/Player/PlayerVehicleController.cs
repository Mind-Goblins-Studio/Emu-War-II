using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerVehicleController : MonoBehaviour
{
    // Vehicle
    [SerializeField] private Transform vehicle;
    private VehicleController vehicleController;
    
    // Player Object
    [SerializeField] private GameObject playerModel;
    private Collider playerCollider;
    private PlayerMove playerMove;
    private PlayerResources playerResources;

    [SerializeField] private LayerMask everything;
    
    // Get In Radius
    [SerializeField] private SphereCollider entryRadius;

    [SerializeField] private Vector3 playerCameraOffset;
    [SerializeField] private Vector3 vehicleCameraOffset;

    [SerializeField] private Camera camera;

    private bool isInsideVehicle = false;
    private float raycastDistance = 2.0f;

    [SerializeField] private UnityEvent enterTrigger;
    [SerializeField] private UnityEvent exitTrigger;

    // Audio
    public AudioSource truckStart;
    public AudioSource truckStop;

    // Start
    private void Start()
    {
        // player components
        playerCollider = GetComponent<Collider>();
        playerResources = GetComponent<PlayerResources>();
        playerMove = GetComponent<PlayerMove>();
        
        vehicleController = vehicle.GetComponent<VehicleController>();
        camera.GetComponent<CameraFollow>().SetTarget(this.transform);
        camera.GetComponent<CameraFollow>().SetOffset(playerCameraOffset);
    }

    private void Update()
    {
        if (isInsideVehicle)
        {
            transform.position = vehicleController.GetOutPosition();
        }
        
        // E Press
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (InVehicleRange() && !isInsideVehicle)
            {
                // Enter vehicle if nearby and outside the vehicle
                EnterVehicle();
            }
            else if (isInsideVehicle)
            {
                // Exit Vehicle
                ExitVehicle();
            }
        }
    }
    
    public void EnterVehicle()
    {
        enterTrigger.Invoke();
        Debug.Log("get in");

        // Toggle player visibility & hitboxes
        playerModel.SetActive(false);
        setScripts(false);
        camera.GetComponent<CameraFollow>().SetTarget(vehicle);
        camera.GetComponent<CameraFollow>().SetOffset(vehicleCameraOffset);
        
        // Set Player to 0
        Debug.Log("player is at zero");
        transform.position = Vector3.zero;

        // Toggle vehicle control
        vehicleController.ToggleVehicleControl(true);
        isInsideVehicle = true;

        // Audio
        truckStart.Play();

        // Particle System
        vehicleController.ToggleParticleSystem(true);
    }

    public void ExitVehicle()
    {
        exitTrigger.Invoke();
        Debug.Log("get out");

        // Query Vehicle Controller for Valid Position
        Vector3 outPosition = vehicleController.GetOutPosition();
        if (outPosition == Vector3.zero)
        {
            Debug.Log("get out failed");
            return;
        }
        
        // Outside Vehicle
        isInsideVehicle = false;
        
        // Set Player to Out Position
        while (transform.position != outPosition)
        {
            transform.position = outPosition;
        }

        // Toggle player visibility & hitboxes
        playerModel.SetActive(true);
        setScripts(true);
        camera.GetComponent<CameraFollow>().SetTarget(this.transform);
        camera.GetComponent<CameraFollow>().SetOffset(playerCameraOffset);

        // Toggle vehicle control
        vehicleController.ToggleVehicleControl(false);

        // Audio
        truckStart.Stop();
        truckStop.Play();

        // Particle System
        vehicleController.ToggleParticleSystem(false);
    }

    private void setScripts(bool status)
    {
        playerMove.enabled = status;
        playerResources.enabled = status;
        playerCollider.enabled = status;
        
        // make collider exclude everything if status is false
        if (!status)
        {
            playerCollider.excludeLayers = everything;
        }
        else
        {
            playerCollider.excludeLayers = 0;
        }
    }
    
    public bool IsInsideVehicle()
    {
        return isInsideVehicle;
    }
    
    public bool InVehicleRange()
    {
        // Check if the player is within the vehicle's entry radius
        return entryRadius.bounds.Contains(this.transform.position);
    }
}
