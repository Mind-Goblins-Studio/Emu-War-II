using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleOnStart : MonoBehaviour
{
    [SerializeField] private GameObject particles;
    
    // On Enable
    private void OnEnable()
    {
        GameObject particles = Instantiate(this.particles, 
            transform.position + Vector3.back + Vector3.up, Quaternion.identity);
        particles.GetComponent<ParticleSystem>().Play();
    }
}
