using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIConsumableInventoryController : MonoBehaviour
{
    public HoldPlayerStats playerInformation;
    public UIConsumableInventory uIInventory;

    private void Start()
    {
        uIInventory.SetConsumableInventory(playerInformation.playerConsumableInventory, true);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) 
        {
            if (uIInventory.gameObject.activeInHierarchy)
            {
                uIInventory.gameObject.SetActive(false);
            }
            else 
            {
                uIInventory.gameObject.SetActive(true);
            }
        }
    }
}
