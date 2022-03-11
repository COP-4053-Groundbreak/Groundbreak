using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawner : MonoBehaviour
{
    [SerializeField] bool Spawned = false;
    [SerializeField] int RoomWidth;
    [SerializeField] int RoomHeight;
    [SerializeField] GameObject Chest;

    private int randX;
    private int randY;

    // Start is called before the first frame update
    void Start()
    {
        if (Spawned == false)
        {
            randX = Random.Range(0, RoomWidth);
            randY = Random.Range(0, RoomHeight);
            Vector3 posChest = new Vector3(randX, randY, 0);

            //Instantiate a chest
            Instantiate(Chest, transform.position, transform.rotation, gameObject.transform);
            Chest.transform.localPosition = posChest;
            Spawned = true;
        }
    }

    
}
