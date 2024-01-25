using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BossBehaviour : MonoBehaviour
{
    // Health GUI
    public GameObject healthGUI;
    public Slider healthSlider;
    
    // Emu Attributes
    private EmuBulletCollision emuBulletCollision;
    private float maxHealth;

    private bool hasStarted = false;


    private void Start()
    {
        // Locate EmuBulletCollision script and Health
        emuBulletCollision = this.GetComponent<EmuBulletCollision>();
        
        // Setup Parameters
        maxHealth = emuBulletCollision.health;
    }

    public void StartBoss()
    {
        healthGUI.SetActive(true);
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
        hasStarted = true;
        
        // TO DO - announce + music change
        // TEMP Audio
        transform.parent.GetComponents<AudioSource>()[0].Play();
    }
    
    private void Update()
    {
        if (hasStarted)
        {
            UpdateHealth(emuBulletCollision.health); 
        }
    }
    
    public void UpdateHealth(float value)
    {
        healthSlider.value = emuBulletCollision.health;
        
        if (emuBulletCollision.health <= 0)
        {
            healthGUI.SetActive(false);
        }
    }
}
