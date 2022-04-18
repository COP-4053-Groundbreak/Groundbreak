using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public bool validPoint = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setInvalid(EnemySpawner currentPoint)
    {
        currentPoint.validPoint = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Spawner Collided with something");

        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Barrel"))
        {
            Debug.Log("Collided with enemy or barrel");
            validPoint = false;
        }
        else if(other.gameObject.CompareTag("Tile"))
        {
            Debug.Log("Collided with Void");

            Tile tile = other.gameObject.GetComponent<Tile>();
            if(tile != null && tile.myElement == Element.Void)
            {
                Debug.Log("Collided with Void");
                validPoint = false;
            }
        }
    }
}