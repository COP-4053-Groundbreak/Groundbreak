using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyDeathState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy){
        // play death animation, maybe that will be handled elsewhere,
        // Destroy enemy object. 
        Debug.Log("DEAD STATE WE ARE DYING!");
        enemy.alive = false;
        enemy.animator.SetBool("alive", false);
        if(enemy.gameObject.name.Contains("Zombie")){
            SoundManagerScript.PlaySound("zombiedeath");
        }
        if(enemy.gameObject.name.Contains("Skeleton")){
            SoundManagerScript.PlaySound("skeletondeath");
        }
        if(enemy.gameObject.name.Contains("Tree")){
            SoundManagerScript.PlaySound("treeDead");
        }
        if(enemy.gameObject.name.Contains("Goblin")){
            SoundManagerScript.PlaySound("goblinDead");
        }
        // wait 1.8 seconds for animation to play TAKE LOOP OFF AND DESTROY LATER. 
    }

    // we might not need these two overridden functions for death. Will have to see when i implement it.

    public override void UpdateState(EnemyStateManager enemy){

    }

    public override void OnCollosionEnter(EnemyStateManager enemy){

    }
}
