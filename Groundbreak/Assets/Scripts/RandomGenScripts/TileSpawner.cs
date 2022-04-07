using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    private int rand;
    private TileTemplates templates;
    private TileTemplates templates2;

    private void Start()
    {
        Debug.Log("WHERE IS THIS " + gameObject);
        templates = GameObject.FindGameObjectWithTag("TileSpawner").GetComponent<TileTemplates>();
        templates2 = GameObject.FindGameObjectWithTag("TileSpawner2").GetComponent<TileTemplates>();

        rand = Random.Range(0, templates.normalTileSet.Length);
    }

    void Update()
    {
        Instantiate(templates2.threeByThreeTileSet[rand], transform.position, templates2.threeByThreeTileSet[rand].transform.rotation);
        Instantiate(templates.normalTileSet[rand], transform.position, templates.normalTileSet[rand].transform.rotation);
        Destroy(gameObject);
    }
}
