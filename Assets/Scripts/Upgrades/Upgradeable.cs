using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Upgradeable : MonoBehaviour
{
    // Abstract method for Upgrade
    public abstract void Upgrade(float upgradeValue, string type);
}
