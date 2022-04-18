using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemy : MonoBehaviour
{
    private void OnDestroy()
    {
        if (FindObjectsOfType<EnemyStateManager>() != null && FindObjectsOfType<EnemyStateManager>().Length <= 0) 
        {
            if (FindObjectOfType<TutorialManager>() != null) 
            {
                FindObjectOfType<TutorialManager>().RoomCleared();
            }
        }
    }
}
