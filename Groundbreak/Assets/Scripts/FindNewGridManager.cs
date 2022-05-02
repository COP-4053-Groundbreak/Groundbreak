using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FindNewGridManager : MonoBehaviour
{
    public event EventHandler OnGridChanged;
    [SerializeField] GameObject text;
    public void ChangedRoom() 
    {
        if (OnGridChanged != null)
        {
            OnGridChanged(this, EventArgs.Empty);
            text.GetComponent<DisplayInitiative>().ResetRepeat();
        }

        EnemyStateManager[] enemies = FindObjectsOfType<EnemyStateManager>();
        foreach (EnemyStateManager enemy in enemies) 
        {
            ReactionManager.gridManager.grid[enemy.enemyX, enemy.enemyY].gameObjectAbove = enemy.gameObject;
        }
    }
}
