using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndScreenManager : MonoBehaviour
{
    private PersistentGameInfo persistentGameInfo;
    
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI scrapText;

    // Start is called before the first frame update
    void Start()
    {
        persistentGameInfo = PersistentGameInfo.GetInstance();
        
        if (timeText != null)
        {
            timeText.text = TimeConversion(persistentGameInfo.timeRemaining);
        }
        
        if (healthText != null)
        {
            healthText.text = $"{persistentGameInfo.healthRemaining * 100}%";
        }
        
        if (scrapText != null)
        {
            scrapText.text = persistentGameInfo.scrapRemaining.ToString();
        }
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
    
}
