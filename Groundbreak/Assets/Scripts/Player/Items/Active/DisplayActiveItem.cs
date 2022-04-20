using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayActiveItem : MonoBehaviour
{
    ActiveItem activeItem;
    [SerializeField] HoldPlayerStats holdPlayerStats;
    GameObject display;
    private void Start()
    {
        activeItem = holdPlayerStats.playerActiveItem;
        display = transform.Find("Display").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        activeItem = holdPlayerStats.playerActiveItem;
        if (activeItem != null)
        {
            display.GetComponent<Image>().enabled = true;
            display.GetComponent<Image>().sprite = activeItem.GetSprite();
        }
        else 
        {
            display.GetComponent<Image>().enabled = false;
        }
    }
}
