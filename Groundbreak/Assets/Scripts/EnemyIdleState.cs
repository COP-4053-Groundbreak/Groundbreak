using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public static bool isEnemyTurn = false;
    public override EnemyState RunCurrentState()
    {
        if (isEnemyTurn == true)
        {
            // if in range we will attack
            Debug.Log("The Enemy has attacked the player!!!");
            // if not in range we will move closer
            return this;
        }
        else{
            // Debug.Log("The Enemy is in Idle mode.....");
            return this;
        }
    }
}
