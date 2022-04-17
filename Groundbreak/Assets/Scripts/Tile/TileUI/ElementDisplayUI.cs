using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementDisplayUI : MonoBehaviour
{
    [SerializeField] GridManager gridManager;
    [SerializeField] GameObject elemVisual;
    
    // Start is called before the first frame update
    private void Start() {
        gridManager = FindObjectOfType<GridManager>();
        FindObjectOfType<FindNewGridManager>().OnGridChanged += GridChanged;
        if (gridManager == null) Debug.Log("JESUS CHRIST");
        // Create Element UI
        foreach (Tile t in gridManager.grid){
            Instantiate(elemVisual, t.gameObject.transform.position, Quaternion.identity, t.transform);
        }
    }  

    // Currently called all the time and only shown when player presses space.
    // Should probably optimize but works for now
    private void Update() {
        // This should probably be done by some sort of control manager I believe
        if (Input.GetKeyDown(KeyCode.Space)){
            foreach (Tile t in gridManager.grid){
                Debug.Log("Looking through children");
                if (t.transform.childCount > 0){
                    GameObject elemVisual = t.transform.Find("elemVisual(Clone)").gameObject;
                    elemVisual.SetActive(!elemVisual.gameObject.activeInHierarchy);
                    Debug.Log($"After the change, the GO is now {elemVisual.gameObject.activeInHierarchy}");
                }
            }
        }
    }

    private void GridChanged(object sender, System.EventArgs e) {
        gridManager = FindObjectOfType<GridManager>();
        // When the grid changes, need to create all visual effects for new grid
        foreach (Tile t in gridManager.grid){
            Instantiate(elemVisual, t.gameObject.transform.position, Quaternion.identity, t.transform);
        }
    }
}
