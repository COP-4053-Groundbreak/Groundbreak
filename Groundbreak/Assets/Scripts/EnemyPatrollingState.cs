using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrollingState : EnemyBaseState
{
    public float speed = 1f;

    public override void EnterState(EnemyStateManager enemy){
        Debug.Log("Hello from the EnterState method of the Patrolling state!!!");
    }

    public override void UpdateState(EnemyStateManager enemy, Rigidbody2D rigidbody2D){
        // Dummy movement to show that we can switch states from patrolling to idle, going to change this movement logic later. 
        Vector2 position = rigidbody2D.position;
        position.y = position.y + Time.deltaTime * speed;
        rigidbody2D.MovePosition(position);
        // Debug.Log(position.y);

        // if we get in range of enemy or something we want to switch state
        //   if(enemy.isInRangeOfPlayer){
        //       enemy.SwitchState(enemy.AttackState OR enemy.IdleState)
        //   }

        // for testing, lets just switch states after a little movement.
        if(position.y >= 1.0f){
            enemy.SwitchState(enemy.IdleState);
        }
    }

    public override void OnCollosionEnter(EnemyStateManager enemy){

    }
}
