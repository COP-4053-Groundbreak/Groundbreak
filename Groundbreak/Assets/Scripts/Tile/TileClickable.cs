using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileClickable : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    GameObject Player;
    TurnLogic turnLogic;
    int distance = -1;
    Tile lastTileHovered;
    private void Start()
    {
        // Gets Reference to player
        Player = FindObjectOfType<PlayerMovement>().gameObject;
        turnLogic = FindObjectOfType<TurnLogic>();
    }

    // Unity event handler only triggers when a click raycast hits the tile.
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        
        GameObject ThisTile = eventData.pointerCurrentRaycast.gameObject;
        if (turnLogic.isMovementPhase)
        {
            // Left click
            if (eventData.button == 0)
            {
                // Calls movement checker fucnction in playerMovement script
                Player.GetComponent<PlayerMovement>().MovePlayer(distance, ThisTile.GetComponent<TilePathNode>().GetX(), ThisTile.GetComponent<TilePathNode>().GetY());
            }
            // Right click
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                Player.GetComponent<PlayerActions>().PickUpTile(ThisTile.GetComponent<Tile>());
                
            }
        }
        else if (turnLogic.isThrowPhase)
        {
            // Left click
            if (eventData.button == 0) 
            {
                Player.GetComponent<PlayerActions>().ThrowTile(ThisTile);
            }
            // Right click
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                Player.GetComponent<PlayerActions>().PickUpTile(ThisTile.GetComponent<Tile>());
                
            }
        }
        else 
        {
            Debug.Log("Player turn phase not found!");
        }
    }

    // When mouse hovers over this tile
    // Added some "UI" that would allow to see neighbors more easily to debug. Might be  useful
    // in the future to show active item effect AoE/MovementRange. They're all the comments with
    // -P before them
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) 
    {
        
        GameObject ThisTile = eventData.pointerCurrentRaycast.gameObject;
        // -P: lastTileHovered = ThisTile.GetComponent<Tile>();
        Player.GetComponent<PlayerMovement>().ShowLine(ThisTile.GetComponent<TilePathNode>().GetX(), ThisTile.GetComponent<TilePathNode>().GetY());
        // -P: foreach(Tile neighbor in lastTileHovered.neighbors)
            // -P: neighbor.GetComponent<Renderer>().material.color = Color.yellow;
    }

    // Clear line when mouse leaves the tile
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData) 
    {
        Player.GetComponent<PlayerMovement>().ClearLine();
        // -P: Debug.Log($"Set {lastTileHovered.name}'s neighbors to original color!");
        // -P: foreach(Tile neighbor in lastTileHovered.neighbors)
            // -P: neighbor.setElement(neighbor.myElement);
        
    }

    public void updateDistanceToPlayer() 
    {
        if (!Player)
        {
            Player = FindObjectOfType<PlayerMovement>().gameObject;
        }
        
        // Calculates how far this tile is from the player
        distance = (int)(Mathf.Abs(Player.GetComponent<PlayerMovement>().playerX - gameObject.GetComponent<TilePathNode>().GetX()) +
                            Mathf.Abs(Player.GetComponent<PlayerMovement>().playerY - gameObject.GetComponent<TilePathNode>().GetY()));
        
    }

    public int GetDistance() 
    {
        return distance;
    }
}
