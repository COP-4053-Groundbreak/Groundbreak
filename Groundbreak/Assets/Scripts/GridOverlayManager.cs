using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridOverlayManager : MonoBehaviour
{
    [SerializeField] GameObject gridHolder;
    [SerializeField] GameObject boxOverlay;
    // Start is called before the first frame update
    void Start()
    {
        // Finds every tile, creates grid overlay under grid holder
        Tile[] tileList = FindObjectsOfType<Tile>();
        foreach (Tile tile in tileList) 
        {
            Instantiate(boxOverlay, tile.gameObject.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)), gridHolder.transform);
        }
    }

    public void DisableGrid() 
    {
        gridHolder.SetActive(false);
    }

}
