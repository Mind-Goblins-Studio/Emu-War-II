using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalGameTimer : MonoBehaviour
{
    private float timeElapsed = 0f;
    private float START_GAME_TIME = 300.99f; // 5 minutes
    private bool isPaused = false;
    public bool isTutorial = false;
    private bool gameWon = false;
    
    private SceneFade sceneFade;
    
    // Objects to get data from
    private PersistentGameInfo persistentGameInfo;
    [SerializeField] private PlayerResources playerResources;
    [SerializeField] private HQController hqController;
    
    // Start
    void Start()
    {
        sceneFade = GetComponent<SceneFade>();
        persistentGameInfo = PersistentGameInfo.GetInstance();
        ResetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused && !isTutorial)
        {
            timeElapsed += Time.deltaTime;
        }
        
        // If Time Over, End Game
        if (timeElapsed >= START_GAME_TIME)
        {
            EndGameWin();
        }
    }
    
    [ContextMenu("Game Win")]
    public void EndGameWin()
    {
        Debug.Log("YOU WIN, GAME IS OVER");
        gameWon = true;
        sceneFade.SceneFadeOut();
        // Delay the scene fade-out by 0.25 seconds (250 milliseconds).
        Invoke("SceneFadeOut", 0.55f);
    }

    [ContextMenu("Game Lose")]
    public void EndGameLose()
    {
        Debug.Log("YOU LOSE, GAME IS OVER");
        gameWon = false;
        sceneFade.SceneFadeOut();
        // Delay the scene fade-out by 0.25 seconds (250 milliseconds).
        Invoke("SceneFadeOut", 0.55f);
    }

    // This method will be invoked after the specified delay.
    private void SceneFadeOut()
    {
        // Save the game data.
        persistentGameInfo.timeRemaining = START_GAME_TIME - timeElapsed;
        persistentGameInfo.healthRemaining = hqController.health / hqController.GetMaxHealth();
        persistentGameInfo.scrapRemaining = playerResources.resourceCount;
        
        // After the fade-out, load the respective scene based on the game result.
        if (gameWon)
        {
            SceneManager.LoadScene("WinScene");
        }
        else
        {
            SceneManager.LoadScene("LoseScene");
        }
    }
    
    [ContextMenu("Add Time")]
    public void RemoveTime30()
    {
        timeElapsed += 30;
    }
    
    public float getTime()
    {
        return timeElapsed;
    }
    
    public float getStartTime()
    {
        return START_GAME_TIME;
    }
    
    public float getTimeRemaining()
    {
        return START_GAME_TIME - timeElapsed;
    }

    private void ResetTimer()
    {
        timeElapsed = 0f;
    }

    public void PauseNoFreeze()
    {
        isPaused = true;
    }
    
    public void UnpauseNoFreeze()
    {
        isPaused = false;
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0;
    }
    
    public void Unpause()
    { 
        isPaused = false;
        Time.timeScale = 1;
    }

    public void ReturnToMenu()
    {
        ReloadCurrent();
        
        // Switch to StartScene
        sceneFade.SceneFadeOut();
        SceneManager.LoadScene("StartScene");
    }

    public void ReloadCurrent()
    {
        // Reload Current Scene
        ResetTimer();
        Unpause();
        SceneManager.LoadScene("GameScene");
    }

    public void Restart()
    {
        sceneFade.SceneFadeOut();
        Invoke("ReloadCurrent", 0.55f);
    }
}
