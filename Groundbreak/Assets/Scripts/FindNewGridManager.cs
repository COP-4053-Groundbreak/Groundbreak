using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FindNewGridManager : MonoBehaviour
{
    public event EventHandler OnGridChanged;
    [SerializeField] GameObject text;

    GameObject currRoom;
    Vector2 localPos;
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
            currRoom = FindObjectOfType<GridManager>().gameObject.transform.parent.gameObject;
            localPos = currRoom.transform.InverseTransformPoint(enemy.transform.position);
            enemy.enemyX = (int)(localPos.x + 5.5f);
            enemy.enemyY = (int)(localPos.y + 5.5f);
            Debug.LogError("Setting at " + enemy.enemyX + " " + enemy.enemyY);
            ReactionManager.gridManager.grid[enemy.enemyX, enemy.enemyY].gameObjectAbove = enemy.gameObject;
        }
    }
}
