using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class MiniMapController : MonoBehaviour
{
    // GameObjects to track
    [SerializeField] private Transform player;
    [SerializeField] private Transform vehicle;
    [SerializeField] private GameObject emuIconPrefab;
    [SerializeField] private GameObject resourceIconPrefab;
    [SerializeField] private GameObject supplyIconPrefab;
    
    // UI Objects to track
    public GameObject playerIcon;
    public GameObject vehicleIcon;
    
    
    // Update
    private void Update()
    {
        // Update player position
        playerIcon.transform.localPosition = ConvertPosition(player.position);
        
        // Update ute position and orientation
        vehicleIcon.transform.localPosition = ConvertPosition(vehicle.position);
        vehicleIcon.transform.localRotation = Quaternion.Euler(0, 0, -vehicle.rotation.eulerAngles.y);
        
    }

    public static Vector3 ConvertPosition(Vector3 position)
    {
        Vector3 newPosition = Vector3.zero;
        newPosition.x = (position.x / (26*5f)) * (530f / 2);
        newPosition.y = (position.z / (16*5f)) * (300f / 2);
        return newPosition;
    }

    public void HidePlayer()
    {
        playerIcon.SetActive(false);
    }

    public void UnhidePlayer()
    {
        playerIcon.SetActive(true);
    }

    public void AddEmu(EmuMovement newEmu)
    {
        GameObject newIcon = Instantiate(emuIconPrefab);
        newIcon.transform.parent = this.transform;
        newIcon.transform.SetSiblingIndex(1);
        newEmu.LinkIcon(newIcon);
        newEmu.HideIcon();
    }
    
    public void AddResource(ScrapDepositController newResource)
    {
        GameObject newIcon = Instantiate(resourceIconPrefab);
        newIcon.transform.parent = this.transform;
        newIcon.transform.SetSiblingIndex(1);
        newResource.LinkIcon(newIcon);
    }

    public void AddSupply(SupplyDropController newSupply)
    {
        GameObject newIcon = Instantiate(supplyIconPrefab);
        newIcon.transform.parent = this.transform;
        newIcon.transform.SetSiblingIndex(1);
        newSupply.LinkIcon(newIcon);
    }
}
