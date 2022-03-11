using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawner : MonoBehaviour
{
    [SerializeField] bool Spawned = false;
    [SerializeField] int RoomWidth;
    [SerializeField] int RoomHeight;
    [SerializeField] GameObject Chest;
    private Transform posChest;

    int w = 10;
    int h = 10;

    private int randX;
    private int randY;

    // Start is called before the first frame update
    void Start()
    {
        if (Spawned == false)
        {
            randX = Random.Range(0, RoomWidth) - 5;
            randY = Random.Range(0, RoomHeight) - 5;
            //posChest = new Vector3(randX, randY, 0);

            //Instantiate a chest
            Instantiate(Chest, transform.position + new Vector3(randX, randY), transform.rotation, gameObject.transform);
            posChest = Chest.transform;

            //Debug.Log(Chest.transform.position);
            //Chest.transform.position += new Vector3(w, h, 0);
            //Debug.Log(Chest.transform.position);

            Spawned = true;
        }
    }
}
