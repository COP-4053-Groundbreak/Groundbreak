using System;
using System.Timers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrollingState : EnemyBaseState
{
     public float period = 0.0f;
     System.Random random = new System.Random(); 

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
        float x_value = enemy.transform.position.x;
        // Debug.Log("X-Value current : " + x_value);
        if (period >= 3)
        {
            // deal damage, every 3 seconds for testing. 
            enemy.DealDamage(5);
            // enemy.healthSystem.Damage(20);
            // Debug.Log("X-Value current : " + x_value);
     
            int random_num_x = random.Next(0,2);
            int random_num_y = random.Next(0,2);
            // Debug.Log("X-Value random 1: " + random_num_x);
            // Debug.Log("Y-Value random 1: " + random_num_y);
            var x_random = random_num_x;
            var y_random = random_num_y;
            
            //Check if we are all the way left or right. (X) readjust if neeeded
            if(x_random + x_value <= 0){
                // so if the random is less than the width, lets make it 1 to move right
                x_random = 0;
            }
            if(x_random + x_value >  enemy.width - 1){
                // so if the random is more than the width, lets make it -1 to move left
                x_random = -1;
            }
            if(y_random + y_value <= 0){
                // so if the random is less than the width, lets make it 1 to move up
                y_random = 0;
            }
            if(y_random + y_value > enemy.height - 1){
                // so if the random is less than the height, lets make it 1 to move down
                y_random = -1;
            }

            // Debug.Log("Y-Value random 2: " + random_num_y);

            
            enemy.MoveEnemy(x_value + x_random, y_value + y_random);
            period = 0;
        }
        period += UnityEngine.Time.deltaTime;

    }

    public override void OnCollosionEnter(EnemyStateManager enemy){

    }
}
