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
    private int[] occupied = new int[20];
    private int counter = 0;


    void Start()
    {
        //Debug.Log("Inside Start function for EnemyGeneration");
        templates = FindObjectOfType<EnemyTemplates>();

        if (spawned == false)
        {
            numOfEnemies = Random.Range(1, numOfPossibleEnemies) + 1;
            for(int i=1; i< numOfEnemies; i++)
            {
                spawned = false;

                //Getting random numbers
                rand = Random.Range(0, templates.enemiesFloorOne.Length);
                posRand = Random.Range(0, spawnPointHolder.transform.childCount-1);

                //Debug.Log("posRand = " + posRand);
                //Debug.Log("spawnPointHolder.transform.childCount-1 = " + (spawnPointHolder.transform.childCount - 1));

                //Check if we used this position already
                for (int j = 0; j < counter; j++)
                {
                    //If we hit a match increament and if
                    if (posRand == occupied[counter])
                    {
                        posRand++;
                        //if we reach the limit go back to 0
                        if (posRand == spawnPointHolder.transform.childCount - 1)
                            posRand = 0;
                    }

                }
                //getting random postition
                spawnHere = spawnPointHolder.transform.GetChild(posRand);

                //Make the enemy
                currentEnemy = Instantiate(templates.enemiesFloorOne[rand], spawnHere.position, templates.enemiesFloorOne[rand].transform.rotation, gameObject.transform);

                //Store the postion the enemy spawned then increament
                occupied[counter] = posRand;
                counter++;

                currentEnemy.SetActive(false);
            }
            spawned = true;

            //Delete all the enemy spawnPoints
            foreach (Transform child in spawnPointHolder.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
