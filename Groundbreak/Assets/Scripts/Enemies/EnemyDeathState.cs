using UnityEngine;

public class EnemyDeathState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy){
        // play death animation, maybe that will be handled elsewhere,
        // Destroy enemy object. 

    }

    // we might not need these two overridden functions for death. Will have to see when i implement it.

    public override void UpdateState(EnemyStateManager enemy){

    }

    public override void OnCollosionEnter(EnemyStateManager enemy){

    }
}
