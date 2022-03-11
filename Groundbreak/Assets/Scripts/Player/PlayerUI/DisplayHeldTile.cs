using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHeldTile : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Image>().color = Color.clear;
    }
    public void DisplayTile(GameObject tile) 
    {
       GetComponent<Image>().sprite = tile.GetComponent<SpriteRenderer>().sprite;
       GetComponent<Image>().color = tile.GetComponent<Renderer>().material.color;
    }

    public void ClearTile() 
    {
        GetComponent<Image>().sprite = null;
        GetComponent<Image>().color = Color.clear;
    }
}
