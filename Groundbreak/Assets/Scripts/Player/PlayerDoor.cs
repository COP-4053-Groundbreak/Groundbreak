using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDoor : MonoBehaviour
{
    private GameObject currentTeleporter;

    void Update()
    {
        //Figure out how to do on contact teleportation
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Door"))
        {
            currentTeleporter = collision.gameObject;
        }

        //Make it deactive current room
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Door"))
        {
            if(collision.gameObject == currentTeleporter)
            if(collision.gameObject == currentTeleporter)
            {
                currentTeleporter = null;
            }
        }

        //Make it active new room?
    }
}
