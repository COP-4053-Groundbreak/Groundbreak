using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int width, height;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private Transform cam;
    [SerializeField] GameObject tileHolder;
    [SerializeField] Vector2 bottomLeftCorner;
    [SerializeField] GameObject room;


    public static Tile[,] grid;

    GameObject[] spawnPoints;
    private int rand;
    private TileTemplates templates;
    [SerializeField] GameObject holder;

    // Start is called before the first frame update
    void Awake(){
        grid = new Tile[10, 10];
        spawnPoints = GameObject.FindGameObjectsWithTag("TileSpawnPoint");
        Debug.Log(spawnPoints.Length);
        templates = GameObject.FindGameObjectWithTag("TileSpawner").GetComponent<TileTemplates>();
        rand = Random.Range(0, templates.normalTileSet.Length);
        SpawnTilesInRoom();

        bottomLeftCorner = new Vector2((int)(this.room.transform.position.x - 5.0f), (int)(this.room.transform.position.y - 5.0f));
        room = this.gameObject;
        //generateGrid();
        setCameraPos();

        //Debug.Log(grid[0, 0].getElement());

    }

    public void SpawnTilesInRoom()
    {

        foreach (GameObject spawn in spawnPoints)
        {
            GameObject tilePrefab = Instantiate(templates.normalTileSet[rand], spawn.transform.position, templates.normalTileSet[rand].transform.rotation, holder.transform);
            //Debug.Log(spawn.transform.position + new Vector3(4.5f, 4.5f, 0));
            foreach (Transform child in tilePrefab.transform)
            {
                grid[(int)(child.position.x + 4.5f), (int)(child.position.y + 4.5f)] = child.GetComponent<Tile>();
                //Debug.Log((int)(child.position.x + 4.5f) + " , " + (int)(child.position.y + 4.5f));
            }
            Destroy(spawn);
        }
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
        return (x >= bottomLeftCorner.x && x < bottomLeftCorner.x + width && y >= bottomLeftCorner.y && y < bottomLeftCorner.y + height);
    }

    // Assuming we have the bottom right corner of a room with spawned tiles, this should get the
    // relative position of any gameobject
    public  Vector2 getRelativePos(GameObject obj){
        int x = (int) obj.transform.position.x;
        int y = (int) obj.transform.position.y;

        return new Vector2(x - bottomLeftCorner.x, y - bottomLeftCorner.y);
    }
    public Tile[,] getGrid() 
    {
        return grid;
    }

}
