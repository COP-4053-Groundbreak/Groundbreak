using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] int movementSpeed = 2;
    int currentMovementRemaining;
    // Start is called before the first frame update
    private void Start()
    {
        currentMovementRemaining = movementSpeed;
    }
    public void MovePlayer(int distance, float x, float y) 
    {
        if (distance <= currentMovementRemaining) 
        {
            currentMovementRemaining -= distance;
            gameObject.transform.SetPositionAndRotation(new Vector3(x, y, 0), new Quaternion(0, 0, 0, 0));
        }
    }

    public void ResetMovement()
    {
        currentMovementRemaining = movementSpeed;
    }
}
