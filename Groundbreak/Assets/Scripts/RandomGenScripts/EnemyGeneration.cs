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
    [SerializeField] int MinNumOfPossibleEnemies;
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
            numOfEnemies = Random.Range(MinNumOfPossibleEnemies+1, numOfPossibleEnemies+1);

            //SpawnRanged();

            for(int i=0; i<numOfEnemies-1; i++)
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
                    //Debug.LogError(gridManager.grid[pointX, pointY].gameObjectAbove);
                    //Debug.Log("While Loop Test");

                } while ((gridManager.grid[pointX, pointY].gameObjectAbove != null && 
                                               (gridManager.grid[pointX, pointY].gameObjectAbove.CompareTag("Chest") || gridManager.grid[pointX, pointY].gameObjectAbove.CompareTag("Barrel") || gridManager.grid[pointX, pointY].gameObjectAbove.CompareTag("Enemy"))) ||
                                               gridManager.grid[pointX, pointY].myElement == Element.Void);


                if(spawnPont == null || spawnPont.validPoint == false)
                    Debug.LogWarning("Da enemy spawn thing did a mess up");

                localPos.x = pointX;
                localPos.y = pointY;
                //Make the enemy
                //Debug.LogWarning(gridManager.grid + " " + pointX + " " + pointY);
                currentEnemy = Instantiate(templates.enemiesFloorOne[rand], gridManager.grid[pointX, pointY].transform.position, templates.enemiesFloorOne[rand].transform.rotation, gameObject.transform);
                gridManager.grid[pointX, pointY].gameObjectAbove = currentEnemy;
                currentEnemy.SetActive(false);
            }
            spawned = true;


            StartCoroutine(DelayAndShow(spawnPointHolder, 5f));
        }
    }

    IEnumerator DelayAndShow(GameObject spawnPointHolder, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Kill(spawnPointHolder);
    }

    public void Kill(GameObject spawnPointHolder)
    {
        foreach (Transform child in spawnPointHolder.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void SpawnRanged()
    {
        //Debug.LogWarning("Ranged Spawned");

        Vector3 localPos;
        int pointX;
        int pointY;

        //Getting random numbers
        rand = Random.Range(0, templates.enemiesRanged.Length);
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
            //Debug.LogError(gridManager.grid[pointX, pointY].gameObjectAbove);
            //Debug.Log("While Loop Test");

        } while ((gridManager.grid[pointX, pointY].gameObjectAbove != null &&
                                       (gridManager.grid[pointX, pointY].gameObjectAbove.CompareTag("Barrel") || gridManager.grid[pointX, pointY].gameObjectAbove.CompareTag("Enemy"))) ||
                                       gridManager.grid[pointX, pointY].myElement == Element.Void);


        if (spawnPont == null || spawnPont.validPoint == false)
            Debug.LogWarning("Da enemy spawn thing did a mess up");

        localPos.x = pointX;
        localPos.y = pointY;
        //Make the enemy
        //Debug.LogWarning(gridManager.grid + " " + pointX + " " + pointY);
        currentEnemy = Instantiate(templates.enemiesRanged[rand], gridManager.grid[pointX, pointY].transform.position, templates.enemiesFloorOne[rand].transform.rotation, gameObject.transform);
        //Debug.LogError("Setting at " + pointX + " " + pointY);
        gridManager.grid[pointX, pointY].gameObjectAbove = currentEnemy;
        currentEnemy.SetActive(false);
    }
}
