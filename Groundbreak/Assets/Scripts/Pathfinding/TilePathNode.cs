using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePathNode: MonoBehaviour
{
    // Class attatched to tiles, contains their positions, and costs for pathfinding
    // gcost is the cost from the start node to here
    // hcost estimates the cheapest cost from here to the end
    // fcost is the final cost
    private int x;
    private int y;
    public bool isWalkable;

    public int gCost;
    public int hCost;
    public int fCost;

    public TilePathNode previousNode;

    private void Start()
    {
        x = (int)transform.position.x + 5;
        y = (int)transform.position.y + 5;
        
        if (gameObject.GetComponent<Tile>().getElement() == Element.Void)
        {
            isWalkable = false;
        }
        else 
        {
            isWalkable = true;
        }
    }


    public int GetX() 
    {
        return this.x;
    }

    public int GetY()
    {
        return this.y;
    }
    public void CalculateFCost() 
    {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        return x + " ," + y;
    }
}
