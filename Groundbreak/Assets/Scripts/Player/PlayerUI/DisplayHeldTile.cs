using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHeldTile : MonoBehaviour
{
    Image myImage;
    private void Start()
    {
        myImage = GetComponent<Image>();
        myImage.color = Color.clear;
    }
    public void DisplayTile(GameObject tile) 
    {
       myImage.sprite = tile.GetComponent<SpriteRenderer>().sprite;
       myImage.color = tile.transform.GetChild(0).GetComponent<Renderer>().material.color;
       myImage.color = new Color(myImage.color.r, myImage.color.g, myImage.color.b);
       Debug.Log("BB" + this.transform.childCount);
       Image p = this.transform.GetChild(0).GetComponent<Image>();
       p.sprite = tile.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite;
    }

    public void ClearTile() 
    {
        myImage.sprite = null;
        myImage.color = Color.clear;
    }
}
