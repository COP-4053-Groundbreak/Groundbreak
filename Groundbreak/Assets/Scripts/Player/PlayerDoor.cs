using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDoor : MonoBehaviour
{
    private GameObject currentTeleporter;
    public GameObject currentRoom;

    void Update()
    {
        if(currentTeleporter != null)
        {
            transform.position = currentTeleporter.GetComponent<Door>().GetDestination().position;
            //Debug.LogError("Object " + currentRoom.name);
            foreach (Transform child in currentRoom.transform)
            {
                //Debug.Log("Inside foreach");
                if ((child.name == "PlayerDetector1" || child.name == "PlayerDetector2" || child.name == "PlayerDetector3" || child.name == "PlayerDetector4"))
                {
                    //Debug.Log("set a detector as true");
                    child.gameObject.SetActive(true);
                }
                else
                {
                    //Debug.LogError("Set " + child.name);
                    //Debug.Log("set an object as false");
                    child.gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Collision with door! This happened");

        if (collision.CompareTag("Door"))
        {
            currentTeleporter = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Door"))
        {
            if(collision.gameObject == currentTeleporter)
            {
                currentTeleporter = null;
            }
        }
    }
}
