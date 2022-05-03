using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawner : MonoBehaviour
{
    [SerializeField] bool Spawned = false;
    [SerializeField] int RoomWidth;
    [SerializeField] int RoomHeight;
    int ChestSpawn = 10;
    [SerializeField] GameObject Chest;
    private Transform posChest;
    private GameObject currentChest;
    GridManager gridManager;

    private int randX;
    private int randY;
    private int luck;

    void Start()
    {
        luck = Random.Range(0, ChestSpawn);
        if(luck >= 1)
        {
            Debug.Log("A chest should spawn in " + gameObject);
            if (Spawned == false)
            {
                gridManager = gameObject.transform.Find("ThisRoomGridManager").GetComponent<GridManager>();
                Vector3 localPos;   
                do
                {
                    randX = Random.Range(0, RoomWidth);
                    randY = Random.Range(0, RoomHeight);
                    localPos = transform.position + new Vector3(randX, randY);
                    //Debug.Log("While Loop Test");
                } while ((gridManager.grid[randX, randY].gameObjectAbove &&
                                              (gridManager.grid[randX, randY].gameObjectAbove.CompareTag("Barrel") || gridManager.grid[randX, randY].gameObjectAbove.CompareTag("Enemy"))));


                currentChest = Instantiate(Chest, gridManager.grid[randX, randY].transform.position, transform.rotation, gameObject.transform);
       
                Spawned = true;
                currentChest.SetActive(false);
            }
        }
    }
}
