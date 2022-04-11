using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int width, height;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private Transform cam;
    [SerializeField] GameObject tileHolder;
    [SerializeField] public Vector2 bottomLeftCorner;
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
       // rand = Random.Range(0, templates.normalTileSet.Length);
        

        bottomLeftCorner = new Vector2((int)(this.room.transform.position.x - 5.0f), (int)(this.room.transform.position.y - 5.0f));
        room = this.gameObject;
        SpawnTilesInRoom();

        //generateGrid();

        //Debug.Log(grid[0, 0].getElement());

    }

    void Start(){
        foreach (Tile t in grid){
            t.findNeighbors(this);
        }
    }
    
    /*private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha6)){
            recolorAll();
        }    
    }*/

    public void SpawnTilesInRoom()
    {

        foreach (GameObject spawn in spawnPoints)
        {
            rand = Random.Range(0, templates.normalTileSet.Length);
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
        bool cond1 = x >= 0;
        bool cond2 = x < width;
        bool cond3 = y >= 0;
        bool cond4 = y < height;
        //Debug.Log($"Checking if in bounds: {0} <= {x} < {width}");
        //Debug.Log($"Checking if in bounds: {0} <= {y} < {height}");
        return (cond1 && cond2 && cond3 && cond4);
    }

    
    public Tile getTile(float x, float y){
        //Debug.Log($"Getting tile at {x},{y}");
        return grid[(int)x, (int)y];
    }
    
    public Tile[,] getGrid() {
        return grid;
    }

    // For debugging only
    public void recolorAll(){
        foreach(Tile tile in grid){
            tile.setElement(tile.myElement);
        }
    }

}
