using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmuBulletCollision : MonoBehaviour {
    
    public float health;

    [SerializeField] private GameObject hitParticles;
    
    private void OnCollisionEnter(Collision collision)
    {
        // check for bullet collision
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // use this to access bullet damage
            GameObject bullet = collision.gameObject;
            float damage = bullet.GetComponent<BulletBehaviour>().damage;
            TakeDamage(damage);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
            
        // spawn and play particles
        GameObject particles = Instantiate(hitParticles, 
            transform.position + Vector3.up, Quaternion.identity);
        particles.GetComponent<ParticleSystem>().Play();

        // check if emu is dead
        if (health <= 0) {
            this.gameObject.GetComponent<EmuMovement>().Unlink();
            
            // if boss remove gui
            if (this.gameObject.GetComponent<BossBehaviour>() != null)
            {
                this.gameObject.GetComponent<BossBehaviour>().healthGUI.SetActive(false);
            }
            
            Destroy(gameObject);
            
            // NOTE: tutorial emus got no sound yet
            RandomAudioScript audioScript = transform.parent.GetComponent<RandomAudioScript>();
            if (audioScript != null) {
                audioScript.PlayRandomSound();
            }
        }
    }
}
