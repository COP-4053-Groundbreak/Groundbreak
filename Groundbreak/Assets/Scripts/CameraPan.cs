using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour
{
    [SerializeField] float panSpeed = 1f;
    [SerializeField] float zoomSpeed = 0.5f;
    [SerializeField] float maxZoom = 2f;
    [SerializeField] float minZoom = 10f;

    GameObject player;
    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>().gameObject;

    }
    // Update is called once per frame
    void Update()
    {
        Camera camera = GetComponent<Camera>();
        MoveCamera();

        CameraZoom(camera);

        // Center Camera on player
        if (Input.GetKey(KeyCode.Tab))
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        }
    }

    private void MoveCamera()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0, panSpeed) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(0, -panSpeed) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(panSpeed, 0) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-panSpeed, 0) * Time.deltaTime;
        }
    }

    private void CameraZoom(Camera camera)
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            camera.orthographicSize += zoomSpeed;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            camera.orthographicSize -= zoomSpeed;
        }

        if (GetComponent<Camera>().orthographicSize > 10)
        {
            camera.orthographicSize = 10;
        }

        if (GetComponent<Camera>().orthographicSize < 2)
        {
            camera.orthographicSize = 2;
        }
    }
}
