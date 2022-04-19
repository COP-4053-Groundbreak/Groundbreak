using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIActiveItem : MonoBehaviour
{
    [SerializeField] HoldPlayerStats playerStats;

    // Update is called once per frame
    void Update()
    {
        if (playerStats.playerActiveItem != null)
        {
            gameObject.transform.Find("Image").GetComponent<Image>().enabled = true;
            gameObject.transform.Find("Image").GetComponent<Image>().sprite = playerStats.playerActiveItem.GetSprite();
        }
        else 
        {
            gameObject.transform.Find("Image").GetComponent<Image>().enabled = false;
        }
    }
}
