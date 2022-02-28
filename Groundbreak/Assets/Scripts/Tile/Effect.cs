using UnityEngine;

public class Effect : MonoBehaviour {

    // ID obtained from adding two elements
    int id;
    SpriteRenderer mySpriteRenderer;
    string effectName;

    void Awake(){
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
        mySpriteRenderer.sprite = Resources.Load<Sprite>($"Assets/Sprites/Effects/{effectName}.png");
        GridManager.grid[(int)transform.position.x, (int)transform.position.y].setEffect(this);
    }
}