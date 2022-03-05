using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementDisplayUI : MonoBehaviour
{
    [SerializeField] GameObject tileHolder;
    [SerializeField] GameObject elemVisual;
    
    // Start is called before the first frame update
    private void Start() {
        foreach (Transform child in tileHolder.transform){
            Instantiate(elemVisual, child.gameObject.transform.position, Quaternion.identity, child);
        }
    }  
}
