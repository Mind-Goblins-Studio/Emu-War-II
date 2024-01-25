using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyDropController : MonoBehaviour
{
    // Falling
    [SerializeField] private float fallSpeed = 1;
    private bool isGrounded = false;
    private float GROUND_HEIGHT = 1.05f;
    
    // Collection
    [SerializeField] private GameObject destroyParticles;
    [SerializeField] private float value = 100;
    
    // Icon
    private bool isLinked = false;
    private GameObject icon;
    
    
    // Update is called once per frame
    void Update()
    {
        if (!isGrounded)
        {
            // Fall down
            transform.position += Vector3.down * (fallSpeed * Time.deltaTime);
            
            // Icon to Mimic
            if (isLinked)
            {
                icon.transform.localPosition = MiniMapController.ConvertPosition(transform.position);
                icon.transform.localScale *= 1.0f + (Time.deltaTime * 0.1f);
            }
            
            // Check if we are grounded
            if (transform.position.y <= GROUND_HEIGHT)
            {
                isGrounded = true;
            }
        }
    }
    
    // Collision to detect pickup
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Ute"))
        {
            OnPickup();
        }
    }
    
    // Called when the supply drop is picked up
    private void OnPickup()
    {
        // Audio, play SupplyDropSpawner's 2nd audio source, the collection sound
        transform.parent.GetComponents<AudioSource>()[1].Play();
        // Create particles
        GameObject particles = Instantiate(destroyParticles, 
            transform.position + 0.5f*Vector3.up, Quaternion.identity);
        particles.GetComponent<ParticleSystem>().Play();
        
        // Inform Spawner to add gold
        this.transform.parent.GetComponent<SupplyDropSpawner>().AddResource(value);
        
        // Destroy the supply drop
        Unlink();
        Destroy(gameObject);
    }
    
    // Link Icon
    public void LinkIcon(GameObject newIcon)
    {
        isLinked = true;
        icon = newIcon;
        icon.transform.localPosition = MiniMapController.ConvertPosition(transform.position);
    }

    public bool IsLinked()
    {
        return isLinked;
    }

    public void Unlink()
    {
        if (isLinked)
        {
            isLinked = false;
            Destroy(icon);
        }
    }
}
