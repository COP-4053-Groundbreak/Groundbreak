using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour
{
    [SerializeField] float panSpeed = 0.05f;
    GameObject player;
    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>().gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W)) 
        {
            transform.position += new Vector3(0, panSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(0, -panSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(panSpeed, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-panSpeed, 0);
        }

        // Center Camera on player
        if (Input.GetKey(KeyCode.Tab))
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        }
    }
}
