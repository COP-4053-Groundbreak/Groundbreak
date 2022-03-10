using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;
    public GameObject currentDoor;
    //1 -->need bottom door
    //2 -->need top door
    //3 -->need left door
    //4 -->need right door
 
    private RoomTemplates templates;
    private GameObject newRoom;
    //private GameObject newDoor;
    private Door newDoor;
    private Transform destination;

    private int rand;
    private bool spawned = false;

    public float waitTime = 4f;

    private void Start()
    {
        //Destroy(gameObject, waitTime);
        templates = FindObjectOfType<RoomTemplates>();
        Invoke("Spawn", 0.2f);
    }


    private void Spawn()
    {
        if (spawned == false)
        {
            if (openingDirection == 1)
            {
                //Need to spawn a room with a BOTTOM door
                rand = Random.Range(0, templates.bottomRooms.Length);
                Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);
                //newRoom = (GameObject)GameObject.Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);
                //newDoor = newRoom.transform.Find("Door1").GetComponent<Door>();
                //newDoor.destination = newDoor.transform;
                //currentDoor.GetComponent<Door>().destination = newDoor.transform;
                //Destroy(gameObject);

            }
            else if (openingDirection == 2)
            {
                //Need to spawn a room with a TOP door
                rand = Random.Range(0, templates.topRooms.Length);
                Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation);
                //newRoom = (GameObject)GameObject.Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation);
                //newDoor = newRoom.transform.Find("Door2").GetComponent<Door>();
                //newDoor.destination = newDoor.transform;
                //currentDoor.GetComponent<Door>().destination = newDoor.transform;
                //Destroy(gameObject);
            }
            else if (openingDirection == 3)
            {
                //Need to spawn a room with a LEFT door
                rand = Random.Range(0, templates.leftRooms.Length);
                Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);
                //newRoom = (GameObject)GameObject.Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);
                //newDoor = newRoom.transform.Find("Door3").GetComponent<Door>();
                //newDoor.destination = newDoor.transform;
                //currentDoor.GetComponent<Door>().destination = newDoor.transform;
                //Destroy(gameObject);
            }
            else if (openingDirection == 4)
            {
                //Need to spawn a room with a RIGHT door
                rand = Random.Range(0, templates.rightRooms.Length);
                Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);
                //newRoom = (GameObject)GameObject.Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);
                //newDoor = newRoom.transform.Find("Door4").GetComponent<Door>();
                //newDoor.destination = newDoor.transform;
                //currentDoor.GetComponent<Door>().destination = newDoor.transform;
                //Destroy(gameObject);
            }
            spawned = true;
        }
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false)
            {
                templates = FindObjectOfType<RoomTemplates>();
                if ((other.GetComponent<RoomSpawner>().openingDirection == 1 || other.GetComponent<RoomSpawner>().openingDirection == 2)
                        && (openingDirection == 1 || openingDirection == 2))
                {
                    //Need a room with bottom and top door
                    Instantiate(templates.TB, transform.position, templates.TB.transform.rotation);
                }
                else if ((other.GetComponent<RoomSpawner>().openingDirection == 1 || other.GetComponent<RoomSpawner>().openingDirection == 3)
                        && (openingDirection == 1 || openingDirection == 3))
                {
                    //Need a room with a bottom and left door
                    Instantiate(templates.BL, transform.position, templates.BL.transform.rotation);
                }
                else if ((other.GetComponent<RoomSpawner>().openingDirection == 1 || other.GetComponent<RoomSpawner>().openingDirection == 4)
                        && (openingDirection == 1 || openingDirection == 4))
                {
                    //Need a room with a bottom and right door
                    Instantiate(templates.RB, transform.position, templates.RB.transform.rotation);
                }
                else if ((other.GetComponent<RoomSpawner>().openingDirection == 2 || other.GetComponent<RoomSpawner>().openingDirection == 3)
                        && (openingDirection == 2 || openingDirection == 3))
                {
                    //Need a room with a top and left door
                    Instantiate(templates.TL, transform.position, templates.TL.transform.rotation);
                }
                else if ((other.GetComponent<RoomSpawner>().openingDirection == 2 || other.GetComponent<RoomSpawner>().openingDirection == 4)
                        && (openingDirection == 2 || openingDirection == 4))
                {
                    //Need a room with a top and right door
                    Instantiate(templates.TR, transform.position, templates.TR.transform.rotation);
                }
                else if ((other.GetComponent<RoomSpawner>().openingDirection == 3 || other.GetComponent<RoomSpawner>().openingDirection == 4)
                        && (openingDirection == 3 || openingDirection == 4))
                {
                    //Need a room with a left and right door
                    Instantiate(templates.RL, transform.position, templates.RL.transform.rotation);
                }

                Destroy(gameObject);
            }
            spawned = true;
        }
    }
}
