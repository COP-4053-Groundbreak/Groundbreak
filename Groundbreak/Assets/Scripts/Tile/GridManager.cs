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


    public Tile[,] grid;

    GameObject[] spawnPoints;
    private int rand;
    private TileTemplates templates;
    [SerializeField] GameObject holder;

    // Start is called before the first frame update
    void Awake(){
        grid = new Tile[10, 10];
        spawnPoints = GameObject.FindGameObjectsWithTag("TileSpawnPoint");
        //Debug.Log(spawnPoints.Length);
        templates = GameObject.FindGameObjectWithTag("TileSpawner").GetComponent<TileTemplates>();
        rand = Random.Range(0, templates.normalTileSet.Length);
        

        bottomLeftCorner = new Vector2((int)(this.room.transform.position.x - 5.0f), (int)(this.room.transform.position.y - 5.0f));
        room = this.gameObject;
        SpawnTilesInRoom();
        //generateGrid();

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
                //child.SetParent(tileHolder.transform);
                //Debug.Log(child.name + " " + (int)(child.localPosition.x + child.parent.localPosition.x + child.parent.parent.localPosition.x + 5.5f - room.transform.InverseTransformPoint(child.transform.position).x) + " , " + (int)(child.localPosition.y + child.parent.localPosition.y + child.parent.parent.localPosition.y + 5.5f - room.transform.InverseTransformPoint(child.transform.position).y));
                grid[(int)(room.transform.InverseTransformPoint(child.transform.position).x + 5.5f), (int)(room.transform.InverseTransformPoint(child.transform.position).y + 5.5f)] = child.GetComponent<Tile>();
                
            }
            Destroy(spawn);
        }
        GetComponentInParent<RoomManager>().ToggleRoom(false);
    }

    /*
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
    }*/


    public int getHeight(){
        return height;
    }
    public int getWidth(){
        return width;
    }
    public void setWidth(int width){
        this.width = width;
    }
    public void setHeight(int height){
        this.height = height;
    }

    public bool inBounds(int x, int y){
        return (x >= bottomLeftCorner.x && x < bottomLeftCorner.x + width && y >= bottomLeftCorner.y && y < bottomLeftCorner.y + height);
    }

    // Assuming we have the bottom right corner of a room with spawned tiles, this should get the
    // relative position of any gameobject
    public  Vector2 getRelativePos(float newX, float newY){
        int x = (int)newX;
        int y = (int)newY;

        return new Vector2(newX - bottomLeftCorner.x, newY - bottomLeftCorner.y);
    }
    public Tile getTile(float x, float y){
        return grid[(int)x, (int)y];
    }
    
    public Tile[,] getGrid() {
        return grid;
    }

}
