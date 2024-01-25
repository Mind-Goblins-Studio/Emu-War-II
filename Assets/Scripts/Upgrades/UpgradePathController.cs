using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class UpgradePathController : MonoBehaviour
{
    [System.Serializable] public class UpgradeElem
    {
        public Upgradeable upgradeable; // object to upgrade
        public float price; // price of upgrade 
        public string type; // name of attribute to upgrade
        public float newValue; // new value of attribute
        public Sprite buttonImage; // button image for upgrade
        public Sprite tooltipImage; // tooltip image for upgrade
        
        // // Object Swapping
        public GameObject oldObject;
        public GameObject newObject;
    }

    [SerializeField] private List<UpgradeElem> upgrades; // list of upgrades
    private int idx = 0;
    [SerializeField] private Button activeButton;

    [SerializeField] private UpgradeBar upgradeBar;
    [SerializeField] private Image upgradeTooltip;

    [SerializeField] private PlayerResources playerResources;
    
    private bool limitReached = false;
    private bool isRestricted = false;
    
    [SerializeField] private KeyCode hotkey;
    
    // Start
    private void Start()
    {
        activeButton.image.sprite = upgrades[idx].buttonImage;
        upgradeTooltip.sprite = upgrades[idx].tooltipImage;
    }
    
    // Update
    private void Update()
    {
        if (!limitReached)
        {
            // Check if upgrade is restricted
            if (isRestricted && (idx + 1 == upgrades.Count - 1))
            {
                activeButton.interactable = false;
                return;
            }
            
            // Check Resource Count with Current Price
            if (playerResources.resourceCount >= upgrades[idx].price && playerResources.InUteRange())
            {
                activeButton.interactable = true;
            }
            else
            {
                activeButton.interactable = false;
            }
        }
        
        // Check for Hotkey
        if (Input.GetKeyDown(hotkey) && activeButton.interactable)
        {
            Upgrade();
        }
    }
    
    // Button Clicked
    public void Upgrade()
    {
        if (!limitReached)
        {
            // Pay Price
            playerResources.resourceCount -= upgrades[idx].price;
            
            // Object Swapping
            if (upgrades[idx].oldObject != null && upgrades[idx].newObject != null)
            {
                upgrades[idx].oldObject.SetActive(false);
                upgrades[idx].newObject.SetActive(true);
                upgrades[idx].newObject.GetComponent<UpgradeModel>().SpawnParticles();
            }
            
            // Shift upgrade by one
            upgrades[idx].upgradeable.Upgrade(upgrades[idx].newValue, upgrades[idx].type);
            idx += 1;
            activeButton.image.sprite = upgrades[idx].buttonImage;
            upgradeTooltip.sprite = upgrades[idx].tooltipImage;
            upgradeBar.IncrementBars();
            
            // Tier four just bought
            if ((idx + 1) == upgrades.Count)
            {
                limitReached = true;
                activeButton.interactable = false;
                
                // Inform controller and restrict any further tier 4s being bought
                UpgradeGUIController controller = GameObject.Find("UpgradeManager")
                    .GetComponent<UpgradeGUIController>();
                controller.LimitReached();
            }
            
            // Audio, 2 levels above
            transform.parent.parent.GetComponent<AudioSource>().Play();
        }
    }

    public void Restrict()
    {
        isRestricted = true;
    }

}
