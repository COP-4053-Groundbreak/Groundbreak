using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrollingState : EnemyBaseState
{
    public float speed = 1f;

    public override void EnterState(EnemyStateManager enemy){
        Debug.Log("Hello from the EnterState method of the Patrolling state!!!");
        // enemy.MoveEnemy(0,1);
    }

    public override void UpdateState(EnemyStateManager enemy){
        enemy.MoveEnemy(0,1);
    }

    public override void OnCollosionEnter(EnemyStateManager enemy){

    }
}
