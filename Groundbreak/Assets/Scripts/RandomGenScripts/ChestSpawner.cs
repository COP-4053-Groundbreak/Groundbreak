using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawner : MonoBehaviour
{
    [SerializeField] bool Spawned = false;
    [SerializeField] int RoomWidth;
    [SerializeField] int RoomHeight;
    [SerializeField] int ChestSpawn;
    [SerializeField] GameObject Chest;
    private Transform posChest;
    private GameObject currentChest;

    private int randX;
    private int randY;
    private int luck;

    void Start()
    {
        luck = Random.Range(0, ChestSpawn);
        if(luck == 1)
        {
            if (Spawned == false)
            {
                randX = Random.Range(0, RoomWidth) - 5;
                randY = Random.Range(0, RoomHeight) - 5;

                currentChest = Instantiate(Chest, transform.position + new Vector3(randX, randY), transform.rotation, gameObject.transform);
       
                Spawned = true;
                currentChest.SetActive(false);
            }
        }
    }
}
