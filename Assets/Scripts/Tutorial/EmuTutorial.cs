using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmuTutorial : AbstractTutorial
{
    // Tooltips
    [SerializeField] private Fading emuSpawnTooltip;
    [SerializeField] private Fading turretInfoTooltip;
    [SerializeField] private Fading multitaskTooltip;
    [SerializeField] private Fading declarationTooltip;
    
    // Progress Guards
    private bool startGuard;
    private bool turretInfoAppeared;
    private bool playerIsNearEmus;
    private bool playerHasKilledEmus;
    private bool emusAreDead;
    
    // World Objects
    [SerializeField] private SpawnerManagerScript tutorialEmuSpawner;
    [SerializeField] private Transform player;
    
    // Timer for emu radius
    private float timer = 0f;
    private float timerMax = 1f;
    
    // Timer for ending
    private float endTimer = 0f;
    private float endTimerMax = 3.5f;
    private bool endTimerStarted = false;
    
    private float declarationTimer = 0f;
    private float declarationTimerMax = 5.0f;
    private bool declarationTimerStarted = false;
    
    private float finalTimer = 0f;
    private float finalTimerMax = 2.0f;
    private bool finalTimerStarted = false;
    
    private void Update()
    {
        if (isStarted && !isFinished)
        {
            if (!startGuard)
            {
                StartCoroutine(tutorialEmuSpawner.SpawnWave(tutorialEmuSpawner.waves[0]));
                emuSpawnTooltip.FadeIn();
                startGuard = true;
            }
            else
            {   
                // Once in range of emus, phase out emu tooltip and phase in turret tooltip
                if (playerIsNearEmus && !turretInfoAppeared)
                {
                    emuSpawnTooltip.FadeOut();
                    turretInfoTooltip.FadeIn();
                    turretInfoAppeared = true;
                }
                
                if (emusAreDead && !playerHasKilledEmus)
                {
                    turretInfoTooltip.FadeOut();
                    multitaskTooltip.FadeIn();
                    playerHasKilledEmus = true;
                    endTimerStarted = true;
                }
                
                // At end of timer, declare
                if (endTimerStarted)
                {
                    endTimer += Time.deltaTime;
                    if (endTimer >= endTimerMax)
                    {
                        multitaskTooltip.FadeOut();
                        declarationTooltip.FadeIn();
                        endTimerStarted = false;
                        declarationTimerStarted = true;
                        // war begins sound
                        transform.parent.GetComponent<AudioSource>().Play();
                    }
                }
                
                // At end of declaration timer, end tutorial
                if (declarationTimerStarted)
                {
                    declarationTimer += Time.deltaTime;
                    if (declarationTimer >= declarationTimerMax)
                    {
                        declarationTooltip.FadeOut();
                        declarationTimerStarted = false;
                        finalTimerStarted = true;
                        // stop warning sound
                        transform.parent.GetComponent<AudioSource>().loop = false;
                    }
                }
                
                if (finalTimerStarted)
                {
                    finalTimer += Time.deltaTime;
                    if (finalTimer >= finalTimerMax)
                    {
                        isFinished = true;
                    }
                }
            }
        }
    }
    
    // Check if player is near emus (run every second)
    private void FixedUpdate()
    {
        // Emu Detection
        if (!playerIsNearEmus && isStarted)
        {
            timer += Time.deltaTime;
            if (timer >= timerMax)
            {
                timer = 0f;
                // Check for emus in radius
                GameObject[] emuObjects = GameObject.FindGameObjectsWithTag("Emu");

                // Loop through the found Emu objects
                foreach (GameObject emuObject in emuObjects)
                {
                    // Check if the emu is within the radius
                    if (Vector3.Distance(emuObject.transform.position, player.position) < 20f)
                    {
                        playerIsNearEmus = true;
                        return;
                    }
                }
            }
        }
        
        // Emu Kill Detection
        if (!emusAreDead && isStarted)
        {
            timer += Time.deltaTime;
            if (timer >= timerMax)
            {
                timer = 0f;
                // Check for emus in radius
                GameObject[] emuObjects = GameObject.FindGameObjectsWithTag("Emu");
                if (emuObjects.Length == 0)
                {
                    emusAreDead = true;
                    return;
                }
            }
        }
    }  
}
