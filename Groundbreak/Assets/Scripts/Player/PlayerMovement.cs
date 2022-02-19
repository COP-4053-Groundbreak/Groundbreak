using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // 10 movement speed is 1 tile, used so we can have say 2 tiles take 3 points of movement without needing float math
    [SerializeField] int movementSpeed = 20;
    Pathfinding pathfinding;
    int currentMovementRemaining;
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] GameObject pathHolder;
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
        
        TilePathNode endNode = path[0];
        // If path cost is less than movement remaining, take it
        if (endNode.fCost <= currentMovementRemaining)
        {
            currentMovementRemaining -= endNode.fCost;
            gameObject.transform.SetPositionAndRotation(new Vector3(x, y, 0), new Quaternion(0, 0, 0, 0));
            UpdateTilesAfterMove();
        }
    }

    // Draws line along the path the character takes
    public void ShowLine(float x, float y) 
    {
        List<Transform> path = pathfinding.FindPathWaypoints((int)gameObject.transform.position.x, (int)gameObject.transform.position.y, (int)x, (int)y);
        if (path != null)
        {
            path.Add(transform);
            Transform endNode = path[0];
            if (endNode.gameObject.GetComponent<TilePathNode>().fCost <= currentMovementRemaining)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    GameObject thisArrow;
                    if (path[i].position.x > path[i + 1].position.x)
                    {
                        Debug.Log("right");
                        thisArrow = Instantiate(arrowPrefab, path[i].position, Quaternion.Euler(new Vector3(0, 0, 0)), pathHolder.transform);
                        thisArrow.transform.position += new Vector3(-0.5f, 0, 0);
                    }
                    else if (path[i].position.x < path[i + 1].position.x)
                    {
                        Debug.Log("left");
                        thisArrow = Instantiate(arrowPrefab, path[i].position, Quaternion.Euler(new Vector3(0, 0, 180)), pathHolder.transform);
                        thisArrow.transform.position += new Vector3(0.5f, 0, 0);
                    }
                    else if (path[i].position.y > path[i + 1].position.y)
                    {
                        Debug.Log("up");
                        thisArrow = Instantiate(arrowPrefab, path[i].position, Quaternion.Euler(new Vector3(0, 0, 90)), pathHolder.transform);
                        thisArrow.transform.position += new Vector3(0, -0.5f, 0);
                    }
                    else if (path[i].position.y < path[i + 1].position.y)
                    {
                        Debug.Log("down");
                        thisArrow = Instantiate(arrowPrefab, path[i].position, Quaternion.Euler(new Vector3(0, 0, -90)), pathHolder.transform);
                        thisArrow.transform.position += new Vector3(0, 0.5f, 0);
                    }
                    else 
                    {
                        Debug.Log("err");
                    }
                    
                }
            }
        }
    }

    public void ClearLine() 
    {
        foreach (Transform child in pathHolder.transform) 
        {
            Destroy(child.gameObject);
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
