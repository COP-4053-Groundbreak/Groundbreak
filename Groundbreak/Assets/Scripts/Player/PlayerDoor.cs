using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDoor : MonoBehaviour
{
    private GameObject currentTeleporter;

    void Update()
    {
        if(currentTeleporter != null)
        {
            transform.position = currentTeleporter.GetComponent<Door>().GetDestination().position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Door"))
        {
            Debug.Log("Collision with door! This happened");
            currentTeleporter = collision.gameObject;
        }

        //Make it deactive current room
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

        //Make it active new room?
    }
}
