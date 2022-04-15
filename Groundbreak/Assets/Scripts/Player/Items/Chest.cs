using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] Sprite OpenSprite;
    PassiveItemManager passiveItemManager;
    bool full = true;
    private int passiveConsumableActive = 1;
    private void Start()
    {
        passiveItemManager = FindObjectOfType<PassiveItemManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    // Add item to player when they collide
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && full) 
        {
            StartCoroutine(WaitAndOpenChest());
            full = false;
        }
    }

    // Wait half a second before opening for game feel
    IEnumerator WaitAndOpenChest()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().sprite = OpenSprite;
        if (passiveConsumableActive == 0)
        {
            passiveItemManager.AddRandomItem(this.gameObject);
        }
        else if (passiveConsumableActive == 1)
        {
            passiveItemManager.AddRandomConsumableItem(this.gameObject);
        }
        else 
        {
            passiveItemManager.AddRandomActiveItem(this.gameObject);
        }
    }

    public void SetPassiveConsumableActice(int set) 
    {
        passiveConsumableActive = set;
    }

}
