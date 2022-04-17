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

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Enemy") || other.CompareTag("Barrel"))
        {
            Debug.Log("Collided with enemy or barrel");
            validPoint = false;
        }
        else if(other.CompareTag("Tile"))
        {
            Tile tile = other.gameObject.GetComponent<Tile>();
            if(tile != null && tile.myElement == Element.Void)
            {
                validPoint = false;
            }
        }
    }
}