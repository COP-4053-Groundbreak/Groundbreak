using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PassiveItemManager : MonoBehaviour
{
    [SerializeField] float pickUpTime = 1f;
    PlayerStats playerStats;
    public HoldPlayerStats holdPlayerStats;
    private void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        
    }

    public void AddRandomItem(GameObject chest)
    {
        PassiveItem item = PassiveItem.GetRandomPassiveItem();
        chest.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = item.GetSprite();
        chest.transform.GetChild(1).GetComponent<TextMeshPro>().text = item.GetEffect();
        chest.transform.GetChild(2).gameObject.SetActive(true);
        StartCoroutine(WaitAndAddItem(chest, item));

    }

    public void AddRandomConsumableItem(GameObject chest) 
    {
        ConsumableItem item = ConsumableItem.GetRandomConsumableItem();
        chest.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = item.GetSprite();
        chest.transform.GetChild(1).GetComponent<TextMeshPro>().text = item.GetDescription();
        chest.transform.GetChild(2).gameObject.SetActive(true);
        StartCoroutine(WaitAndAddConsumableItem(chest, item));
    }


    // Adding items


    // Remove item


    // Enumerator to have a delay before item is added
    IEnumerator WaitAndAddItem(GameObject chest, PassiveItem item)
    {
        yield return new WaitForSeconds(pickUpTime);
        holdPlayerStats.playerPassiveInventory.AddItem(item, playerStats);
        chest.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
        chest.transform.GetChild(1).GetComponent<TextMeshPro>().text = "";
        chest.transform.GetChild(2).gameObject.SetActive(false);
    }

    IEnumerator WaitAndAddConsumableItem(GameObject chest, ConsumableItem item)
    {
        yield return new WaitForSeconds(pickUpTime);
        FindObjectOfType<UIConsumableInventoryController>().uIInventory.consumableInventory.AddItem(item);
        chest.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
        chest.transform.GetChild(1).GetComponent<TextMeshPro>().text = "";
        chest.transform.GetChild(2).gameObject.SetActive(false);
    }
}
