using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding: MonoBehaviour
{
    int MOVE_COST = 10;
    private List<TilePathNode> openList;
    private List<TilePathNode> closedList;
    public int width = 10;
    public int height = 10;
    GridManager currRoom;

    private void Start()
    {
        //width = FindObjectOfType<GridManager>().getWidth();
        //height = FindObjectOfType<GridManager>().getHeight();
        currRoom = FindObjectOfType<GridManager>();
    }
    public List<TilePathNode> FindPath(int startX, int startY, int endX, int endY) 
    {
        Tile[,] grid = currRoom.getGrid();
        TilePathNode startNode = currRoom.getTile(startX, startY).gameObject.GetComponent<TilePathNode>();
        TilePathNode endNode = currRoom.getTile(endX, endY).gameObject.GetComponent<TilePathNode>();

        // List of seen nodes and completly seen nodes
        openList = new List<TilePathNode> { startNode };
        closedList = new List<TilePathNode>();

        // Reset path costs
        for (int x = 0; x < width; x++) 
        {
            for (int y = 0; y < height; y++) 
            {
                TilePathNode tilePathNode = currRoom.getTile(x, y).gameObject.GetComponent<TilePathNode>();
                tilePathNode.gCost = int.MaxValue;
                tilePathNode.CalculateFCost();
                tilePathNode.previousNode = null;
            }
        }

        // Initilize start cost
        startNode.gCost = 0;
        startNode.hCost = CalculateDistance(startNode, endNode);
        startNode.CalculateFCost();

        // While we have seen nodes
        while (openList.Count > 0) 
        {
            // Find the lowest Fcost node we have
            TilePathNode currentNode = GetLowestFCost(openList);
            // If that node is the end, retrace the path
            if (currentNode == endNode) 
            {
                return CalculatePath(endNode);
            }

            // remove it from the seen nodes and put it on the finished nodes
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            // for each tile in cardnal directions from this one
            foreach (TilePathNode neighborNode in GetNeighborList(currentNode, grid)) 
            {
                if (closedList.Contains(neighborNode)) 
                {
                    continue;
                }

                if (neighborNode.isWalkable == false) 
                {
                    closedList.Add(neighborNode);
                    continue;
                }

                // calculate the cost it would take to get to that neighbor node using this tile
                // If its cheaper than its already existing cost, update it
                int tempGCost = currentNode.gCost + CalculateDistance(currentNode, neighborNode);
                if (tempGCost < neighborNode.gCost) 
                {
                    neighborNode.previousNode = currentNode;
                    neighborNode.gCost = tempGCost;
                    neighborNode.hCost = CalculateDistance(neighborNode, endNode);
                    neighborNode.CalculateFCost();

                    // if we havent seen this node yet, add it to the list
                    if (!openList.Contains(neighborNode)) 
                    {
                        openList.Add(neighborNode);
                    }
                }
            }
        }
        // No Path Found
        return null;
    }


    public List<Transform> FindPathWaypoints(int startX, int startY, int endX, int endY)
        {
            Tile[,] grid = FindObjectOfType<GridManager>().getGrid();
            Debug.Log(startX + " " + startY + " " + endX + " " + endY);
            //Debug.Log("HERE" + grid[9, 9]);
            TilePathNode startNode = grid[startX, startY].gameObject.GetComponent<TilePathNode>();
            TilePathNode endNode = grid[endX, endY].gameObject.GetComponent<TilePathNode>();

            // List of seen nodes and completly seen nodes
            openList = new List<TilePathNode> { startNode };
            closedList = new List<TilePathNode>();

            // Reset path costs
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (!grid[x, y])
                    {
                        Debug.LogWarning(grid[x, y]);
                    }
                    TilePathNode tilePathNode = grid[x, y].gameObject.GetComponent<TilePathNode>();
                    tilePathNode.gCost = int.MaxValue;
                    tilePathNode.CalculateFCost();
                    tilePathNode.previousNode = null;
                }
            }


            // Initilize start cost
            startNode.gCost = 0;
        startNode.hCost = CalculateDistance(startNode, endNode);
        startNode.CalculateFCost();

        // While we have seen nodes
        while (openList.Count > 0)
        {
            // Find the lowest Fcost node we have
            TilePathNode currentNode = GetLowestFCost(openList);
            // If that node is the end, retrace the path
            if (currentNode == endNode)
            {
                return CalculateWaypointPath(endNode);
            }

            // remove it from the seen nodes and put it on the finished nodes
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            // for each tile in cardnal directions from this one
            foreach (TilePathNode neighborNode in GetNeighborList(currentNode, grid))
            {
                if (closedList.Contains(neighborNode))
                {
                    continue;
                }

                if (neighborNode.isWalkable == false)
                {
                    closedList.Add(neighborNode);
                    continue;
                }

                // calculate the cost it would take to get to that neighbor node using this tile
                // If its cheaper than its already existing cost, update it
                int tempGCost = currentNode.gCost + CalculateDistance(currentNode, neighborNode);
                if (tempGCost < neighborNode.gCost)
                {
                    neighborNode.previousNode = currentNode;
                    neighborNode.gCost = tempGCost;
                    neighborNode.hCost = CalculateDistance(neighborNode, endNode);
                    neighborNode.CalculateFCost();

                    // if we havent seen this node yet, add it to the list
                    if (!openList.Contains(neighborNode))
                    {
                        openList.Add(neighborNode);
                    }
                }
            }
        }
        // No Path Found
        Debug.Log("No path");
        return null;
    }


    // Finds 4 neighbors
    private List<TilePathNode> GetNeighborList(TilePathNode currentNode, Tile[,] grid)
    {
        List<TilePathNode> neighbors = new List<TilePathNode>();
        if (currentNode.GetX() > 0) 
        {
            neighbors.Add(currRoom.getTile(currentNode.GetX() - 1, currentNode.GetY()).gameObject.GetComponent<TilePathNode>());
        }

        if (currentNode.GetX() < width - 1)
        {
            neighbors.Add(currRoom.getTile(currentNode.GetX() + 1, currentNode.GetY()).gameObject.GetComponent<TilePathNode>());
        }

        if (currentNode.GetY() > 0)
        {
            neighbors.Add(currRoom.getTile(currentNode.GetX(), currentNode.GetY() - 1).gameObject.GetComponent<TilePathNode>());
        }

        if (currentNode.GetY() < height - 1)
        {
            neighbors.Add(currRoom.getTile(currentNode.GetX(), currentNode.GetY() + 1).gameObject.GetComponent<TilePathNode>());
        }

        return neighbors;
    }

    // Retrace linked list and makes it a regular list
    private List<TilePathNode> CalculatePath(TilePathNode endNode) 
    {
        List<TilePathNode> path = new List<TilePathNode>();
        path.Add(endNode);
        TilePathNode currentNode = endNode;
        while (currentNode.previousNode != null) 
        {
            path.Add(currentNode.previousNode);
            currentNode = currentNode.previousNode;
        }
        return path;
    }


    // Retrace linked list and makes it a regular list
    private List<Transform> CalculateWaypointPath(TilePathNode endNode)
    {
        List<Transform> path = new List<Transform>();
        path.Add(endNode.transform);
        TilePathNode currentNode = endNode;
        while (currentNode.previousNode != null)
        {
            path.Add(currentNode.transform);
            currentNode = currentNode.previousNode;
        }
        //Debug.Log(path.Count);
        return path;
    }
    // Calcualtes distance from tile a to tile b
    private int CalculateDistance(TilePathNode a, TilePathNode b) 
    {
        return MOVE_COST * (Mathf.Abs(a.GetX() - b.GetX()) + Mathf.Abs(a.GetY() - b.GetY()));
    }

    // Iterates through list and finds lowest fcost tile
    private TilePathNode GetLowestFCost(List<TilePathNode> tilePathNodeList) 
    {
        TilePathNode lowest = tilePathNodeList[0];
        for (int i = 1; i < tilePathNodeList.Count; i++) 
        {
            if (tilePathNodeList[i].fCost < lowest.fCost) 
            {
                lowest = tilePathNodeList[i];
            }
        }

        return lowest;
    }
}
