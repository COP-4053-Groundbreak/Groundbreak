using UnityEngine;

public class Effect : MonoBehaviour {

    // ID obtained from adding two elements
    int id;
    string effectName;
    Tile tileUnderEffect;


    // Some effects should be processed immediately upon creation (i.e. mud, smoke)
    public void Initialize(Element a, Element b){
          id = (int)a + (int)b;  
          tileUnderEffect = GridManager.grid[(int)transform.position.x, (int)transform.position.y];
          switch(id){
            case ((int)Element.Air + (int)Element.Earth): // Sandstorm
                effectName = "Sandstorm";
                break;
            case ((int)Element.Earth + (int)Element.Fire): // Magma
                effectName = "Magma";
                break;
            case ((int)Element.Water + (int)Element.Earth): // Mud
                effectName = "Mud";
                tileUnderEffect.setMovementModifier(tileUnderEffect.getMovementModifier() - 1);
                break;
            case ((int)Element.Water + (int)Element.Fire): // Smoke
                effectName = "Smoke";
                break;
            case ((int)Element.Air + (int)Element.Fire): // Spreadfire
                effectName = "Spreadfire";
                break;
            default: // Storm
                effectName = "Storm";
                // Check each tile adjacent to tile effect is on
                // Each player or enemy should get pushed by this
                foreach (Tile adjTile in tileUnderEffect.neighbors){
                    if (adjTile.gameObjectAbove != null){
                        if (adjTile.gameObjectAbove.tag == "Player" || adjTile.gameObjectAbove.tag == "Enemy"){
                            // Push characters
                            Debug.Log($"Pushing {adjTile.gameObjectAbove.name}");
                            pushGO(this.gameObject, adjTile.transform.position - this.transform.position, 1, adjTile.gameObjectAbove);
                        }
                    }
                }
                break;
        }
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Sprites/Effects/{effectName}");
        GridManager.grid[(int)transform.position.x, (int)transform.position.y].setEffect(this);
    }

    //TODO: Fix GO moving out of tile while another remains on top
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Entered effect collider!");
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy"){
            switch(id){
                case ((int)Element.Air + (int)Element.Earth): // Sandstorm
                    Debug.Log("Entered sandstorm!");
                    break;
                case ((int)Element.Earth + (int)Element.Fire): // Magma
                    Debug.Log("Entered magma!");
                    other.gameObject.GetComponentInParent<PlayerStats>().DealDamage(25);
                    break;
                case ((int)Element.Water + (int)Element.Earth): // Mud
                    // Don't think anything needs to be done if mud is entered, it's an effect that
                    // impacts the player BEFORE entering it and not while on it.
                    break;
                case ((int)Element.Water + (int)Element.Fire): // Smoke
                    
                    break;
                case ((int)Element.Air + (int)Element.Fire): // Spreadfire
                    Debug.Log("Entered spreadfire");
                    // Spread fire to three tiles in cone from the direction the tile was thrown
                    // Draw line from player to tile under effect
                    // First three tiles this line would touch after touching the efffect become tiles
                    // with lit fire
                    break;
                default: // Storm
                    Debug.Log("Entered Storm!");
                    // Move GO back to random position of neighbors
                    int[] arr = {-1,0,1};
                    
                    int[] rand = {arr[Random.Range(0,3)], arr[Random.Range(0,3)]};
                    
                    if (rand[0] == 0 && rand[1] == 0){
                        rand[0]++;
                    }
                    
                    pushGO(this.gameObject, new Vector2(rand[0], rand[1]), 1, other.gameObject);
                    break;
            }
        }
    }

    void pushGO(GameObject pushOrigin, Vector2 pushDir, int numPushed, GameObject pushable){
        // Move the game object
        int[] arr = {-1, 1};

        // If statements adjust GO pos so it's centered. Since GO gets move before they
        // reach the center of the tile, we need to figure out how to center them.
        // GO coming from right
        if (pushOrigin.transform.position.x < pushable.transform.position.x)
            pushable.transform.position = new Vector3((int)pushable.transform.position.x, pushable.transform.position.y, 0)
                                          + new Vector3(pushDir.x * numPushed, 0, 0); 
        else // GO coming from left
            pushable.transform.position = new Vector3((int)(pushable.transform.position.x + .5), pushable.transform.position.y, 0)
                                          + new Vector3(pushDir.x * numPushed, 0, 0); 
        // GO coming from above
        if (pushOrigin.transform.position.y < pushable.transform.position.y)
            pushable.transform.position  = new Vector3(pushable.transform.position.x, (int)pushable.transform.position.y, 0)
                                           + new Vector3(0, pushDir.y * numPushed);
        else // GO coming from under
            pushable.transform.position  = new Vector3(pushable.transform.position.x, (int)(pushable.transform.position.y + .5) , 0)
                                           + new Vector3(0, pushDir.y * numPushed);
        // Make sure if they are a player or enemy they don't 
        // TODO: Implement enemy 
        if (pushable.tag == "Enemy"){
            
        } else if (pushable.tag == "Player"){
            pushable.GetComponent<PlayerMovement>().endMove();
        }
        Debug.Log($"Player new position is {pushable.transform.position}");
    }
}