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

    // Currently called all the time and only shown when player presses space.
    // Should probably optimize but works for now
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)){
            foreach (Transform child in tileHolder.transform){
                Debug.Log("Looking through children");
                if (child.childCount > 0){
                    GameObject elemVisual = child.GetChild(0).gameObject;
                    elemVisual.SetActive(!elemVisual.gameObject.activeInHierarchy);
                    Debug.Log($"After the change, the GO is now {elemVisual.gameObject.activeInHierarchy}");
                }
            }
        }
    }
}
