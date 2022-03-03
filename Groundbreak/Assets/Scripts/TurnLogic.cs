using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnLogic : MonoBehaviour
{
    bool isPlayerTurn = true;
    // Determines what action the player is currently performing
    public bool isMovementPhase = true;
    public bool isThrowPhase = false;
    // Determines if combat is taking place
    public bool isCombatPhase = false;

    float dummyTurnTime = 3f;
    // Player movment and actions references
    PlayerMovement playerMovement;
    PlayerActions playerActions;

    // enemy state manager
    // EnemyStateManager enemyStateManager;
    EnemyStateManager[] enemyStateManagers;



    // Buttons and canvass references 
    [SerializeField] Button endTurnButton;
    [SerializeField] Button moveButton;
    [SerializeField] Button throwTileButton;
    [SerializeField] GameObject battleCanvas;

    // List of all tiles for free roaming pathfinding
    Tile[] tiles;
    // Empty gameobject to instantiate under void tiles to give them colliders
    [SerializeField] GameObject empty;

    private void Start()
    {
        // Start in movephase and grey out move button
        moveButton.interactable = false;
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerActions = FindObjectOfType<PlayerActions>();
        // grab all the enemy state managers in the room. 
        enemyStateManagers = UnityEngine.Object.FindObjectsOfType<EnemyStateManager>();
        StartCombat();
    }

    // Called by end turn button
    public void EndTurnPressed() 
    {
        if (isPlayerTurn) 
        {
            Debug.Log("Player ended their turn");
            isPlayerTurn = false;

            // Disable End Turn Button
            endTurnButton.interactable = false;

            // Dummy coroutine to simulate ai turn
            // Replace with actual enemy turn logic function call(s) when they exist

            StartCoroutine(DummyEnemyTurn());
        }
    }

    // Switch to move phase
    public void MovePressed()
    {
        moveButton.interactable = false;
        throwTileButton.interactable = true;

        isMovementPhase = true;
        isThrowPhase = false;
    }

    // Switch to throw / pick up phase
    public void ThrowPressed()
    {
        if (!throwTileButton) 
        {
            Debug.Log("HERE");
        }
        throwTileButton.interactable = false;
        moveButton.interactable = true;

        isMovementPhase = false;
        isThrowPhase = true;
    }

    IEnumerator DummyEnemyTurn() 
    {
        Debug.Log("AI started their turn");
        // enemyStateManager.isEnemyTurn = true;
        // Set enemy turn to true. 
        for(int i = 0; i < enemyStateManagers.Length; i++){
            enemyStateManagers[i].isEnemyTurn = true;
        }
        // return from function and come back here. 
        yield return new WaitForSeconds(dummyTurnTime);
        // set attack counter back to 0 so we can attack again if the player is still in range next turn. 
        for(int i = 0; i < enemyStateManagers.Length; i++){
            enemyStateManagers[i].attackCounter = 0;
        }
        Debug.Log("AI ended their turn");

        // Reset player's movement points for new turn
        playerMovement.ResetMovement();
        playerActions.ResetActions();
        // setting turn back to false. 
        for(int i = 0; i < enemyStateManagers.Length; i++){
            enemyStateManagers[i].isEnemyTurn = false;
        }
        isPlayerTurn = true;

        // Enable End Turn Button
        endTurnButton.interactable = true;
        playerMovement.PossiblyShowTile();

    }

    // Starts combat in a room
    public void StartCombat()
    {
        battleCanvas.SetActive(true);
        isCombatPhase = true;
        // Probably not needed as we never start combat in the same room again but just incase
        DestroyVoidColliders();
    }

    // Ends combat in a room and generates colliders for impassable terrain
    public void EndCombat()
    {
        battleCanvas.SetActive(false);
        isCombatPhase = false;
        CreateVoidColliders();
    }

    // Destroys all children of void tiles
    void DestroyVoidColliders() 
    {
        tiles = FindObjectsOfType<Tile>();
        GameObject[] allChildren = new GameObject[transform.childCount];
        int i = 0;

        foreach (Tile tile in tiles)
        {
            if (tile.getElement() == Element.Void)
            {
                i = 0;
                //Find all child obj and store to that array
                foreach (Transform child in transform)
                {
                    allChildren[i] = child.gameObject;
                    i += 1;
                }
                //Now destroy them
                foreach (GameObject child in allChildren)
                {
                    DestroyImmediate(child.gameObject);
                }
            }
        }




    }

    // Creates children for void tiles and gives them box colliders
    void CreateVoidColliders() 
    {
        tiles = FindObjectsOfType<Tile>();
        foreach (Tile tile in tiles)
        {
            if (tile.getElement() == Element.Void)
            {
                Transform child = Instantiate(empty.transform, tile.transform);
                child.gameObject.AddComponent<BoxCollider2D>();
            }
        }
    }

}
