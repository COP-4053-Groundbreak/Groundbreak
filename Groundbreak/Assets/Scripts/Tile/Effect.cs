using UnityEngine;

public class Effect : MonoBehaviour {

    // ID obtained from adding two elements
    int id;
    string effectName;

    void Awake(){
    }

    public void Initialize(Element a, Element b){
          id = (int)a + (int)b;  
          switch(id){
            case ((int)Element.Air + (int)Element.Earth): // Sandstorm
                effectName = "Sandstorm";
                break;
            case ((int)Element.Earth + (int)Element.Fire): // Magma
                effectName = "Magma";
                break;
            case ((int)Element.Water + (int)Element.Earth): // Mud
                effectName = "Mud";
                break;
            case ((int)Element.Water + (int)Element.Fire): // Smoke
                effectName = "Smoke";
                break;
            case ((int)Element.Air + (int)Element.Fire): // Spreadfire
                effectName = "Spreadfire";
                break;
            default: // Storm
                effectName = "Storm";
                break;
        }
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Sprites/Effects/{effectName}");
        GridManager.grid[(int)transform.position.x, (int)transform.position.y].setEffect(this);
    }
}