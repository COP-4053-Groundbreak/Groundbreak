using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elemVisual : MonoBehaviour
{
    // Elements stored in alphabetical ordered, accessible by bitshifting
    [SerializeField] Sprite[] elementSymbols;
    private bool isVisible = false;
    // Index in array of corresponding visual
    int idx;
    // Start is called before the first frame update
    void Start(){
        int a = (int)transform.parent.GetComponent<Tile>().getElement();
        // Element to index convertor
        idx = ReactionManager.elementToIdx(transform.parent.GetComponent<Tile>().getElement());
        setSymbol();
        setVisibility(false);
    }

    // Currently called all the time and only shown when player presses space.
    // Should probably optimize but works for now
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            setVisibility(!isVisible); // Invert visibility
        }
    }

    // Sets whether the UI is visible or not. GameObject ALWAYS exists and is active,
    // not sure if this is what's best for performance
    private void setVisibility(bool visible){
        if (visible){
            this.gameObject.transform.localScale = new Vector3(1, 1, 1);
            isVisible = true;
        } else {
            this.gameObject.transform.localScale = new Vector3(0, 0, 0);
            isVisible = false;
        }
    }

    // Sets symbol of UI to whatever element its parent tile is
    public void setSymbol(){
        idx = ReactionManager.elementToIdx(transform.parent.GetComponent<Tile>().getElement());
         // Debug.Log((idx>=0 && idx < 4) ? "In Bounds of array": $"Out of bounds of array val {idx}");
        this.gameObject.GetComponent<SpriteRenderer>().sprite = (idx < 4) ? elementSymbols[idx] : null;
    }
}
