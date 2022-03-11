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
    [SerializeField] GameObject spawnPointHolder;
    private Transform spawnHere;
    private GameObject currentEnemy;

    public int test;

    private int rand;
    private int posRand;
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
                posRand = Random.Range(0, spawnPointHolder.transform.childCount-1);

                spawnHere = spawnPointHolder.transform.GetChild(posRand);

                currentEnemy = Instantiate(templates.enemiesFloorOne[rand], spawnHere.position, templates.enemiesFloorOne[rand].transform.rotation, gameObject.transform);
                currentEnemy.SetActive(false);
            }
            spawned = true;

            foreach (Transform child in spawnPointHolder.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
