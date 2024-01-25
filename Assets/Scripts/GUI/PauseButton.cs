using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseButton : MonoBehaviour
{
    private bool isPaused = false;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GlobalGameTimer GlobalGameTimer;
    
    private void Update()
    {
        // Pause Game if Escape or Backspace is pressed
        if (Input.GetKeyDown(KeyCode.Escape) || (Input.GetKeyDown(KeyCode.Backspace)))
        {
            TogglePause();
        }
    }
    
    // On Click
    public void TogglePause()
    {
        isPaused = !isPaused;
        if (!isPaused)
        {
            GlobalGameTimer.Pause();
            pauseMenu.SetActive(true);
        }
        else
        {
            GlobalGameTimer.Unpause();
            pauseMenu.SetActive(false);
        }
    }

    public void RestartGame()
    {
        GlobalGameTimer.ReloadCurrent();
    }
}
