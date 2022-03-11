using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneration : MonoBehaviour
{
    private EnemyTemplates templates;

    [SerializeField] bool spawned = false;
    [SerializeField] int roomWidth;
    [SerializeField] int roomHeight;
    [SerializeField] int numOfPossibleEnemies;

    public int test;

    private int rand;
    private int numOfEnemies;

    void Start()
    {
        //Debug.Log("Inside Start function for EnemyGeneration");
        templates = FindObjectOfType<EnemyTemplates>();

        if (spawned == false)
        {
            numOfEnemies = Random.Range(1, numOfPossibleEnemies) + 1;
            for(int i=1; i< numOfEnemies; i++)
            {
                rand = Random.Range(0, templates.enemiesFloorOne.Length);
                //Debug.Log("Rand value: " + 1);
                Instantiate(templates.enemiesFloorOne[rand], transform.position, templates.enemiesFloorOne[rand].transform.rotation, gameObject.transform);
            }
            spawned = true;
        }
    }
}
