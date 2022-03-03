using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy){
        Debug.Log("We are in IDLE STATE!!!!!!!!!!!!!!!!!!!!!!!!!");
    }

    public override void UpdateState(EnemyStateManager enemy){

        // we have to switch state once its enemy turn and no longer player turn. 
        // we just have to switch state to attack or move depending where the enemy is etc.... 
        // Or switch to attack state and check in attack if we are in range to attack, if not switch to move state and move closer. 

    }

    public override void OnCollosionEnter(EnemyStateManager enemy){

    }
}
