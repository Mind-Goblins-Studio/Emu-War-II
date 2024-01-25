using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Collider collider;
    
    private bool isOpen = true;
    
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
    }
    
    void Update()
    {
        Debug.Log(this.gameObject.name + " is open: " + isOpen);
    }
    
    // When another object enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            return;
        }
        isOpen = false;
    }
    
    // When another object exits the trigger
    private void OnTriggerExit(Collider other)
    {
        isOpen = true;
    }
    
    public bool IsOpen()
    {
        return isOpen;
    }
    
    public Vector3 position
    {
        get { return this.gameObject.transform.position; }
    }
}
