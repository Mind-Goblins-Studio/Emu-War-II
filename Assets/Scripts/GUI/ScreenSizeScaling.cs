using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSizeScaling : MonoBehaviour
{
    [SerializeField] private Vector3 scale;
    
    // Start
    private void Start()
    {
        this.GetComponent<RectTransform>().localScale = scale;
    }
}
