using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradeable : Upgradeable
{
    [SerializeField] private Material playerMaterial;
    [SerializeField] private Texture2D defaultPlayerTex;
    [SerializeField] private Texture2D upgradePlayerTex;
    
    // Start
    private void Start()
    {
        playerMaterial.mainTexture = defaultPlayerTex;
    }
    
    // Implemented Upgrade Method
    public override void Upgrade(float upgradeValue, string type)
    {
        type = type.ToLower();

        switch (type)
        {
            case "mining": // mining upgrade (xx2)
                this.GetComponent<PlayerResources>().collectionCooldown = upgradeValue;
                this.playerMaterial.mainTexture = upgradePlayerTex;
                break;
            default:
                return;
        }
    }
}
