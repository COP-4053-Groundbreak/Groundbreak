using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState
{
    public abstract void EnterState(EnemyStateManager enemy);

    public abstract void UpdateState(EnemyStateManager enemy, Rigidbody2D rigidbody2D);
    // Maybe this can be OnRadiusEnter when the player comes close to the enemy. Enemy switches from patrolling state to idle/attack state depending on who attacks first.
    public abstract void OnCollosionEnter(EnemyStateManager enemy); 
}
