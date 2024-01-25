using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    [SerializeField] private CommandMoveable credits;
    [SerializeField] private float creditsSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        // Uncover Button & Ute after 3 sec
        Invoke("StartCredits", 1.2f);
    }
    
    private void StartCredits()
    {
        credits.CommandMove(Vector3.up * 100000, creditsSpeed);
    }
}
