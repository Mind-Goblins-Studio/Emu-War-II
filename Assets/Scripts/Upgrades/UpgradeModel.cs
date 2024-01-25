using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeModel : MonoBehaviour
{
    [SerializeField] private GameObject particles;
    [SerializeField] private Transform particleSpawnLocation;
    
    // Spawn Particles 
    public void SpawnParticles()
    {
        Instantiate(particles, particleSpawnLocation.position, Quaternion.identity);
    }
}
