using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

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
    GameObject[] test1;
    GameObject[] test2;
    private int rand;
    private TileTemplates templates;
    [SerializeField] GameObject holder;

    // Start is called before the first frame update
    void Awake(){
        grid = new Tile[10, 10];

        test1 = GameObject.FindGameObjectsWithTag("TileSpawnPoint");
        test2 = GameObject.FindGameObjectsWithTag("TileSpawnPoint2");
        spawnPoints = test1.Concat(test2).ToArray();
        //Debug.Log(spawnPoints.Length);

        templates = GameObject.FindGameObjectWithTag("TileSpawner").GetComponent<TileTemplates>();
        

        bottomLeftCorner = new Vector2((int)(this.room.transform.position.x - 5.0f), (int)(this.room.transform.position.y - 5.0f));
        room = this.gameObject;

        if (SceneManager.GetActiveScene().name != "Tutorial")
        {
            SpawnTilesInRoom();
        }
        else 
        {
            AddTilesToGridTutorial();
        }

        //generateGrid();

        //Debug.Log(grid[0, 0].getElement());

    }

    private void AddTilesToGridTutorial()
    {
        Debug.Log("hewwo");
        foreach (Transform tilePrefab in transform.parent.Find("TileHolder")) 
        {
            foreach (Transform child in tilePrefab.transform)
            {
                //child.SetParent(tileHolder.transform);
                //Debug.Log(child.name + " " + (int)(child.localPosition.x + child.parent.localPosition.x + child.parent.parent.localPosition.x + 5.5f - room.transform.InverseTransformPoint(child.transform.position).x) + " , " + (int)(child.localPosition.y + child.parent.localPosition.y + child.parent.parent.localPosition.y + 5.5f - room.transform.InverseTransformPoint(child.transform.position).y));
                grid[(int)(room.transform.InverseTransformPoint(child.transform.position).x + 5.5f), (int)(room.transform.InverseTransformPoint(child.transform.position).y + 5.5f)] = child.GetComponent<Tile>();

            }
        }
        GetComponentInParent<RoomManager>().ToggleRoom(false);
    }

    void Start(){
        foreach (Tile t in grid){
            t.findNeighbors(this);
        }
    }

    public void SpawnTilesInRoom()
    {

        foreach (GameObject spawn in spawnPoints)
        {
            GameObject tilePrefab;

            if (spawn.CompareTag("TileSpawnPoint"))
            {
                rand = Random.Range(0, templates.normalTileSet.Length);
                tilePrefab = Instantiate(templates.normalTileSet[rand], spawn.transform.position, templates.normalTileSet[rand].transform.rotation, holder.transform);
                foreach (Transform child in tilePrefab.transform)
                {
                    //child.SetParent(tileHolder.transform);
                    //Debug.Log(child.name + " " + (int)(child.localPosition.x + child.parent.localPosition.x + child.parent.parent.localPosition.x + 5.5f - room.transform.InverseTransformPoint(child.transform.position).x) + " , " + (int)(child.localPosition.y + child.parent.localPosition.y + child.parent.parent.localPosition.y + 5.5f - room.transform.InverseTransformPoint(child.transform.position).y));
                    grid[(int)(room.transform.InverseTransformPoint(child.transform.position).x + 5.5f), (int)(room.transform.InverseTransformPoint(child.transform.position).y + 5.5f)] = child.GetComponent<Tile>();

                }
            }
            else if(spawn.CompareTag("TileSpawnPoint2"))
            {
                rand = Random.Range(0, templates.threeByThreeTileSet.Length);
                tilePrefab = Instantiate(templates.threeByThreeTileSet[rand], spawn.transform.position, templates.threeByThreeTileSet[rand].transform.rotation, holder.transform);
                foreach (Transform child in tilePrefab.transform)
                {
                    //child.SetParent(tileHolder.transform);
                    //Debug.Log(child.name + " " + (int)(child.localPosition.x + child.parent.localPosition.x + child.parent.parent.localPosition.x + 5.5f - room.transform.InverseTransformPoint(child.transform.position).x) + " , " + (int)(child.localPosition.y + child.parent.localPosition.y + child.parent.parent.localPosition.y + 5.5f - room.transform.InverseTransformPoint(child.transform.position).y));
                    grid[(int)(room.transform.InverseTransformPoint(child.transform.position).x + 5.5f), (int)(room.transform.InverseTransformPoint(child.transform.position).y + 5.5f)] = child.GetComponent<Tile>();

                }
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

    // Assuming we have the bottom right corner of a room with spawned tiles, this should get the
    // relative position of any gameobject
    public  Vector2 getRelativePos(float newX, float newY){
        int x = (int)newX;
        int y = (int)newY;

        return new Vector2(newX - bottomLeftCorner.x, newY - bottomLeftCorner.y);
    }
    public Tile getTile(float x, float y){
        //Debug.Log($"Getting tile at {x},{y}");
        return grid[(int)x, (int)y];
    }
    
    public Tile[,] getGrid() {
        return grid;
    }

}
