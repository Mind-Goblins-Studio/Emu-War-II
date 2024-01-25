using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningTutorial : AbstractTutorial
{
    // Tooltips
    [SerializeField] private Fading drivingTooltip;
    [SerializeField] private Fading exitUteTooltip;
    [SerializeField] private Fading miningTooltip;
    [SerializeField] private Fading whileMiningTooltip;
    [SerializeField] private Fading upgradeTabTooltip;
    [SerializeField] private Fading upgradeUteTooltip;
    
    // Progress Guards
    private bool startGuard;
    private bool playerHasExitedVehicle;
    private bool playerInRange;
    private bool playerHasMined;
    private bool playerHasMined40;
    private bool playerHasMined50;
    private bool playerHasMined100;
    private bool resourcesIsGone;
    private bool tabPressed;
    private bool timerStarted;
    private float timer = 0f;
    
    // World Objects
    [SerializeField] private Arrow arrow;
    [SerializeField] private GameObject tutorialScrapManager;
    [SerializeField] private GameObject ute;
    [SerializeField] private GameObject player;

    // Update is called once per frame
    void Update()
    {
        if (isStarted && !isFinished)
        {   
            // Start Step
            if (!startGuard)
            {
                arrow.sourceObject = ute.transform;
                arrow.targetObject = tutorialScrapManager.transform;
                arrow.Show();
                arrow.SetDistance(7.0f);
                tutorialScrapManager.gameObject.SetActive(true);
                startGuard = true;
                drivingTooltip.FadeIn();
            }
            else
            {
                // Player is near scrap
                if (player.GetComponent<PlayerResources>().InRangeVal(22.5f) && !playerHasExitedVehicle)
                {
                    exitUteTooltip.FadeIn();
                    drivingTooltip.FadeOut();
                }
                
                // Player exits ute for the first time, reset arrow
                if (!player.GetComponent<PlayerVehicleController>().IsInsideVehicle() 
                    && !playerHasExitedVehicle
                    && !resourcesIsGone)
                {
                    exitUteTooltip.FadeOut();
                    playerHasExitedVehicle = true;
                    arrow.Show();
                    arrow.sourceObject = player.transform;
                    arrow.SetDistance(3.0f);
                }
                
                // Edge case, player gets back in vehicle
                if (playerHasExitedVehicle 
                    && player.GetComponent<PlayerVehicleController>().IsInsideVehicle() 
                                           
                    && !resourcesIsGone)
                {
                    arrow.SetDistance(7.0f);
                    arrow.sourceObject = ute.transform;
                    arrow.Show();
                    playerHasExitedVehicle = false;
                }
                
                // Player enters scrap radius
                if (playerHasExitedVehicle && player.GetComponent<PlayerResources>().InRange() && !playerInRange)
                {
                    drivingTooltip.FadeOut();
                    playerInRange = true;
                    miningTooltip.FadeIn();
                    arrow.Hide();
                    Debug.Log("Player is in range");
                }
            
                // Player starts mining
                if (player.GetComponent<PlayerResources>().IsMining() && !playerHasMined)
                {
                    Debug.Log("Player is mining");
                    playerHasMined = true;
                }
                    
                // Player gets to 40 resources
                if (!playerHasMined40 && player.GetComponent<PlayerResources>().resourceCount >= 40)
                {
                    playerHasMined40 = true;
                    miningTooltip.FadeOut();
                }
                
                // Player gets to 50 resources
                if (!playerHasMined50 && player.GetComponent<PlayerResources>().resourceCount >= 50)
                {
                    playerHasMined50 = true;
                    whileMiningTooltip.FadeIn();
                }
                
                // Player gets to 100 resources
                if (!playerHasMined100 && player.GetComponent<PlayerResources>().resourceCount >= 100)
                {
                    playerHasMined100 = true;
                    arrow.Hide();
                    whileMiningTooltip.FadeOut();
                    upgradeTabTooltip.FadeIn();
                    resourcesIsGone = true; 
                }
                
                // Player presses TAB once
                if (!tabPressed && playerHasMined100 && Input.GetKeyDown(KeyCode.Tab))
                {
                    tabPressed = true;
                    upgradeTabTooltip.FadeOut();
                    upgradeUteTooltip.FadeIn();
                    timerStarted = true;
                }
                
                // Timer for upgradeUteTooltip
                if (timerStarted)
                {
                    timer += Time.deltaTime;
                    
                    if (timer >= 8.0f)
                    {
                        upgradeUteTooltip.FadeOut();
                    }
                    
                    // End of Timer, End Tutorial
                    if (timer >= 10.0f)
                    {
                        upgradeUteTooltip.gameObject.SetActive(false);
                        arrow.Hide();
                        timerStarted = false;
                        isFinished = true;
                    }
                }
            }
        }
    }
}
