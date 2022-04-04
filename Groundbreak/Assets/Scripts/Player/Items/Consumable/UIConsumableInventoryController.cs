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
            if (uIInventory.gameObject.transform.GetChild(0).gameObject.activeInHierarchy)
            {
                foreach (Transform child in uIInventory.transform)
                {
                    child.gameObject.SetActive(false);
                }
            }
            else
            {
                foreach (Transform child in uIInventory.transform)
                {
                    if (child.gameObject.name != "itemTemplate")
                    {
                        child.gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}
