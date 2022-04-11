using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCopy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<SpriteRenderer>().sprite = transform.parent.transform.parent.GetComponent<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
