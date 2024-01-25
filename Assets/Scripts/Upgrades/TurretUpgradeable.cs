using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretUpgradeable : Upgradeable
{
    [SerializeField] private VehicleController ute;
    [SerializeField] private AoEDamageUpgrade aoeDamageRadius;
    
    // Implemented Upgrade Method
    public override void Upgrade(float upgradeValue, string type)
    {
        type = type.ToLower();

        switch (type)
        {
            case "damage": // damage upgrade (1xx, 2xx, 3xx)
                this.GetComponent<TurretBehaviour>().damage = upgradeValue;
                break;
            case "speed": // speed upgrades (x1x, x3x, x4x)
                this.GetComponent<TurretBehaviour>().shootCooldown = upgradeValue;
                break;
            case "range": // range upgrades (x2x)
                this.GetComponent<TurretBehaviour>().detectionRadius += upgradeValue;
                
                // find the ute controller and update radius visual
                ute.UpdateDetectionRadius();
                
                // find AOE radius and update size
                aoeDamageRadius.SetRadius(this.GetComponent<TurretBehaviour>().detectionRadius * 2.22222222f);
                break;
            
            case "sniper": // Sniper Tier 4 (4xx)
                this.GetComponent<TurretBehaviour>().damage = upgradeValue;
                this.GetComponent<TurretBehaviour>().detectionRadius += 10f;
                this.GetComponent<TurretBehaviour>().bulletSizeScale = 1.2f;
                ute.UpdateDetectionRadius();
                break;
            default:
                return;
        }
    }
}
