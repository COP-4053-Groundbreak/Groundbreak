using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnLogic : MonoBehaviour
{
    bool isPlayerTurn = true;
    public bool isMovementPhase = true;
    public bool isThrowPhase = false;
    float dummyTurnTime = 1f;
    PlayerMovement playerMovement;
    PlayerActions playerActions;

    [SerializeField] Button endTurnButton;
    [SerializeField] Button moveButton;
    [SerializeField] Button throwTileButton;
    private void Start()
    {
        moveButton.interactable = false;
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerActions = FindObjectOfType<PlayerActions>();
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

    }
}
