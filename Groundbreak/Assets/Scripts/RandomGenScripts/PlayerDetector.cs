using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] GameObject currentRoom;
    [SerializeField] public PlayerDoor PlayerRoom;

    // Update is called once per frame
    private void Start()
    {
        PlayerRoom = FindObjectOfType<PlayerDoor>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision with player has happened");

        //PlayerRoom.currentRoom = currentRoom;

        foreach (Transform child in currentRoom.transform)
        {
            Debug.Log("Inside foreach");
            if ( (child.name == "PlayerDetector1" || child.name == "PlayerDetector2" || child.name == "PlayerDetector3" || child.name == "PlayerDetector4"))
            {
                Debug.Log("set a detector as false");
                child.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("set an object as true");
                child.gameObject.SetActive(true);
            }
        }
        FindObjectOfType<FindNewGridManager>().ChangedRoom();
        PlayerRoom.currentRoom = currentRoom;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Player has left the box thing");
        /*foreach (Transform child in currentRoom.transform)
        {
            Debug.Log("Inside foreach");
            if ((child.name == "PlayerDetector1" || child.name == "PlayerDetector2" || child.name == "PlayerDetector3" || child.name == "PlayerDetector4"))
            {
                Debug.Log("set a detector as false");
                child.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("set an object as true");
                child.gameObject.SetActive(true);
            }
        }*/
    }
}
