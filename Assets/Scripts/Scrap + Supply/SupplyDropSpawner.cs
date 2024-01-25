using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class SupplyDropSpawner : MonoBehaviour
{
    [SerializeField] private GlobalGameTimer GlobalGameTimer;
    
    // Player
    [SerializeField] private PlayerResources player;

    // Prefab
    [SerializeField] private GameObject supplyDropPrefab;

    // Spawn Objects
    [SerializeField] private List<Vector3> spawnPoints;
    [SerializeField] private List<float> spawnTimes;

    [SerializeField] private MiniMapController miniMapController;

    // Notifications
    [SerializeField] private Fading notification;
    [SerializeField] private Fading pickupNotification;
    [SerializeField] private float timeToFade = 2f;
    
    private float timer = 0;
    private float timerMax = 7f;
    private bool timerStarted = false;

    // Update is called once per frame
    void Update()
    {
        // Check if we need to spawn a new supply drop
        if (spawnTimes.Count > 0 && GlobalGameTimer.getTime() >= spawnTimes[0])
        {
            // Pick a random spawn location
            // int index = Random.Range(0, spawnPoints.Count);

            // Spawn a new supply drop
            GameObject supplyDrop = Instantiate(supplyDropPrefab,
                spawnPoints[0], Quaternion.identity);
            supplyDrop.transform.parent = this.transform;

            // Link Icon
            miniMapController.AddSupply(supplyDrop.GetComponent<SupplyDropController>());
            notification.FadeIn();
            
            // Start Timer
            timerStarted = true;

            // Remove the spawn time and spawn location
            spawnTimes.RemoveAt(0);
            spawnPoints.RemoveAt(0);
            
            // Audio, 1st audio source is spawn sound
            transform.GetComponents<AudioSource>()[0].Play();
        }
        
        // Timer to remove notification
        if (timerStarted)
        {
            timer += Time.deltaTime;
            
            if (timer >= timerMax)
            {
                notification.FadeOut();
                timerStarted = false;
                timer = 0;
            }
        }

    }

    // Add Resources to Player
    public void AddResource(float value)
    {
        Debug.Log("Supply Drop Picked Up");
        player.resourceCount += value;
        pickupNotification.FadeIn();
        Invoke("FadeOutPickup", timeToFade);
    }
    
    // Fade Out Pickup
    private void FadeOutPickup()
    {
        pickupNotification.FadeOut();
    }
}
