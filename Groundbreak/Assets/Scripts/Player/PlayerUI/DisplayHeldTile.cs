using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHeldTile : MonoBehaviour
{
    Image myImage, symbolImage;
    private void Start()
    {
        myImage = GetComponent<Image>();
        myImage.color = Color.clear;
        symbolImage = this.transform.GetChild(0).GetComponent<Image>();
        symbolImage.color = Color.clear;
    }
    public void DisplayTile(GameObject tile) 
    {
       myImage.sprite = tile.GetComponent<SpriteRenderer>().sprite;
       myImage.color = tile.transform.GetChild(0).GetComponent<Renderer>().material.color;
       myImage.color = new Color(myImage.color.r, myImage.color.g, myImage.color.b);

       symbolImage.sprite = tile.transform.Find("elemVisual(Clone)").GetComponent<SpriteRenderer>().sprite;
       symbolImage.color = Color.black;
    }

    public void ClearTile() 
    {
        myImage.sprite = null;
        myImage.color = Color.clear;
        Image childImage = this.transform.GetChild(0).GetComponent<Image>();
        childImage.sprite = null;
        childImage.color = Color.clear;
    }
}
