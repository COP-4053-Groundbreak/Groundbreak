using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elemVisual : MonoBehaviour{
    // Elements stored in alphabetical ordered, accessible by bitshifting
    [SerializeField] Sprite[] elementSymbols;
    // Index in array of corresponding visual
    int idx;
    // Start is called before the first frame update
    void Start(){
        int a = (int)transform.parent.GetComponent<Tile>().getElement();
        // Element to index convertor
        idx = ReactionManager.elementToIdx(transform.parent.GetComponent<Tile>().getElement());
        setSymbol();
        this.gameObject.SetActive(false);
    }

    // Sets whether the UI is visible or not. GameObject ALWAYS exists and is active,
    // not sure if this is what's best for performance

    // Sets symbol of UI to whatever element its parent tile is
    public void setSymbol(){
        Debug.Log("Setting symbol, element is " + transform.parent.GetComponent<Tile>().getElement());
        idx = ReactionManager.elementToIdx(transform.parent.GetComponent<Tile>().getElement());
        if (transform.parent.GetComponent<Tile>().getElement() == Element.Void){
            this.gameObject.GetComponent<SpriteRenderer>().sprite = null;
        }

        Debug.Log("Index is *" + idx);
         // Debug.Log((idx>=0 && idx < 4) ? "In Bounds of array": $"Out of bounds of array val {idx}");
        this.gameObject.GetComponent<SpriteRenderer>().sprite = (idx < 4) ? elementSymbols[idx] : null;
    }
}
