using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public void ToggleRoom(bool whatToSet) 
    {
        foreach (Transform child in transform) 
        {
            if (!child.CompareTag("SpawnPoint")) 
            {
                child.gameObject.SetActive(whatToSet);
            }
        }
    }
}
