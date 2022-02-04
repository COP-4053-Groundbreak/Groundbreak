using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnLogic : MonoBehaviour
{
    bool isPlayerTurn = true;
    float dummyTurnTime = 1f;
    // Called by end turn button
    public void EndTurnPressed() 
    {
        if (isPlayerTurn) 
        {
            Debug.Log("Player ended their turn");
            isPlayerTurn = false;
            // Dummy coroutine to simulate ai turn
            // Replace with actual enemy turn logic function call(s) when they exist
            StartCoroutine(DummyEnemyTurn());
        }
    }

    IEnumerator DummyEnemyTurn() 
    {
        Debug.Log("AI started their turn");
        yield return new WaitForSeconds(dummyTurnTime);
        Debug.Log("AI ended their turn");
        isPlayerTurn = true;
    }
}
