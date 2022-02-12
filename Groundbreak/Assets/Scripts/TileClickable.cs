using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileClickable : MonoBehaviour, IPointerDownHandler
{
    GameObject Player;
    int distance = -1;
    private void Start()
    {
        // Gets Reference to player
        Player = FindObjectOfType<PlayerMovement>().gameObject;
    }

    // Unity event handler only triggers when a click raycast hits the tile.
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        GameObject ThisTile = eventData.pointerCurrentRaycast.gameObject;
        // Calls movement checker fucnction in playerMovement script
        Player.GetComponent<PlayerMovement>().MovePlayer(distance, ThisTile.transform.position.x, ThisTile.transform.position.y);
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
}
