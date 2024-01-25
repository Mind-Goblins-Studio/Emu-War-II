using System.Collections.Generic;
using UnityEngine;

public class ScrapDepositManager : MonoBehaviour
{
    private List<GameObject> scrap = new List<GameObject>();
    
    [SerializeField] private MiniMapController miniMapController;
    
    public void Start()
    {
        // Get all child scrap
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Resource"))
            {   
                // Link to Icon
                scrap.Add(child.gameObject);
                miniMapController.AddResource(child.GetComponent<ScrapDepositController>());
            }
        }
    }
}
