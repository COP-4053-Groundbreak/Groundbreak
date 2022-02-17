using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int width, height;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private Transform cam;

    private Tile[,] grid;

    // Start is called before the first frame update
    void Awake(){
        generateGrid();
        setCameraPos();
    }
    void generateGrid(){
        grid = new Tile[width, height];
        
        // Place down tiles and set their elements
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                grid[x,y] = Instantiate(tilePrefab, new Vector3(x,y), Quaternion.identity);
                grid[x,y].name = $"Tile {x} {y}";

                // This part places down elements specifically for 3x3, mainly for testing
                if (x == 0 && y == 0 || x == 2 && y == 0) grid[x,y].setElement(Element.Air);
                else if (x == 0 && y == 1 || x == 2 && y == 1) grid[x,y].setElement(Element.Water);
                else if (x == 0 && y == 2 || x == 2 && y == 2) grid[x,y].setElement(Element.Fire);
                else {
                    if (!(x == 1 && y == 1)) grid[x,y].setElement(Element.Earth);
                    else grid[x,y].setElement(Element.Void);
                }
            }
        }
        // Necessary to find neighbors after procedural generation
        foreach (Tile a in grid){
            a.findNeighbors();
            a.setLF((LandFeature)Random.Range(0,6));
        }
    }
    void setCameraPos(){
        cam.transform.position = new Vector3((float)width/2 - 0.5f, (float)height/2 - 0.5f, -10);
    }
    public int getHeight(){
        return height;
    }
    public int getWidth(){
        return width;
    }

    public Tile[,] getGrid() 
    {
        return grid;
    }

}
