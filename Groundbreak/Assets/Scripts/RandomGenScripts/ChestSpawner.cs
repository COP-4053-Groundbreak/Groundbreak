using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawner : MonoBehaviour
{
    [SerializeField] bool Spawned = false;
    [SerializeField] int RoomWidth;
    [SerializeField] int RoomHeight;

    private int randX;
    private int randY;

    // Start is called before the first frame update
    void Start()
    {
        randX = Random.Range(0, RoomWidth);
        randY = Random.Range(0, RoomHeight);

        //Instantiate a chest
        //Instantiate
    }

    
}
