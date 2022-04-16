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
    private Sprite mySprite;

    private void Start() {
        setElement(this.myElement);
        gameObjectAbove = null;
        tl = FindObjectOfType<TurnLogic>();
        for (int i = 0; i < this.transform.childCount; i++)
            this.transform.GetChild(i).gameObject.SetActive(true);

        mySprite = this.GetComponent<SpriteRenderer>().sprite;
        if (this.GetComponent<SpriteRenderer>().sprite.name.Contains("barrel")){
           /* Debug.Log("Spawning barrel!");
            staticObjAbove = Instantiate(new GameObject("barrel"), transform.position, Quaternion.identity);
            staticObjAbove.AddComponent<Rigidbody2D>();
            staticObjAbove.AddComponent<BoxCollider2D>();*/
        }
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
                if (notCenter && gridManager.inBounds(x + add[i], y + add[j]) && (i==1 || j==1)){
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
        if (myElement == Element.Void){
            this.GetComponent<SpriteRenderer>().sprite = mySprite;
            Debug.Log("In it baby");
        }
        myElement = newElement;
        Color newColor;

        switch(newElement){
            case Element.Air: // cyan
                newColor = Color.cyan;
                newColor = new Color(newColor.r, newColor.g, newColor.b, 0.25f);
                break;
            case Element.Earth: // green
                newColor = new Color(0, 1, 0, 0.25f);
                break;
            case Element.Fire: // red
                newColor = new Color(1, 0, 0, 0.25f);
                break;
            case Element.Water: // blue
                newColor = new Color(0, 0, 1, 0.25f);
                break;
            case Element.Base: // white
                newColor = new Color(1, 1, 1, 0.25f);
                break;
            default: // grey
                isThrowable = false;
                newColor = new Color(0, 0, 0, 0.65f);
                setVoid();
                //this.GetComponent<Renderer>().material.SetColor("_Color", newColor);
                break;
        }
        
        Debug.Log("HI MY NAME IS " + transform.name);
        // Following for visualization purposes
        this.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", newColor);
        // Debug.Log("HI MY COLOR IS " + transform.GetChild(0).GetComponent<Renderer>().material.color);
        // Set Tile Element symbol to new element
        if (transform.childCount > 2){
            //Debug.Log("I am with child");
            transform.GetChild(2).GetComponent<elemVisual>().setSymbol();
        }
    }

    public void setVoid(){
        this.GetComponent<SpriteRenderer>().sprite = null;
    }
    public void setMovementModifier(int a){
        movementModifier = a;
    }
    public int getMovementModifier(){
        return movementModifier;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Chest" || other.gameObject.tag == "Barrel")
            staticObjAbove = other.gameObject;
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player" || other.gameObject.tag == "Chest" || other.gameObject.tag == "Barrel"){
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
