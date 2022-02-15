using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // 10 movement speed is 1 tile, used so we can have say 2 tiles take 3 points of movement without needing float math
    [SerializeField] int movementSpeed = 20;
    Pathfinding pathfinding;
    int currentMovementRemaining;
    // Start is called before the first frame update
    private void Start()
    {
        UpdateTilesAfterMove();
        pathfinding = FindObjectOfType<Pathfinding>();
        currentMovementRemaining = movementSpeed;
    }
    // Move player to a spot if they have the movement points remaining
    public void MovePlayer(int distance, float x, float y) 
    {
        // Find a path
        List<TilePathNode> path = pathfinding.FindPath((int)gameObject.transform.position.x, (int)gameObject.transform.position.y, (int)x, (int)y);
        if (path == null) 
        {
            return;
        }
        
        TilePathNode endNode = path[path.Count - 1];
        // If path cost is less than movement remaining, take it
        if (endNode.fCost <= currentMovementRemaining)
        {
            currentMovementRemaining -= endNode.fCost;
            gameObject.transform.SetPositionAndRotation(new Vector3(x, y, 0), new Quaternion(0, 0, 0, 0));
            UpdateTilesAfterMove();
        }
    }

    public void ShowLine(float x, float y) 
    {
        List<TilePathNode> path = pathfinding.FindPath((int)gameObject.transform.position.x, (int)gameObject.transform.position.y, (int)x, (int)y);
        if (path != null)
        {
            TilePathNode endNode = path[path.Count - 1];
            Debug.Log(endNode.fCost + " , " + currentMovementRemaining);
            if (endNode.fCost <= currentMovementRemaining)
            {
                
                for (int i = 0; i < path.Count - 1; i++)
                {
                    // Can uncomment to view the path that it takes, generates a 1px line along the path to the hovered tile
                    // Here we draw the line to the hovered over node
                    Debug.DrawLine(new Vector3(path[i].GetX(), path[i].GetY()), new Vector3(path[i + 1].GetX(), path[i + 1].GetY()), Color.black, 1, false);
                }
            }
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
