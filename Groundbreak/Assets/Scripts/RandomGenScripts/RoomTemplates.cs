using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;

    //Rooms for Corner Gltich, R = Right, L = Left, T = Top, B = Bottom
    public GameObject TB;
    public GameObject BL;
    public GameObject RB;
    public GameObject TL;
    public GameObject TR;
    public GameObject RL;

    //Rooms to close off when we hit the max
    public GameObject R;
    public GameObject L;
    public GameObject T;
    public GameObject B;

    public List<GameObject> rooms;

    public int roomLimit;
    public float waitTime;
    private bool spawnedBoss;
    [SerializeField] GameObject boss;
    private GameObject bossPos;

    private GameObject spawnPointHolder;
    private int posRand;
    GridManager gridManager;


    private void Update()
    {
        if(waitTime <= 0 && spawnedBoss == false)
        {
            foreach (Transform child in rooms[rooms.Count - 1].transform)
            {
                if (child.CompareTag("Enemy"))
                {
                    Destroy(child.gameObject);
                }
            }

            GameObject currentRoom = rooms[rooms.Count-1];
            gridManager = currentRoom.transform.Find("ThisRoomGridManager").GetComponent<GridManager>();
            spawnPointHolder = currentRoom.transform.Find("EnemySpawnsHolder").gameObject;
            Debug.Log(spawnPointHolder);

            Vector3 localPos;
            int pointX;
            int pointY;

            //Getting random numbers
            GameObject currentPoint;
            EnemySpawner spawnPont;

            do
            {
                posRand = Random.Range(0, spawnPointHolder.transform.childCount - 1);
                Debug.Log(posRand);
                currentPoint = spawnPointHolder.transform.GetChild(posRand).gameObject;
                Debug.Log(currentPoint);
                spawnPont = currentPoint.GetComponent<EnemySpawner>();

                if (currentPoint == null)
                    Debug.LogWarning("Fuck");

                localPos = currentRoom.transform.InverseTransformPoint(currentPoint.transform.position);
                Debug.Log(localPos);
                pointY = (int)(localPos.y + 5.5f);
                pointX = (int)(localPos.x + 5.5f);

                Debug.Log("point x: " + pointX + "point y: " + pointY);

            } while ((gridManager.grid[pointX, pointY].gameObjectAbove &&
                                               (gridManager.grid[pointX, pointY].gameObjectAbove.CompareTag("Barrel") || gridManager.grid[pointX, pointY].gameObjectAbove.CompareTag("Enemy"))) ||
                                               gridManager.grid[pointX, pointY].myElement == Element.Void);

            localPos.x = pointX;
            localPos.y = pointY;

            //Make the enemy
            //Debug.LogWarning(gridManager.grid + " " + pointX + " " + pointY);
            bossPos = Instantiate(boss, gridManager.grid[pointX, pointY].transform.position, Quaternion.identity, rooms[rooms.Count - 1].transform);
            bossPos.SetActive(false);
            spawnedBoss = true;

            //bossPos = Instantiate(boss, rooms[rooms.Count - 1].transform.position, Quaternion.identity, rooms[rooms.Count - 1].transform);
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }
}
