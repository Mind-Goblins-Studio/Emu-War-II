using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float speed; // Speed of the bullet
    public float damage; // Damage value of the bullet
    public float lifetime = 3f; // How long the bullet lives before destroying itself

    private Vector3 target;
    

    private void OnCollisionEnter(Collision collision)
    {
        // Destroy the bullet on collision
        if (collision.gameObject.CompareTag("Ute"))
        {
            return;
        }
        Destroy(gameObject);
    }

    public void startBullet(Vector3 target)
    {
        // Calculate the direction to the target
        Vector3 directionToTarget = (target - transform.position).normalized;

        // Apply velocity to the Rigidbody to move towards the target
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = directionToTarget * speed;
        
        // Destroy the bullet after the specified lifetime
        Destroy(gameObject, lifetime);
    }
}
