using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionManager : MonoBehaviour
{
    [SerializeField] GridManager grid;
    // Reactions will always occur between two different objects, each with an element

    // First tile is grabbed one, second tile is stationary one
    // Reaction created when two tiles overlap
    // Different types of tile on tile reactions:
    //  Non-Base element on Base element => place element on Non-Base tile
    //  Non-Base element on different Non-Base element => AoE element reaction
    //  Non-Base element on same Non-Base element => Surrounding tiles imbued with element
    
    void TileOnTile(Tile movingTile, Tile staticTile){
        Element movingElem = movingTile.getElement();
        Element staticElem = staticTile.getElement();

        // No reaction on void tiles
        if (movingElem == Element.Void || staticElem == Element.Void)
            return;

        // Tiles have same element
        // Consume the element of both tiles and make every tile around it the same element
        if (movingElem == staticElem) {
            // Non-Base on same Non-Base
            if (movingElem != Element.Base) { 
                List<Tile> neighbors = staticTile.neighbors;
                
                // Change the element of each neighbor
                foreach (Tile neighbor in neighbors){
                    neighbor.myElement = movingElem;
                }
                
                // Consume the elements
                movingTile.myElement = Element.Base;
                staticTile.myElement = Element.Base;
            }
        } else{
            if (staticElem == Element.Base) {
                // Regardless of what the other element is, static tile will become this element
                staticTile.myElement = movingTile.myElement; 
            } else if (staticElem == Element.Air) {
                if (staticElem == Element.Earth) {
                    // Sandstorm
                } else if (staticElem == Element.Fire) {
                    // Spreads fire
                } else if (staticElem == Element.Water) {
                    // Storm
                }
            } else if (staticElem == Element.Earth) {
                if (staticElem == Element.Air) {
                    // Sandstorm
                } else if (staticElem == Element.Fire) {
                    // Magma
                } else if (staticElem == Element.Water) {
                    // Mud
                }
            } else if (staticElem == Element.Fire) {
                if (staticElem == Element.Air) {
                    // Spreads Fire
                } else if (staticElem == Element.Earth) {
                    // Magma
                } else if (staticElem == Element.Water) {
                    // Smoke
                }
            } else if (staticElem == Element.Water) {
                if (staticElem == Element.Air) {
                    // Storm
                } else if (staticElem == Element.Earth) {
                    // Mud
                } else if (staticElem == Element.Fire) {
                    // Smoke
                }
            }
        }
    }

    void TileOnEnemy(Tile a, GameObject b){

        
    }

    void EnemyOnTile(){

    }

    void AbilityOnTile(){

    }
    
}
