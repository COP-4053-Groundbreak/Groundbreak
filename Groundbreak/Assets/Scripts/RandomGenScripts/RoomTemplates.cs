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
    [SerializeField] GameObject boss;
    private GameObject bossPos;



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
            bossPos = Instantiate(boss, rooms[rooms.Count - 1].transform.position, Quaternion.identity, rooms[rooms.Count - 1].transform);
            bossPos.SetActive(false);
            spawnedBoss = true;
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }
}
