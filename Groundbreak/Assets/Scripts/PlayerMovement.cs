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
        UpdateTilesAfterMove();
        currentMovementRemaining = movementSpeed;
    }
    // Move player to a spot if they have the movement points remaining
    public void MovePlayer(int distance, float x, float y) 
    {
        if (distance <= currentMovementRemaining) 
        {
            currentMovementRemaining -= distance;
            gameObject.transform.SetPositionAndRotation(new Vector3(x, y, 0), new Quaternion(0, 0, 0, 0));
            UpdateTilesAfterMove();
        }
    }

    public void ResetMovement()
    {
        currentMovementRemaining = movementSpeed;
    }

    // Calculates the distance from each tile to the player
    // Needed if we want to say highlight tiles that they can move to
    private void UpdateTilesAfterMove()
    {
        TileClickable[] TileList = FindObjectsOfType<TileClickable>();
        foreach (TileClickable tile in TileList)
        {
            tile.updateDistanceToPlayer();
        }
    }
}
