using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UteUpgradeable : Upgradeable
{
    [SerializeField] private GameObject aura;
    
    // Implemented Upgrade Method
    public override void Upgrade(float upgradeValue, string type)
    {
        type = type.ToLower();

        switch (type)
        {
            case "driving": // off-road upgrade (xx1)
                Debug.Log("driving up");
                this.GetComponent<VehicleMove>().maxSpeed = this.GetComponent<VehicleMove>()
                    .maxSpeed * upgradeValue;
                this.GetComponent<VehicleMove>().acceleration= this.GetComponent<VehicleMove>()
                    .acceleration * upgradeValue;
                break;
            case "autoshoot": // ai turret (xx3)
                this.GetComponent<VehicleController>().EnableAutoShoot();
                break;
            case "aura": // Great Wall (xx4)
                aura.SetActive(true);
                break;
            default:
                return;
        }
    }
}
