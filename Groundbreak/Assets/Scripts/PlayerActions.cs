using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public bool canPickUpTile = true;
    [SerializeField] int pickupRange = 1;
    [SerializeField] Element heldTileElement;

    // Picks up a tile and stores what element it is
    public void PickUpTile(Tile tile) 
    {
        // Checks you have an action to pick up tile, the tile's element is void, and that its within range
        if (canPickUpTile && tile.getElement() != Element.Void && tile.gameObject.GetComponent<TileClickable>().GetDistance() <= pickupRange) 
        {
            // Cant pickup tile you are standing on
            if (tile.gameObject.transform.position != this.transform.position) 
            {
                heldTileElement = tile.getElement();
                canPickUpTile = false;
                // Currently sets tile element to void, possibly will change depending on how we want to implement this
                tile.setElement(Element.Void);
                Debug.Log(heldTileElement); 
            }
        }
    }

    public void ResetActions() 
    {
        canPickUpTile = true;
    }
}
