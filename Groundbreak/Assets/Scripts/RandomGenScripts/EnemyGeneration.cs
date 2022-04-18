using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    GridManager gridManager;

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
        gridManager = gameObject.transform.Find("ThisRoomGridManager").GetComponent<GridManager>();

        if (SceneManager.GetActiveScene().name == "Tutorial") 
        {
            return;
        }


        if (spawned == false)
        {
            numOfEnemies = Random.Range(1, numOfPossibleEnemies) + 1;
            for(int i=1; i< numOfEnemies; i++)
            {
                spawned = false;

                Vector3 localPos;
                int pointX;
                int pointY;

                //Getting random numbers
                rand = Random.Range(0, templates.enemiesFloorOne.Length);
                GameObject currentPoint;
                EnemySpawner spawnPont;

                do
                {
                    posRand = Random.Range(0, spawnPointHolder.transform.childCount - 1);
                    currentPoint = spawnPointHolder.transform.GetChild(posRand).gameObject;
                    spawnPont = currentPoint.GetComponent<EnemySpawner>();

                    localPos = gameObject.transform.InverseTransformPoint(currentPoint.transform.position);
                    pointY = (int)(localPos.y + 5.5f);
                    pointX = (int)(localPos.x + 5.5f);

                    Debug.Log("While Loop Test");
                    
                } while ((gridManager.grid[pointX, pointY].gameObjectAbove && 
                                               (gridManager.grid[pointX, pointY].gameObjectAbove.CompareTag("Barrel") || gridManager.grid[pointX, pointY].gameObjectAbove.CompareTag("Enemy"))) ||
                                               gridManager.grid[pointX, pointY].myElement == Element.Void);


                if(spawnPont == null || spawnPont.validPoint == false)
                    Debug.LogWarning("Da enemy spawn thing did a mess up");

                localPos.x = pointX;
                localPos.y = pointY;
                //Make the enemy
                //Debug.LogWarning(gridManager.grid + " " + pointX + " " + pointY);
                currentEnemy = Instantiate(templates.enemiesFloorOne[rand], gridManager.grid[pointX, pointY].transform.position, templates.enemiesFloorOne[rand].transform.rotation, gameObject.transform);
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
