using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerActions : MonoBehaviour
{
    public bool canPickUpTile = true;
    [SerializeField] int pickupRange = 1;
    [SerializeField] int throwRange = 1;
    [SerializeField] Element heldTileElement;
    GameObject[] enemyList;
    TurnLogic turnLogic;

    private void Start()
    {
        turnLogic = FindObjectOfType<TurnLogic>();
        heldTileElement = Element.Void;
        enemyList = GameObject.FindGameObjectsWithTag("Enemy");

    }
    // Picks up a tile and stores what element it is
    public void PickUpTile(Tile tile) 
    {
        // Checks its your turn, you have an action to pick up tile, the tile's element is void, and that its within range
        if (turnLogic.GetIsPlayerTurn() && canPickUpTile && tile.getElement() != Element.Void && tile.gameObject.GetComponent<TileClickable>().GetDistance() <= pickupRange) 
        {
            // Cant pickup tile with somehting on top of it
            if (tile.gameObjectAbove == null){
                foreach (GameObject enemy in enemyList) 
                {
                    // Breaks the function if we try to pick up a tile an enemy is standing on
                    if (enemy.transform.position == tile.transform.position) 
                    {
                        return;
                    }
                }

                // Update UI for tile held
                FindObjectOfType<DisplayHeldTile>().DisplayTile(tile.gameObject);

                // swap held elements
                Element temp = tile.getElement();
                tile.setElement(heldTileElement);
                heldTileElement = temp;

                // Set walkable
                if (tile.getElement() == Element.Void)
                {
                    tile.gameObject.GetComponent<TilePathNode>().isWalkable = false;
                }
                else
                {
                    tile.gameObject.GetComponent<TilePathNode>().isWalkable = true;
                }
                // Can only pick up one tile a turn
                canPickUpTile = false;
                FindObjectOfType<PlayerMovement>().ClearLine();
                Debug.Log(heldTileElement);
            }
        }
    }

    // Called when player right clicks on a tile
    // Here you can call your own element interaction functions based on the 
    // tile game object passed in and the held tile or whatever way you want to implement it
    public void ThrowTile(GameObject tile) 
    {
        if (turnLogic.GetIsPlayerTurn() && heldTileElement != Element.Void && tile.GetComponent<TileClickable>().GetDistance() <= throwRange) 
        {
            Debug.Log($"Threw a(n) {heldTileElement} tile at a {tile.GetComponent<Tile>().myElement}!");
            ReactionManager.catchElement(heldTileElement, tile);
            // Need to check if there is an enemy standing on this tile to deal base throw damage
            tile.GetComponent<TilePathNode>().isWalkable = true;
            heldTileElement = Element.Void;
            FindObjectOfType<DisplayHeldTile>().ClearTile();
        }
        
    }

    public void ResetActions() 
    {
        canPickUpTile = true;
    }
}
