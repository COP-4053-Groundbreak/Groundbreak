using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy){
        Debug.Log("We have switched from Patrolling to IDLE State!!!!");
    }

    public override void UpdateState(EnemyStateManager enemy, Rigidbody2D rigidbody2D){

    }

    public override void OnCollosionEnter(EnemyStateManager enemy){

    }
}
