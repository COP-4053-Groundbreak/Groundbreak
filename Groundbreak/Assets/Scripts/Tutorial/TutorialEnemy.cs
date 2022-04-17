using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemy : MonoBehaviour
{
    private void OnDestroy()
    {
        if (FindObjectsOfType<EnemyStateManager>().Length <= 0) 
        {
            FindObjectOfType<TutorialManager>().RoomCleared();
        }
    }
}
