using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeBar : MonoBehaviour
{
    private int numBarsCurrent = 0;
    [SerializeField] private Color color;
    
    [SerializeField] private List<Image> bars;
    
    public void IncrementBars()
    {
        bars[numBarsCurrent].color = color;
        numBarsCurrent = (numBarsCurrent + 1) % bars.Count;
    }
}
