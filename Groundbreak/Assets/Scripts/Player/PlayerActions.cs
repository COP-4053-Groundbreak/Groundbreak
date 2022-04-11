using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerActions : MonoBehaviour
{
    public bool canPickUpTile = true;
    public bool canUseActive = true;
    [SerializeField] int pickupRange = 1;
    [SerializeField] public int throwRange = 1;
    [SerializeField] Element heldTileElement;
    GameObject[] enemyList;
    TurnLogic turnLogic;
    [SerializeField] HoldPlayerStats playerStats;
    Animator playerAnimator;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        turnLogic = FindObjectOfType<TurnLogic>();
        heldTileElement = Element.Void;
        enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        FindObjectOfType<FindNewGridManager>().OnGridChanged += GridChanged;

    }

    private void Update()
    {
        if (!canUseActive) 
        {
            turnLogic.activeButton.interactable = false;
        }
    }

    // Picks up a tile and stores what element it is
    public void PickUpTile(Tile tile) 
    {
        tile.gameObject.GetComponent<TileClickable>().updateDistanceToPlayer();
        // Checks its your turn, you have an action to pick up tile, the tile's element is void, and that its within range
        if (turnLogic.GetIsPlayerTurn() && canPickUpTile && tile.getElement() != Element.Void && tile.gameObject.GetComponent<TileClickable>().GetDistance() <= pickupRange) 
        {
            
            // Cant pickup tile with somehting on top of it
            if (tile.gameObjectAbove == null)
            {
                playerAnimator.SetTrigger("PickUp");
                SoundManagerScript.PlaySound("pickuptile");
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
        Debug.Log($"Attemptint to throw a {tile.GetComponent<Tile>().myElement}");
        // Debug.LogWarning(tile.GetComponent<TileClickable>().GetDistance());
        if (turnLogic.GetIsPlayerTurn() && heldTileElement != Element.Void && tile.GetComponent<TileClickable>().GetDistance() <= throwRange) 
        {
            playerAnimator.SetTrigger("Throw");
            SoundManagerScript.PlaySound("throwtile");
            Debug.Log($"Actually threw a {tile.GetComponent<Tile>().myElement}");
            ReactionManager.catchElement(heldTileElement, tile);
            Debug.Log("Out of catch element");
            // Need to check if there is an enemy standing on this tile to deal base throw damage
            tile.GetComponent<TilePathNode>().isWalkable = true;
            heldTileElement = Element.Void;
            FindObjectOfType<DisplayHeldTile>().ClearTile();
        }
        
    }

    public void UseActiveItem(GameObject tile) 
    {
        if (!canUseActive) 
        {
            return;
        }
        tile.GetComponent<TileClickable>().updateDistanceToPlayer();
        switch (playerStats.playerActiveItem.itemName) 
        {
            
            case ActiveItem.ActiveItemName.Sword:
                if (tile.GetComponent<Tile>().gameObjectAbove && tile.GetComponent<Tile>().gameObjectAbove.CompareTag("Enemy") && tile.GetComponent<TileClickable>().GetDistance() <= 1) 
                {
                    tile.GetComponent<Tile>().gameObjectAbove.GetComponent<EnemyStateManager>().DealDamage(10);
                    canUseActive = false;
                    playerAnimator.SetTrigger("Attack");
                    SoundManagerScript.PlaySound("playerSword");
                    StartCoroutine(Duration(1, turnLogic.turnCount));
                }
                break;
            case ActiveItem.ActiveItemName.Bow:
                if (tile.GetComponent<Tile>().gameObjectAbove && tile.GetComponent<Tile>().gameObjectAbove.CompareTag("Enemy") && tile.GetComponent<TileClickable>().GetDistance() <= 3)
                {
                    tile.GetComponent<Tile>().gameObjectAbove.GetComponent<EnemyStateManager>().DealDamage(5);
                    canUseActive = false;
                    playerAnimator.SetTrigger("Ranged");
                    SoundManagerScript.PlaySound("playerBow");
                    StartCoroutine(Duration(1, turnLogic.turnCount));
                }
                break;
            case ActiveItem.ActiveItemName.BlinkRune:
                if (tile.GetComponent<Tile>().gameObjectAbove == null && tile.GetComponent<TileClickable>().GetDistance() <= 3)
                {
                    gameObject.GetComponent<PlayerMovement>().TeleportTo(tile);
                    canUseActive = false;
                    SoundManagerScript.PlaySound("playerTeleport");
                    StartCoroutine(Duration(3, turnLogic.turnCount));

                }
                break;
            case ActiveItem.ActiveItemName.FireballScroll:
                if (tile.GetComponent<Tile>().gameObjectAbove == null && tile.GetComponent<TileClickable>().GetDistance() <= throwRange)
                {
                    Element temp = tile.GetComponent<Tile>().getElement();
                    tile.GetComponent<Tile>().setElement(Element.Air);
                    ReactionManager.TileOnTile(Element.Fire, tile.GetComponent<Tile>());
                    canUseActive = false;
                    tile.GetComponent<Tile>().setElement(temp);
                    StartCoroutine(Duration(4, turnLogic.turnCount));
                }
                break;
            case ActiveItem.ActiveItemName.RepulsionWand:
                if (tile.GetComponent<Tile>().gameObjectAbove && tile.GetComponent<Tile>().gameObjectAbove.CompareTag("Enemy") && tile.GetComponent<TileClickable>().GetDistance() <= 3)
                {
                    Vector2 diff = new Vector2(tile.GetComponent<Tile>().gameObjectAbove.GetComponent<EnemyStateManager>().enemyX -  gameObject.GetComponent<PlayerMovement>().playerX,
                                                tile.GetComponent<Tile>().gameObjectAbove.GetComponent<EnemyStateManager>().enemyY - gameObject.GetComponent<PlayerMovement>().playerY).normalized;
                    ReactionManager.pushGO(gameObject, diff, 1, tile.GetComponent<Tile>().gameObjectAbove);
                    canUseActive = false;
                    StartCoroutine(Duration(2, turnLogic.turnCount));
                }
                break;
            case ActiveItem.ActiveItemName.AttractionWand:
                if (tile.GetComponent<Tile>().gameObjectAbove && tile.GetComponent<Tile>().gameObjectAbove.CompareTag("Enemy") && tile.GetComponent<TileClickable>().GetDistance() <= 3)
                {
                    Vector2 diff = new Vector2(tile.GetComponent<Tile>().gameObjectAbove.GetComponent<EnemyStateManager>().enemyX - gameObject.GetComponent<PlayerMovement>().playerX,
                                                tile.GetComponent<Tile>().gameObjectAbove.GetComponent<EnemyStateManager>().enemyY - gameObject.GetComponent<PlayerMovement>().playerY).normalized;
                    ReactionManager.pullGO(gameObject, -diff, 1, tile.GetComponent<Tile>().gameObjectAbove);
                    canUseActive = false;
                    StartCoroutine(Duration(2, turnLogic.turnCount));
                }
                break;
        }
        
    }

    public void ResetActions() 
    {
        canPickUpTile = true;
    }

    IEnumerator Duration(int duration, int startTurn)
    {
        
        yield return new WaitUntil(() => Check(startTurn, duration));
        canUseActive = true;
        turnLogic.activeButton.interactable = true;
    }


    private bool Check(int startTurn, int duration)
    {
        //Debug.LogError(turnLogic.turnCount + " " + startTurn + " " + duration + " " + (duration + startTurn));
        if (turnLogic.turnCount == startTurn + duration || !turnLogic.isCombatPhase)
        {
            return true;
        }
        return false;
    }

    private void GridChanged(object sender, System.EventArgs e)
    {
        canUseActive = true;
    }

}
