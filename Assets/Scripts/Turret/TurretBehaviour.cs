using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class TurretBehaviour : MonoBehaviour
{

    private bool isActive = false;
    
    // Rotation
    [SerializeField] private Transform target;
    private Vector3 targetTruePosition;
    
    [SerializeField] private Transform turretHead;
    
    // Enemy Detection
    [SerializeField] private string enemyTag = "Emu"; // tag
    
    // Shooting Attributes
    [SerializeField] private Transform turretEnd;
    [SerializeField] private GameObject bullet;
    private float lastShotTime;
    public float bulletSizeScale = 1f;
    
    // Upgradeable Attributes
    public float shootCooldown = 1f;
    public float detectionRadius = 5f; // Radius of detection area
    public float damage = 1f;
    
    // Audio
    public AudioSource shotSound;
    
    private void Update()
    {
        // Don't do anything if turret is not active
        if (!isActive)
        {
            return;
        }
        
        // Check for enemies with the specified tag in the detection radius
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, detectionRadius);
        
        float shortestDistance = Mathf.Infinity;
        Transform nearestEnemy = null;

        foreach (Collider enemyCollider in hitEnemies)
        {
            if (enemyCollider.CompareTag(enemyTag))
            {
                // Assuming the enemy script has a 'distanceToEnd' variable
                EmuMovement enemyScript = enemyCollider.GetComponent<EmuMovement>();

                if (enemyScript != null && enemyScript.getTotalDistanceRemaining() < shortestDistance)
                {
                    shortestDistance = enemyScript.getTotalDistanceRemaining();
                    nearestEnemy = enemyCollider.transform;
                }
            }
        }

        // Update the turret's target enemy
        target = nearestEnemy;
        
        // Shoot at enemy if enemy found
        if (target != null)
        {
            targetTruePosition = target.GetChild(1).position;
            // Check if enough time has passed since the last shot
            LookAt(targetTruePosition);
            if (Time.time - lastShotTime >= shootCooldown)
            {
                // Now you can aim and shoot at the targetEnemy if it's not null
                Shoot(); // Implement your shooting logic here
                lastShotTime = Time.time; // Update the last shot time
            }
        }
    }

    // Call this function to make the turret head look at a specific position
    public void LookAt(Vector3 targetPosition)
    {
        Vector3 directionToTarget = targetPosition - turretHead.position;
        Quaternion rotation = Quaternion.LookRotation(directionToTarget, Vector3.up);

        turretHead.rotation = rotation;
    }
    
    // Shoot function
    public void Shoot()
    {
        GameObject newBullet = Instantiate(bullet);
        newBullet.GetComponent<BulletBehaviour>().damage = damage;
        newBullet.GetComponent<BulletBehaviour>().speed = detectionRadius * 6;
        newBullet.transform.localScale = newBullet.transform.localScale * bulletSizeScale;
        newBullet.transform.position = turretEnd.position;
        newBullet.GetComponent<BulletBehaviour>().startBullet(targetTruePosition);
        shotSound.Play(); // turret fire
    }
    
    // Set if turret can shoot or not
    public void SetActive(bool state)
    {
        isActive = state;
    }
    
    // Display range in editor
    private void OnDrawGizmosSelected()
    {
        // Display the detection radius in the Unity editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
