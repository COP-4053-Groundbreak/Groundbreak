using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] Sprite OpenSprite;
    PassiveItemManager passiveItemManager;
    bool full = true;
    private void Start()
    {
        passiveItemManager = FindObjectOfType<PassiveItemManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && full) 
        {
            StartCoroutine(WaitAndOpenChest());
            full = false;
        }
    }

    IEnumerator WaitAndOpenChest()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().sprite = OpenSprite;
        passiveItemManager.AddRandomItem(this.gameObject);
    }
}
