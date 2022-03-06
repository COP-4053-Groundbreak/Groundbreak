using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSpawner : MonoBehaviour
{
    private int rand;
    private TileTemplates templates;

    private void Start()
    {
        templates = GameObject.FindGameObjectWithTag("TileSpawner").GetComponent<TileTemplates>();
        rand = Random.Range(0, templates.normalTileSet.Length);
    }

    void Update()
    {
        Instantiate(templates.normalTileSet[rand], transform.position, templates.normalTileSet[rand].transform.rotation);
        Destroy(gameObject);
    }
}
