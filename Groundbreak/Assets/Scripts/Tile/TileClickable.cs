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
    int effectIndicatorRange = 0;
    private void Start()
    {
        // Gets Reference to player
        Player = FindObjectOfType<PlayerMovement>().gameObject;
        turnLogic = FindObjectOfType<TurnLogic>();
    }

    // Unity event handler only triggers when a click raycast hits the tile.
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        // Dont do anything not on player turn
        if (!turnLogic.GetIsPlayerTurn()) 
        {
            return;
        }
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
        else if (turnLogic.isActivePhase) 
        {
            // Left click
            if (eventData.button == 0)
            {
                Player.GetComponent<PlayerActions>().UseActiveItem(ThisTile);
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
        lastTileHovered = ThisTile.GetComponent<Tile>();
        if (ThisTile.GetComponent<TilePathNode>() == null) 
        {
            return;
        }
        if (ThisTile.GetComponent<TilePathNode>().GetX() >= 10 || ThisTile.GetComponent<TilePathNode>().GetY() >= 10) 
        {
            return;
        }
        Player.GetComponent<PlayerMovement>().ShowLine(ThisTile.GetComponent<TilePathNode>().GetX(), ThisTile.GetComponent<TilePathNode>().GetY());
        
        if (turnLogic.isThrowPhase && lastTileHovered.GetComponent<TileClickable>().GetDistance() <= Player.GetComponent<PlayerActions>().throwRange) {
            Element activeElement = Player.GetComponent<PlayerActions>().heldTileElement;
            if (lastTileHovered != null && activeElement != Element.Void) {
                Element passiveElement = lastTileHovered.myElement;
                
                // Show reaction with enemy
                if (lastTileHovered.gameObjectAbove != null && lastTileHovered.gameObjectAbove.tag == "Enemy") {
                    passiveElement = lastTileHovered.gameObjectAbove.GetComponent<EnemyStateManager>().myElement;
                } 
                
                Debug.Log("Active elem" + activeElement);
                Debug.Log("Passive elem" + passiveElement);
                switch((int)activeElement + (int)passiveElement) {
                    case ((int)Element.Air + (int)Element.Earth): // Sandstorm 
                        effectIndicatorRange = ReactionManager.SANDSTORM_RANGE;
                        break;
                    case ((int)Element.Earth + (int)Element.Fire): // Magma 
                        effectIndicatorRange = 0;
                        break;
                    case ((int)Element.Water + (int)Element.Earth): // Mud
                        effectIndicatorRange = 0;
                        break;
                    case ((int)Element.Water + (int)Element.Fire): // Smoke 
                        effectIndicatorRange = 0;
                        break;
                    case ((int)Element.Air + (int)Element.Fire): // Fireball 
                        effectIndicatorRange = ReactionManager.FIREBALL_RANGE;
                        break;
                    case ((int)Element.Air + (int)Element.Water): // Storm 
                        effectIndicatorRange = ReactionManager.STORM_RANGE;
                        break;
                    default: // Same elem reaction
                        effectIndicatorRange = 1;
                        break;
                }

                lastTileHovered.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.yellow;
                showRange(effectIndicatorRange, lastTileHovered, new List<Tile>());
            }     
        }
    }

    public void showRange(int range, Tile startTile, List<Tile> neighborsVisited){
        // Base case: We've looked as many tiles away as desired
        if (range == 0){
            startTile.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.yellow;
            return;
        }

        // Look at current tiles neighbors
        foreach(Tile neighbor in startTile.neighbors){
            // If haven't visited before, color
            if (!neighborsVisited.Contains(neighbor)){
                Debug.Log($"Unvisited tile at {neighbor.name}");
                neighbor.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.yellow;
                neighborsVisited.Add(startTile);

                // Repeat but looking at one less set of neighbors, starting at neighbor
                showRange(range - 1, neighbor, neighborsVisited);
            }
        }
    }


    public void clearEffectRangeIndicator(){
        foreach (Tile t in ReactionManager.gridManager.grid){
            // In throw range
            bool hasTile = Player.GetComponent<PlayerActions>().heldTileElement != Element.Void;
            if (Player.GetComponent<PlayerActions>().throwRange >= t.GetComponent<TileClickable>().GetDistance() && hasTile){
                t.transform.GetChild(1).GetComponent<Renderer>().material.color = Color.white;
            } else { // Outside throw range
                t.transform.GetChild(1).GetComponent<Renderer>().material.color = Color.clear;
            }
        }
    }

    // Clear line when mouse leaves the tile
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData) 
    {
        if (Player.GetComponent<PlayerMovement>() != null)
        {
            Player.GetComponent<PlayerMovement>().ClearLine();
        }
        Debug.Log($"Exit");
        
        if (turnLogic.isThrowPhase){
            clearEffectRangeIndicator();
        }     
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
