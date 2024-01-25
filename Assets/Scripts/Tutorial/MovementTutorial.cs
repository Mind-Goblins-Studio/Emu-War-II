using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MovementTutorial : AbstractTutorial
{   
    // Tooltips
    [SerializeField] private Fading wasdTooltip;
    [SerializeField] private float wasdTooltipDelay = 3.0f;
    
    [SerializeField] private Arrow arrowToUte;
    
    [SerializeField] private Fading eTooltip;

    // Progress Trackers
    private bool startGuard = false;
    private bool playerHasMoved = false;
    private bool wasdIsGone = false;
    private bool enteredUteRange = false;
    
    // Timers
    private float wasdTimer = 0f;
    private float wasdTimerMax = 2f;
    private bool wasdTimerStarted = false;
    
    // Game Objects
    [SerializeField] private GameObject player;
    
    
    // Update
    private void Update()
    {
        if (isStarted && !isFinished)
        {
            // Show WASD Tooltip
            if (!startGuard)
            {
                wasdTooltip.FadeIn();
                startGuard = true;
            }
            
            // Start Guard Block
            else
            {
                // Once Player Moves, Fade Out Tooltip
                if (!playerHasMoved && player.GetComponent<PlayerMove>().IsMoving())
                {
                    Debug.Log("Player has moved");
                    playerHasMoved = true;
                    wasdTimerStarted = true;
                }
                
                // Timer
                if (wasdTimerStarted && !wasdIsGone)
                {
                    wasdTimer += Time.deltaTime;
                    if (wasdTimer >= wasdTimerMax)
                    {
                        wasdTimerStarted = false;
                        wasdTimer = 0f;
                        wasdTooltip.FadeOut();
                        wasdIsGone = true;
                        arrowToUte.Show();
                    }
                }
            
                // Once Player is near Ute, remove arrow and show 'E'
                if (!enteredUteRange && wasdIsGone && player.GetComponent<PlayerVehicleController>().InVehicleRange())
                {
                    arrowToUte.Hide();
                    enteredUteRange = true;
                    eTooltip.FadeIn();
                }  
            
                // Once Player is in Ute, fade out 'E'
                if (player.GetComponent<PlayerVehicleController>().IsInsideVehicle())
                {
                    Debug.Log("Player is in Ute");
                    eTooltip.FadeOut();
                    isFinished = true;
                }
            }
        }
    }
}
