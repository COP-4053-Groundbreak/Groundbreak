using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    public void ToggleRoom(bool whatToSet) 
    {
        // THIS IS A VERY BAD WAY TO DO THIS
        // THIS BREAKS IF THE START ROOM IS RENAMED
        // DO THIS ANOTHER WAY WHEN I CAN SEE PAST NULL REFS
        // -N

        // Had to comment this out of toggle " || gameObject.name != "StartRoomLevel2""
        Debug.LogError(SceneManager.GetActiveScene().name + "," + gameObject.name);
        if (gameObject.name != "StartRoomV2")
        {
            foreach (Transform child in transform)
            {
                if (!(child.CompareTag("SpawnPoint") || child.CompareTag("PlayerDetector")))
                {
                    child.gameObject.SetActive(whatToSet);
                }
            }
        }
        if(SceneManager.GetActiveScene().name == "Tutorial" && gameObject.name == "R")
        {
            Debug.LogError("kill me");
            foreach (Transform child in transform)
            {
                if (!(child.CompareTag("SpawnPoint") || child.CompareTag("PlayerDetector")))
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
    }

    private void Start()
    {
        StartCoroutine(DelayAndShow());
    }

    IEnumerator DelayAndShow() 
    {
        yield return new WaitForSeconds(5f);
        ToggleRoom(true);
    }
}
