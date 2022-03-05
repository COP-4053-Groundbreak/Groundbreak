using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    [SerializeField] GridManager gridManager;
    // All tiles start out as base tiles
    public Element myElement = Element.Base;
    // All tiles start out with a blank land feature
    LandFeature myLandFeature = LandFeature.None;

    public GameObject gameObjectAbove = null;
    public Effect myEffect = null;
    int movementModifier = 0;
    public List<Tile> neighbors;
    public bool isThrowable = true;

    // Finds up to 8 neighbors around this tile. Does so using a PhysicsOverlap circle which detects
    // the colliders of other tiles
    public void findNeighbors(){
        
        Collider2D[] neighborColliders = Physics2D.OverlapCircleAll(this.transform.position, 1.0f);

             
        Debug.Log($"Tile {this.name} collided with {neighborColliders.Length} neighbors");
        foreach(Collider2D a in neighborColliders){
            if (a.gameObject.tag == "Tile" && a.gameObject.name != this.name){
                // Can be optimized by making it so that both tiles mark each other as neighbor
                addNeighbor(a.gameObject.GetComponent<Tile>());
            }
        }
        
        /*
        int x = (int)transform.position.x;
        int y = (int)transform.position.y;
        int[,] add = {{-1,-1},{0,-1},{1,-1},{-1,0},{1,0},{-1,1},{0,1},{1,1}};

        for (int i = 0; i < 8; i ++){
            if (gridManager.inBounds(x + add[i, 0], y + add[i, 1]))
                addNeighbor(GridManager.grid[x + add[i, 0], y + add[i, 1]]);
        }*/
        
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
        if (transform.childCount > 0)
            transform.GetChild(0).GetComponent<elemVisual>().setSymbol();
    }
    public void setMovementModifier(int a){
        movementModifier = a;
    }
    public int getMovementModifier(){
        return movementModifier;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag != "Effect"){
            gameObjectAbove = other.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        Debug.Log($"Exited {name}");
        if (other.gameObject.tag != "Effect"){
            gameObjectAbove = null;
        }
    }
}
