using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GUIController : MonoBehaviour
{
    // Raw Sources
    [SerializeField] private GameObject hq;
    [SerializeField] private GameObject player;

    // Text Outputs
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI  healthText;
    [SerializeField] private TextMeshProUGUI  resourceText;

    // GUI Element Storage
    private float baseHealth;
    private float resources;
    
    [SerializeField] private GlobalGameTimer GlobalGameTimer;

    // Start
    public void Start()
    {
        // Initialise Values
        baseHealth = hq.GetComponent<HQController>().health;
        resources = player.GetComponent<PlayerResources>().resourceCount;
        healthText.text = "100%";
        resourceText.text = "0";
    }

    // Update 
    public void Update()
    {
        // Update timer
        timerText.text = TimeConversion(GlobalGameTimer.getTimeRemaining());
        
        // Update resources
        resourceText.text = ((int)player.GetComponent<PlayerResources>().resourceCount).ToString();
    }

    private string TimeConversion(float timeInSeconds)
    {
        if (timeInSeconds <= 0)
        {
            return "0:00";
        }
        int minutes = (int)(timeInSeconds / 60);
        float secondsFloat = timeInSeconds % 60;
        int seconds = (int)secondsFloat;
        return $"{minutes:D1}:{seconds:D2}";
    }

    public void UpdateHealth(float currentHealth)
    {
        Debug.Log(currentHealth / baseHealth);
        healthText.text = $"{(int)(100 * currentHealth / baseHealth)}%";
    }

    public void UpdateResources(float increment)
    {
        resources += increment;
        resourceText.text = ((int)resources).ToString();
    }
    
}
