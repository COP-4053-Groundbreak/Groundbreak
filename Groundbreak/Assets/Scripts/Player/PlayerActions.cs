using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public bool canPickUpTile = true;
    [SerializeField] int pickupRange = 1;
    [SerializeField] int throwRange = 1;
    [SerializeField] Element heldTileElement;

    private void Start()
    {
        heldTileElement = Element.Void;


    }
    // Picks up a tile and stores what element it is
    public void PickUpTile(Tile tile) 
    {
        // Checks you have an action to pick up tile, the tile's element is void, and that its within range
        if (canPickUpTile && tile.getElement() != Element.Void && tile.gameObject.GetComponent<TileClickable>().GetDistance() <= pickupRange) 
        {
            // Cant pickup tile you are standing on
            if (tile.gameObject.transform.position != this.transform.position) 
            {
                Element temp = tile.getElement();
                tile.setElement(heldTileElement);
                heldTileElement = temp;
                if (tile.getElement() == Element.Void)
                {
                    tile.gameObject.GetComponent<TilePathNode>().isWalkable = false;
                }
                else
                {
                    tile.gameObject.GetComponent<TilePathNode>().isWalkable = true;
                }
                canPickUpTile = false;
                Debug.Log(heldTileElement);
            }
        }
    }

    // Called when player right clicks on a tile
    // Here you can call your own element interaction functions based on the 
    // tile game object passed in and the held tile or whatever way you want to implement it
    public void ThrowTile(GameObject tile) 
    {
        if (heldTileElement != Element.Void && tile.GetComponent<TileClickable>().GetDistance() <= throwRange) 
        {
            Debug.Log($"Threw a(n) {heldTileElement} tile at a {tile.GetComponent<Tile>().myElement}!");
            ReactionManager.catchElement(heldTileElement, tile);
            // Need to check if there is an enemy standing on this tile to deal base throw damage
            tile.GetComponent<TilePathNode>().isWalkable = true;
            heldTileElement = Element.Void;
        }
    }

    public void ResetActions() 
    {
        canPickUpTile = true;
    }
}
