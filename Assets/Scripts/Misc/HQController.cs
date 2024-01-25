using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HQController : MonoBehaviour
{
    [SerializeField] private GlobalGameTimer GlobalGameTimer;
    
    [SerializeField] private GUIController gui;
    [SerializeField] private GameObject smoke80;
    [SerializeField] private GameObject smoke55;
    [SerializeField] private GameObject smoke30;

    private GameObject currentSmoke;

    private bool lessThan80 = false;
    private bool lessThan55 = false;
    private bool lessThan30 = false;

    [SerializeField] private List<GameObject> damageStates = new List<GameObject>();
    private int damageStateIdx = 0;
    
    private float baseHealth;
    public float health;

    private Vector3 smokeOffset = new Vector3(0.0f, 12.35f, 0.0f);
    
    
    // Start is called before the first frame update
    void Start()
    {
        baseHealth = health;
    }
    
    public void loseHealth(float damage)
    {
        // lose health, min of zero
        health -= damage;
        if (health < 0)
        {
            health = 0;
        }
        
        gui.UpdateHealth(health);
        Debug.Log("Current Health: " + health);

        if (health <= 80 && !lessThan80)
        {
            currentSmoke = Instantiate(smoke80, transform.position + smokeOffset, Quaternion.Euler(-90f, 0f, 0f));
            currentSmoke.GetComponent<ParticleSystem>().Play();
            
            // model switch
            damageStates[damageStateIdx].SetActive(false);
            damageStateIdx++;
            damageStates[damageStateIdx].SetActive(true);

            lessThan80 = true;
        }
        else if (health <= 55 && !lessThan55)
        {
            // Stop the previous smoke Particle System (if it exists)
            StopCurrentSmoke();

            // Instantiate the smoke prefab and play its Particle System
            currentSmoke = Instantiate(smoke55, transform.position + smokeOffset, Quaternion.Euler(-90f, 0f, 0f));
            currentSmoke.GetComponent<ParticleSystem>().Play();
            
            // model switch
            damageStates[damageStateIdx].SetActive(false);
            damageStateIdx++;
            damageStates[damageStateIdx].SetActive(true);

            lessThan55 = true; // Set the flag to prevent multiple instantiations
        }
        // Check if health crosses 30 threshold
        else if (health <= 30 && !lessThan30)
        {
            StopCurrentSmoke();

            // Instantiate the smoke prefab and play its Particle System
            currentSmoke = Instantiate(smoke30, transform.position + smokeOffset, Quaternion.Euler(-90f, 0f, 0f));
            currentSmoke.GetComponent<ParticleSystem>().Play();

            // model switch
            damageStates[damageStateIdx].SetActive(false);
            damageStateIdx++;
            damageStates[damageStateIdx].SetActive(true);

            lessThan30 = true;
        }
        
        if (health <= 0)
        {
            Dead();
        }
    }

    [ContextMenu("Dead")]
    public void Dead()
    {
        Debug.Log("Game over");
        StopCurrentSmoke();
        GlobalGameTimer.EndGameLose();
    }

    private void StopCurrentSmoke()
    {
        // Check if there is a current smoke and stop its Particle System
        if (currentSmoke != null)
        {
            ParticleSystem particleSystem = currentSmoke.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                particleSystem.Stop();
            }
        }
    }
    
    public float GetMaxHealth()
    {
        return baseHealth;
    }
}
