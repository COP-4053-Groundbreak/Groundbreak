using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeMoveManager : MonoBehaviour
{
    Tile[] tiles;
    [SerializeField] GameObject empty;
    // Start is called before the first frame update
    private void Start()
    {
        // Create colliders for all void
        tiles = FindObjectsOfType<Tile>();
        foreach (Tile tile in tiles) 
        {
            if (tile.getElement() == Element.Void) 
            {
                Transform child = Instantiate(empty.transform, tile.transform);
                child.gameObject.AddComponent<BoxCollider2D>();
            }
        }
    }


}
