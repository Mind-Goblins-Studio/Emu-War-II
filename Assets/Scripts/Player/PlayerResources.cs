using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    // Player Mining Attributes
    public float resourceCount = 0;
    public float mineValue = 10;
    
    // ResourceDeposit Detection
    [SerializeField] private float interactionRadius = 2.0f; // Radius
    [SerializeField] private string resourceTag = "Resource"; // Resource Tag
    private ScrapDepositController resourceDeposit; // Resource Deposit

    // Cooldown Attributes
    public float collectionCooldown; // Cooldown duration in seconds
    private float lastCollectionTime; // Time of last resource collection
    
    // Ute
    [SerializeField] private Transform ute;
    [SerializeField] private float uteUpgradeRadius = 10.0f; // Radius
    
    // Audio
    public AudioSource miningSound;
    public AudioSource breakSound;
    
    // Cheat Code
    public bool CheatsOn = false;
    
    public void Start() {
        // default cheats to off, require manual activation via component
        CheatsOn = false;
    }

    public void Update()
    {
        // REMOVE AT SOME POINT (FREE MONEY)
        if (CheatsOn && Input.GetKeyDown(KeyCode.N))
        {
            resourceCount += 20;
        }
        
        
        if (Input.GetKey(KeyCode.Space) && InRange())
        {
            if (resourceDeposit != null)
            {
                // start mining sound if it isn't already playing
                if (!miningSound.isPlaying) {
                    miningSound.Play(); 
                }
                animator.SetBool("Mining", true);
                
                // Look at Resource
                this.transform.LookAt(new Vector3(resourceDeposit.transform.position.x, 
                    transform.position.y, 
                    resourceDeposit.transform.position.z));
                
                if (Time.time - lastCollectionTime >= collectionCooldown
                    && resourceDeposit.resourcesLeft > 0)
                {
                    // Mining
                    lastCollectionTime = Time.time; // Update the last collection time
                
                    float value = resourceDeposit.resourceDecrement(mineValue);
                    resourceCount += value;
                    Debug.Log("Resource collected! Total resources: " + resourceCount);
                    breakSound.Play();
                }
            }
        }
        else
        {
            animator.SetBool("Mining", false);
            miningSound.Stop();
        }
    }
    
    // Check if player is in range of a resource deposit
    public bool InRange()
    {
        // Find all colliders with the specified tag within the interaction radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRadius);

        // Loop through the colliders and check for resource deposits
        foreach (Collider collider in colliders)
        {
            // Check if the collider's game object has the specified tag
            if (collider.CompareTag(resourceTag) &&
                collider.gameObject.GetComponent<ScrapDepositController>().resourcesLeft > 0 &&
                Vector3.Distance(this.transform.position, collider.transform.position) <= interactionRadius)
            {
                resourceDeposit = collider.gameObject.GetComponent<ScrapDepositController>();
                return true;
            }
        }
        return false;
    }
    
    // Check player range to nearest scrap
    public bool InRangeVal(float radius)
    {
        // Find all colliders with the specified tag within the interaction radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        // Loop through the colliders and check for resource deposits
        foreach (Collider collider in colliders)
        {
            // Check if the collider's game object has the specified tag
            if (collider.CompareTag(resourceTag) &&
                collider.gameObject.GetComponent<ScrapDepositController>().resourcesLeft > 0 &&
                Vector3.Distance(this.transform.position, collider.transform.position) <= radius)
            {
                return true;
            }
        }
        return false;
    }
    
    
    // Check if player is in range of the ute
    public bool InUteRange()
    {
        if (Vector3.Distance(transform.position, ute.position) <= uteUpgradeRadius)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public bool IsMining()
    {
        return animator.GetBool("Mining");
    }
}