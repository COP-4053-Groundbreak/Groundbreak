using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public bool validPoint = true;

    public void setInvalid(EnemySpawner currentPoint)
    {
        currentPoint.validPoint = false;
    }
}