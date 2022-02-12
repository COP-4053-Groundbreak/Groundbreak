using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileClickable : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] GameObject Player;

    private void Start()
    {
        // Gets Reference to player
        Player = FindObjectOfType<PlayerMovement>().gameObject;
    }

    // Unity event handler only triggers when a click raycast hits the tile.
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        GameObject ThisTile = eventData.pointerCurrentRaycast.gameObject;
        // Calculates distance to player
        int distance = (int)(Mathf.Abs(Player.transform.position.x - ThisTile.transform.position.x) +
                        Mathf.Abs(Player.transform.position.y - ThisTile.transform.position.y));
        // Calls movement checker fucnction in playerMovement script
        Player.GetComponent<PlayerMovement>().MovePlayer(distance, ThisTile.transform.position.x, ThisTile.transform.position.y);
    }
}
