using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SpawnerManagerScript : MonoBehaviour
{
    [SerializeField] private GlobalGameTimer GlobalGameTimer;
    
    // Enum
    public enum SpawnerSide { TOP, MIDDLE, BOTTOM, ANY };
    
    // Wave Class
    [System.Serializable]
    public class Wave
    {
        public string name; // could use as start of wave name in UI
        public GameObject enemy; // type of enemy in wave
        public int count; // number of enemy in waves
        public float rate; // dont go any higher than 1.5
        public float startTime; // start time of the wave (seconds)
        public SpawnerSide spawnerSide; // which side of the map to spawn on
    }

    // Spawner Options
    public List<GameObject> spawners;

    // Wave List
    public List<Wave> waves;
    private int nextWave = 0; // pointing to next wave number 

    // Time (for wave spawning)
    private bool finalWaveReached = false;

    [SerializeField] private MiniMapController miniMapController;
    [SerializeField] private GameObject hq;
    
    // Boss GUI
    public GameObject bossGUI;
    public Slider bossHealthSlider;


    // Start is called before the first frame update
    void Start()
    {
        if (spawners.Count == 0)
        {
            Debug.LogError("No spawn points");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!finalWaveReached && GlobalGameTimer.getTime() >= waves[nextWave].startTime)
        {
            StartCoroutine(SpawnWave(waves[nextWave])); // argument is next wave to spawn
            nextWave++;
            if (nextWave >= waves.Count)
            {
                finalWaveReached = true;
            }
        }
    }

    public IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Spawning Wave: " + _wave.name);

        // Pick a spawner based on the wave
        int spawnNum = -1;
        switch (_wave.spawnerSide)
        {
            case SpawnerSide.ANY:
                spawnNum = Random.Range(0, spawners.Count);
                break;
            case SpawnerSide.TOP:
                spawnNum = 2;
                break;
            case SpawnerSide.MIDDLE:
                spawnNum = 1;
                break;
            case SpawnerSide.BOTTOM:
                spawnNum = 0;
                break;
            default:
                spawnNum = Random.Range(0, spawners.Count);
                break;
        }
        GameObject spawn = spawners[spawnNum];

        for (int i = 0; i < _wave.count; i++)
        {
            SpawnEmu(_wave.enemy, spawn);
            // wait till spawning next enemy
            yield return new WaitForSeconds(1f / _wave.rate);
        }
    }

    void SpawnEmu(GameObject enemy, GameObject spawn)
    {
        Debug.Log("Spawning Emu: " + enemy.name);
        // Spawn Emu Logic
        GameObject newEmu = Instantiate(enemy, spawn.transform.position, spawn.transform.rotation, transform);
        newEmu.GetComponent<EmuMovement>().setMoves(spawn.GetComponent<AddMoves>().moves);
        miniMapController.AddEmu(newEmu.GetComponent<EmuMovement>());
    }

    public void damageHQ(float damage)
    {
        hq.GetComponent<HQController>().loseHealth(damage);
        hq.GetComponent<AudioSource>().Play();
    }
}
