using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnLogic : MonoBehaviour
{
    bool isPlayerTurn = true;
    float dummyTurnTime = .01f;
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
        // set enemy turn to true (testing state logic)
        EnemyIdleState.isEnemyTurn = true;
        yield return new WaitForSeconds(dummyTurnTime);
        Debug.Log("AI ended their turn");
        // set enemy turn to false. (testing state logic)
        EnemyIdleState.isEnemyTurn = false;
        isPlayerTurn = true;
    }
}
