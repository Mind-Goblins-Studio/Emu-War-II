using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeGUIController : MonoBehaviour
{
    // Active and Inactive Manager
    [SerializeField] private GameObject upgradeMenu;
    [SerializeField] private GameObject upgradePopup;
    
    // Paths
    [SerializeField] private List<UpgradePathController> paths;
    
    // Tooltips
    [SerializeField] private List<GameObject> tooltips;

    private bool isActive = false;
    private bool limitReached = false;
    
    // Update
    private void Update() 
    {   
        // Toggle upgrade menu
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isActive = !isActive;
            foreach (GameObject tooltip in tooltips)
            {
                tooltip.SetActive(false);
            }
            upgradeMenu.SetActive(isActive);
            upgradePopup.SetActive(!isActive);
        }
    }
    
    // Limit Reached
    public void LimitReached()
    {
        limitReached = true;
        foreach (UpgradePathController path in paths)
        {
            path.Restrict();
        }
    }
}
