using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    // All tiles start out as base tiles
    public Element myElement = Element.Base;
    // All tiles start out with a blank land feature
    LandFeature myLandFeature = LandFeature.None;
    public GameObject staticObjAbove = null;
    public GameObject gameObjectAbove;
    public Effect myEffect = null;
    int movementModifier = 0;
    public List<Tile> neighbors;
    public bool isThrowable = true;
    private TurnLogic tl;

    private void Start() {
        setElement(this.myElement);
        gameObjectAbove = null;
        tl = FindObjectOfType<TurnLogic>();
    }

    // Finds up to 8 neighbors around this tile. Does so using a PhysicsOverlap circle which detects
    // the colliders of other tiles
    public void findNeighbors(GridManager gridManager){
        StartCoroutine(waitAndFindNeighbors(gridManager));
    }
    IEnumerator waitAndFindNeighbors(GridManager gridManager){
        yield return new WaitForSeconds(1f);
        int x = (int)gameObject.GetComponent<TilePathNode>().GetX();
        int y = (int)gameObject.GetComponent<TilePathNode>().GetY();
        //Debug.Log($"x:{x},y:{y}");
        
        int[] add = {-1, 0, 1};
        for (int i = 0; i < 3; i++){
            for (int j = 0; j < 3; j++){
                bool notCenter = !(i == 1 && j == 1);
                if (notCenter && gridManager.inBounds(x + add[i], y + add[j])){
                    //Debug.Log($"{x + add[i]},{y + add[j]} is in bounds!");
                    addNeighbor(gridManager.getTile(x + add[i], y + add[j]));
                }   
            }
        }

        //Debug.Log($"Tile {this.name} has {neighbors.Count} neighbors!");
        for(int i = 0; i < neighbors.Count; i++){
            //Debug.Log(neighbors[i].name);
        }
    }
     public void setEffect(Effect newEffect){
        myEffect = newEffect;
    }
    public void setLF(LandFeature newLand){
        string text = "";
        myLandFeature = newLand;
        switch(myLandFeature){
            case LandFeature.Dungeon: // Dungeon
                text = "Dun";
                break;
            case LandFeature.Hay: // Hay
                text = "Hay";
                break;
            case LandFeature.Mountainous: // Mountainous
                text = "Mou";
                break;
            case LandFeature.Sandy: // Sandy
                text = "San";
                break;
            case LandFeature.Woods: // Woods
                text = "Woo";
                break;
            default: // None
                text = "Non";
                break;
        }

        Debug.Log($"LandFeature of {name} set to {text}");
    }
    public void addNeighbor(Tile myNeighbor){
        neighbors.Add(myNeighbor);
    }
    public void removeNeighbor(Tile myNeighbor){
        neighbors.Remove(myNeighbor);
    }
    public Element getElement(){
        return myElement;
    }
    public void setElement(Element newElement){ 
        myElement = newElement;
        Color newColor;
        switch(newElement){
            case Element.Air: // green
                newColor = Color.green;
                break;
            case Element.Earth: // brown
                newColor = new Color(210f/255f, 180f/255f, 140f/255f);
                break;
            case Element.Fire: // red
                newColor = Color.red;
                break;
            case Element.Water: // blue
                newColor = Color.blue;
                break;
            case Element.Base: // white
                newColor = Color.white;
                break;
            default: // grey
                isThrowable = false;
                newColor = Color.grey;
                break;
        }

        // Following for visualization purposes
        this.GetComponent<Renderer>().material.SetColor("_Color", newColor);
        // Set Tile Element symbol to new element
        if (transform.childCount > 0){
            //Debug.Log("I am with child");
            transform.GetChild(0).GetComponent<elemVisual>().setSymbol();
        }
    }
    public void setMovementModifier(int a){
        movementModifier = a;
    }
    public int getMovementModifier(){
        return movementModifier;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Chest")
            staticObjAbove = other.gameObject;
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player" || other.gameObject.tag == "Chest"){
            //if (other.gameObject.tag == "Player") Debug.LogWarning("Player entered a tile!");
            gameObjectAbove = other.gameObject;
        }
        if (other.gameObject.tag == "Player" && this.myElement == Element.Void && tl.isCombatPhase){
            other.gameObject.GetComponent<PlayerStats>().DealDamage(99999999);   
        }
        if (other.gameObject.tag == "Enemy" && this.myElement == Element.Void && tl.isCombatPhase){
            other.gameObject.GetComponent<EnemyStateManager>().DealDamage(99999999);   
        }
        
    }
    private void OnTriggerExit2D(Collider2D other) {
        //Debug.Log($"Exited {name}");
        // if (other.gameObject.tag == "Player") Debug.LogWarning("PlayerLeft!");
        if (other.gameObject.tag != "Effect"){
            gameObjectAbove = (staticObjAbove == null) ? null : staticObjAbove;
        }

    }
}
