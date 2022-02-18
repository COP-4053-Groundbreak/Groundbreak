using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileClickable : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    GameObject Player;
    TurnLogic turnLogic;
    int distance = -1;
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
                Player.GetComponent<PlayerMovement>().MovePlayer(distance, ThisTile.transform.position.x, ThisTile.transform.position.y);
            }
            // Right click
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                Player.GetComponent<PlayerActions>().PickUpTile(ThisTile.GetComponent<Tile>());
                ThisTile.GetComponent<TilePathNode>().isWalkable = false;
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
                ThisTile.GetComponent<TilePathNode>().isWalkable = false;
            }
        }
        else 
        {
            Debug.Log("Player turn phase not found!");
        }
    }

    // When mouse hovers over this tile
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) 
    {
        GameObject ThisTile = eventData.pointerCurrentRaycast.gameObject;
        Player.GetComponent<PlayerMovement>().ShowLine(ThisTile.transform.position.x, ThisTile.transform.position.y);
    }

    public void updateDistanceToPlayer() 
    {
        if (!Player)
        {
            Player = FindObjectOfType<PlayerMovement>().gameObject;
        }
        // Calculates how far this tile is from the player
        distance = (int)(Mathf.Abs(Player.transform.position.x - this.transform.position.x) +
                            Mathf.Abs(Player.transform.position.y - this.transform.position.y));
        
    }

    public int GetDistance() 
    {
        return distance;
    }
}
