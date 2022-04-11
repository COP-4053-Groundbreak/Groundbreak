using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileOutline : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Renderer>().material.color = Color.clear;
    }
}

