using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesDestroy : MonoBehaviour
{
    [SerializeField] private ParticleSystem targetParticleSystem;

    private void Update()
    {
        if (!targetParticleSystem.IsAlive())
            Destroy(targetParticleSystem.gameObject);
    }
}