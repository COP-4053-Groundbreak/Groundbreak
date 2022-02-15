using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    Element myElement = Element.Base;
    LandFeature myLandFeature = LandFeature.None;
    int movementModifier = 0;
    public List<Tile> neighbors;

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
                newColor = new Color(188, 152, 106);
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
                newColor = Color.grey;
                break;
        }

        // Following for visualization purposes
        this.GetComponent<Renderer>().material.SetColor("_Color", newColor);
    }
    public void setMovementModifier(int a){
        movementModifier = a;
    }
    public int getMovementModifier(){
        return movementModifier;
    }
}
