using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GlobalGameTimer GlobalGameTimer;
    
    public bool isSkipped = false;
    
    public List<AbstractTutorial> tutorials;
    private int currentTutorial = 0;

    [SerializeField] private List<GameObject> mainGameObjects = new List<GameObject>();
    [SerializeField] private CommandMoveable timerGUI;

    [SerializeField] private GameObject tutorialObjectManager;
    
    // Scrap Managers
    [SerializeField] private GameObject mainScrapManager;
    [SerializeField] private GameObject tutorialScrapManager;
    
    
    // Emu Manager
    [SerializeField] private SpawnerManagerScript tutorialEmuManager;
    
    // Audio - main game
    [SerializeField] private AudioSource mainSoundtrack;
    
    // Start
    private void Start()
    {
        if (isSkipped)
        {
            EndTutorial();
            return;
        }
        GlobalGameTimer.isTutorial = true;
        
        // Tutorial Setup
        GlobalGameTimer.PauseNoFreeze();
        tutorials[currentTutorial].gameObject.SetActive(true);
        tutorials[currentTutorial].PlayTutorial();
        
        // Main Scrap Manager, Hide Icons
        foreach (Transform child in mainScrapManager.transform)
        {
            child.GetComponent<ScrapDepositController>().HideIcon();
        }
        
        // Hide Game Objects (that matter)
        foreach (GameObject obj in mainGameObjects)
        {
            obj.SetActive(false);
        }
    }
    
    // Update
    private void Update()
    {
        // If Skipped, do Nothing
        if (isSkipped)
        {
            return;
        }
        
        // If Enter is pressed, skip the tutorial
        if (Input.GetKeyDown(KeyCode.Return))
        {
            EndTutorial();
        }

        // If the current tutorial is finished, move to the next one
        if (tutorials[currentTutorial].IsFinished())
        {
            currentTutorial++;
            if (currentTutorial < tutorials.Count)
            {
                tutorials[currentTutorial].gameObject.SetActive(true);
                tutorials[currentTutorial].PlayTutorial();
            }
            else
            {
                Debug.Log("All Tutorials Finished");
                EndTutorial();
            }
        }
    }

    private void EndTutorial()
    {   
        GlobalGameTimer.isTutorial = false;
        // Remove Scrap Icon from Tutorial Scrap
        foreach (Transform child in tutorialScrapManager.transform)
        {
            child.GetComponent<ScrapDepositController>().Unlink();
        }
        
        // Main Scrap Manager, Show Icons
        foreach (Transform child in mainScrapManager.transform)
        {
            child.GetComponent<ScrapDepositController>().ShowIcon();
        }
        
        // For Tutorial Emu Manager, Unlink and Hide Icons
        foreach (Transform child in tutorialEmuManager.transform)
        {
            if (child.GetComponent<EmuMovement>() != null)
            {
                child.GetComponent<EmuMovement>().Unlink();
            }
        }
        
        // Remove tutorial objects
        isSkipped = true;
        tutorialObjectManager.SetActive(false);
        
        // Bring Back Game Objects (that matter)
        foreach (GameObject obj in mainGameObjects)
        {
            obj.SetActive(true);
        }
        
        // Move Timer GUI
        timerGUI.CommandMove(Vector3.right*230, 500f);
        
        // Unpause Game
        GlobalGameTimer.UnpauseNoFreeze();
        this.gameObject.SetActive(false);
        
        // Audio
        mainSoundtrack.Play();
    }
}
