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
    
    void TileOnTile(Tile a, Tile b){
        Element aElem = a.getElement();
        Element bElem = b.getElement();

        // Tiles have same element
        if (aElem == bElem) {
            // Nothing happens reaction-wise when both Base. Anything that happens when a tile is thrown should STILL happen.
            if (aElem != Element.Base) {
                // Non-Base on same Non-Base
                
                Vector2 landingPos = b.transform.position;

                
            }

        }
    }

    void TileOnEnemy(){

    }

    void EnemyOnTile(){

    }

    void AbilityOnTile(){

    }
    
}
