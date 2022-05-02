using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class PassiveItemManager : MonoBehaviour
{
    [SerializeField] float pickUpTime = 1f;
    PlayerStats playerStats;
    public HoldPlayerStats holdPlayerStats;
    bool accepted = false;
    bool rejected = false;
    private void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        
    }

    public void AddRandomItem(GameObject chest)
    {
        PassiveItem item;
        if (SceneManager.GetActiveScene().name == "Level2")
        {
            item = PassiveItem.GetRandomHighPassiveItem();
        }
        else 
        {
            item = PassiveItem.GetRandomPassiveItem();
        }
        chest.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = item.GetSprite();
        chest.transform.GetChild(1).GetComponent<TextMeshPro>().text = item.GetEffect();
        chest.transform.GetChild(2).gameObject.SetActive(true);
        chest.transform.GetChild(4).gameObject.SetActive(true);
        StartCoroutine(WaitAndAddItem(chest, item));

    }

    public void AddRandomConsumableItem(GameObject chest) 
    {
        ConsumableItem item = ConsumableItem.GetRandomConsumableItem();
        chest.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = item.GetSprite();
        chest.transform.GetChild(1).GetComponent<TextMeshPro>().text = item.GetDescription();
        chest.transform.GetChild(2).gameObject.SetActive(true);
        chest.transform.GetChild(4).gameObject.SetActive(true);
        StartCoroutine(WaitAndAddConsumableItem(chest, item));
    }
    public void AddRandomActiveItem(GameObject chest)
    {
        ActiveItem item = ActiveItem.GetRandomActiveItem();
        chest.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = item.GetSprite();
        chest.transform.GetChild(1).GetComponent<TextMeshPro>().text = item.GetDescription();
        chest.transform.GetChild(2).gameObject.GetComponentInChildren<TextMeshPro>().SetText("Replace");
        chest.transform.GetChild(2).gameObject.SetActive(true);
        chest.transform.GetChild(3).gameObject.SetActive(true);
        chest.transform.GetChild(4).gameObject.SetActive(true);
        StartCoroutine(WaitAndAddActiveItem(chest, item));
    }

    public void AddTutorial(GameObject chest)
    {
        holdPlayerStats.playerPassiveInventory.AddItem(PassiveItem.GetRandomPassiveItem(), playerStats);
        FindObjectOfType<UIConsumableInventoryController>().uIInventory.consumableInventory.AddItem(ConsumableItem.GetRandomConsumableItem());
        holdPlayerStats.playerActiveItem = new ActiveItem { itemName = ActiveItem.ActiveItemName.Bow};
    }

    // Adding items


    // Remove item


    // Enumerator to have a delay before item is added
    IEnumerator WaitAndAddItem(GameObject chest, PassiveItem item)
    {
        accepted = false;
        rejected = false;
        yield return new WaitUntil(() => accepted);
        holdPlayerStats.playerPassiveInventory.AddItem(item, playerStats);
        chest.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
        chest.transform.GetChild(1).GetComponent<TextMeshPro>().text = "";
        chest.transform.GetChild(2).gameObject.SetActive(false);
        chest.transform.GetChild(4).gameObject.SetActive(false);
        accepted = false;
        rejected = false;
    }

    IEnumerator WaitAndAddConsumableItem(GameObject chest, ConsumableItem item)
    {
        accepted = false;
        rejected = false;
        yield return new WaitUntil(() => accepted);
        FindObjectOfType<UIConsumableInventoryController>().uIInventory.consumableInventory.AddItem(item);
        chest.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
        chest.transform.GetChild(1).GetComponent<TextMeshPro>().text = "";
        chest.transform.GetChild(2).gameObject.SetActive(false);
        chest.transform.GetChild(4).gameObject.SetActive(false);
        accepted = false;
        rejected = false;
    }
    IEnumerator WaitAndAddActiveItem(GameObject chest, ActiveItem item)
    {
        accepted = false;
        rejected = false;
        yield return new WaitUntil(() => accepted || rejected);
        if (accepted) 
        {
            holdPlayerStats.playerActiveItem = item;
        }
        chest.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
        chest.transform.GetChild(1).GetComponent<TextMeshPro>().text = "";
        chest.transform.GetChild(2).gameObject.SetActive(false);
        chest.transform.GetChild(3).gameObject.SetActive(false);
        chest.transform.GetChild(4).gameObject.SetActive(false);
        accepted = false;
        rejected = false;
    }

    public void Accept() 
    {
        accepted = true;
    }
    public void Reject()
    {
        rejected = true;
    }
}
