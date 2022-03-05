using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHeldTile : MonoBehaviour
{
    public void DisplayTile(GameObject tile) 
    {
       GetComponent<Image>().sprite = tile.GetComponent<SpriteRenderer>().sprite;
       GetComponent<Image>().color = tile.GetComponent<Renderer>().material.color;
    }
}
