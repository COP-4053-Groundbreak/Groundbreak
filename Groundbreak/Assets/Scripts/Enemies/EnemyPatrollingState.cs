using System;
using System.Timers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrollingState : EnemyBaseState
{
     public float period = 0.0f;
     public int height = 0;
     public int max_height = 2;

    public override void EnterState(EnemyStateManager enemy){
        Debug.Log("Hello from the EnterState method of the Patrolling state!!!");
        // enemy.MoveEnemy(0,1);
    }

    public override void UpdateState(EnemyStateManager enemy){

        List<Transform> slidingPath = enemy.slidingPath;
        bool isSliding = enemy.isSliding;

        if (isSliding) 
        {
            enemy.SlideThisObjectAlongPath(slidingPath);
        }

        float y_value = enemy.transform.position.y;
        if (period >= 3 && height != max_height)
        {
         //Do Stuff
            enemy.MoveEnemy(0, y_value + 1);
            height += 1;
            period = 0;
        }
        period += UnityEngine.Time.deltaTime;

    }

    public override void OnCollosionEnter(EnemyStateManager enemy){

    }
}

