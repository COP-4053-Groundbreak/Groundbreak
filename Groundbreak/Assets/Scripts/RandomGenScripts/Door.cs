using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform destination;
    [SerializeField] private int doorDirection;
    /*
    1 -->need bottom door
    2 -->need top door
    3 -->need left door
    4 -->need right door
     */

    public Transform GetDestination()
    {
        return destination;
    }

    public Transform ChangeDestination(GameObject currentDoor, Transform newDestination)
    {
        return null;
    }
}
