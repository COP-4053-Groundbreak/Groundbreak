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

    public List<GameObject> rooms;

    public float waitTime;
    private bool spawnedBoss;
    public GameObject boss;

    private void Update()
    {
        if(waitTime <= 0 && spawnedBoss == false)
        {
            Instantiate(boss, rooms[rooms.Count - 1].transform.position, Quaternion.identity);
            spawnedBoss = true;
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }
}
