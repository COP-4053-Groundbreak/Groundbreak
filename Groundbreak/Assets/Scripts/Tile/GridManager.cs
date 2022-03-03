using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int width, height;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private Transform cam;
    [SerializeField] GameObject tileHolder;

    public static Tile[,] grid;

    // Start is called before the first frame update
    void Awake(){
        generateGrid();
        setCameraPos();
    }

    void generateGrid(){
        grid = new Tile[width, height];
        
        Element[] arr = {Element.Air, Element.Earth, Element.Fire, Element.Water, Element.Base, Element.Void};
        int arr_len = arr.Length;
        // Place down tiles and set their elements
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                grid[x,y] = Instantiate(tilePrefab, new Vector3(x,y), Quaternion.identity, tileHolder.transform);
                grid[x,y].name = $"Tile {x} {y}";
                grid[x,y].setElement(arr[Random.Range(0, arr_len)]);
            }
        }
        // Necessary to find neighbors after procedural generation
        foreach (Tile a in grid){
            a.findNeighbors();
            a.setLF((LandFeature)Random.Range(0,6));
        }

        grid[0,0].setElement(Element.Base);
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

    public bool inBounds(int x, int y){
        return (x >= 0 && x < width && y >= 0 && y < height);
    }

    public Tile[,] getGrid() 
    {
        return grid;
    }

}
