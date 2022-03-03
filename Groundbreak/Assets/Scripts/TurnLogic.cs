using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnLogic : MonoBehaviour
{
    bool isPlayerTurn = true;
    public bool isMovementPhase = true;
    public bool isThrowPhase = false;
    public bool isCombatPhase = false;
    float dummyTurnTime = 1f;
    PlayerMovement playerMovement;
    PlayerActions playerActions;

    [SerializeField] Button endTurnButton;
    [SerializeField] Button moveButton;
    [SerializeField] Button throwTileButton;
    [SerializeField] GameObject battleCanvas;

    Tile[] tiles;
    [SerializeField] GameObject empty;

    private void Start()
    {
        moveButton.interactable = false;
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerActions = FindObjectOfType<PlayerActions>();
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

    public void MovePressed()
    {
        moveButton.interactable = false;
        throwTileButton.interactable = true;

        isMovementPhase = true;
        isThrowPhase = false;
    }

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
        yield return new WaitForSeconds(dummyTurnTime);
        Debug.Log("AI ended their turn");

        // Reset player's movement points for new turn
        playerMovement.ResetMovement();
        playerActions.ResetActions();
        isPlayerTurn = true;

        // Enable End Turn Button
        endTurnButton.interactable = true;
        playerMovement.PossiblyShowTile();

    }
    public void StartCombat()
    {
        battleCanvas.SetActive(true);
        isCombatPhase = true;
    }

    public void EndCombat()
    {
        battleCanvas.SetActive(false);
        isCombatPhase = false;
        CreateVoidColliders();
    }

    void DestroyVoidColliders() 
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
