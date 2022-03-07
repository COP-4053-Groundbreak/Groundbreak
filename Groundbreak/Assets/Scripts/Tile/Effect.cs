using UnityEngine;
using System.Collections.Generic;

public class Effect : MonoBehaviour {

    // ID obtained from adding two elements
    int id;
    string effectName;
    Tile tileUnderEffect;

    int effectDuration;

    // These should probably be put into reaction manager.
    // I.e.: doesn't make sense for earth effect to know the magma damage
    [SerializeField] const int MAGMA_DMG = 25;
    [SerializeField] const int SANDSTORM_DMG = 5;
    [SerializeField] const int SANDSTORM_RANGE = 2;
    [SerializeField] const int STORM_RANGE = 2;
    [SerializeField] const int STORM_DMG = 5;
    // Damage from two units colliding from a push or pull
    [SerializeField] const int CRASH_DMG = 5;
    [SerializeField] const int FIREBALL_DMG = 30;
    [SerializeField] const int FIREBALL_RANGE = 2;
    
    public GridManager gridManager;

    // Some effects should be processed immediately upon creation (i.e. mud, smoke)
    // TODO: Smoke
    public void Initialize(Element a, Element b){
          id = (int)a + (int)b;
          gridManager = FindObjectOfType<GridManager>();  
          tileUnderEffect = GridManager.grid[(int)transform.position.x, (int)transform.position.y];
          switch(id) {
              case ((int)Element.Air + (int)Element.Earth): // Sandstorm PUSHES
                effectName = "Sandstorm";
                Debug.Log("Sandstorm down effect time!"); 
                sandStormDownEffect(tileUnderEffect, SANDSTORM_RANGE, new List<Tile>(), new List<GameObject>());
                break;
            case ((int)Element.Earth + (int)Element.Fire): // Magma
                effectName = "Magma";
                if (tileUnderEffect.gameObjectAbove.tag == "Player" || tileUnderEffect.gameObjectAbove.tag == "Enemy"){
                    dealDamageToChar(tileUnderEffect.gameObject, MAGMA_DMG);
                }
                break;
            case ((int)Element.Water + (int)Element.Earth): // Mud
                effectName = "Mud";
                tileUnderEffect.setMovementModifier(tileUnderEffect.getMovementModifier() - 1);
                break;
            case ((int)Element.Water + (int)Element.Fire): // Smoke
                effectName = "Smoke";
                break;
            case ((int)Element.Air + (int)Element.Fire): // Fireball
                effectName = "Fireball";
                fireballEffect(tileUnderEffect, FIREBALL_RANGE, new List<Tile>(), new List<GameObject>());
                break;
            default: // Storm PULLS
                effectName = "Storm";
                Debug.Log("Storm down effect time!"); 
                stormDownEffect(tileUnderEffect, STORM_RANGE, new List<Tile>(), new List<GameObject>());
                break;
        }
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Sprites/Effects/{effectName}");
        GridManager.grid[(int)transform.position.x, (int)transform.position.y].setEffect(this);
    }

    // TODO: Smoke
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Entered effect collider!");
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy"){
            switch(id){
                case ((int)Element.Air + (int)Element.Earth): // Sandstorm
                    Debug.Log("Entered sandstorm!");
                    // Move GO back to random position of neighbors
                    int[] arr = {-1,0,1};
                    int[] rand = {arr[Random.Range(0,3)], arr[Random.Range(0,3)]};
                    
                    // To make sure they go in SOME direction, though increases odds for one direction
                    if (rand[0] == 0 && rand[1] == 0){
                        rand[0]++;
                    }
                    dealDamageToChar(other.gameObject, SANDSTORM_DMG);
                    pushGO(this.gameObject, new Vector2(rand[0], rand[1]), 1, other.gameObject);
                    break;
                case ((int)Element.Earth + (int)Element.Fire): // Magma
                    Debug.Log("Entered magma!");
                    dealDamageToChar(other.gameObject, MAGMA_DMG);
                    break;
                case ((int)Element.Water + (int)Element.Earth): // Mud
                    // Don't think anything needs to be done if mud is entered, it's an effect that
                    // impacts the player BEFORE entering it and not while on it.
                    break;
                case ((int)Element.Water + (int)Element.Fire): // Smoke
                    break;
                case ((int)Element.Air + (int)Element.Fire): // Fireball
                    Debug.Log("Entered Fireball");
                    // One time effect, shouldn't linger
                    break;
                default: // Storm
                    // Temporarily set character movement to 1 if they have any movement left
                    if (other.gameObject.tag == "Player"){
                        PlayerMovement moveMng = other.gameObject.GetComponent<PlayerMovement>();
                        if (moveMng.currentMovementRemaining > 0){
                            moveMng.currentMovementRemaining = 1;
                        }
                    } else if (other.gameObject.tag == "Enemy"){
                        EnemyStateManager enemyState = other.gameObject.GetComponent<EnemyStateManager>();
                        if (enemyState.enemyMovementRemaining > 0){
                            enemyState.enemyMovementRemaining = 1;
                        }
                    }
                    break;
            }
        }
    }
    // Pushes pushable towards pushOrigin in pushDir direction numPushed tiles
    void pushGO(GameObject pushOrigin, Vector2 pushDir, int numPushed, GameObject pushable){
        Debug.Log($"Pushing in {pushDir.ToString()}");
        // Use a variable first so as to not modify player position. Check if someone there. If so,
        // deal more damage and don't move into that tile
        int postPushX = 0;
        int postPushY = 0;
        
        // "Normalize" vector while keeping negative, then decide which neighbor to go to
        pushDir = new Vector2(pushDir.x/pushDir.magnitude, pushDir.y/pushDir.magnitude);
        if (pushDir.x > 0) { pushDir.x = 1;} else if (pushDir.x < 0) { pushDir.x = -1;}
        if (pushDir.y > 0) { pushDir.y = 1;} else if (pushDir.y < 0) { pushDir.y = -1;}

        Debug.Log($"Adjusted pushDir is {pushDir}");

        // Calculate x position        
        postPushX = (int)(pushable.transform.position.x + pushDir.x * numPushed);
        // If player walks into range, they won't be in center of tile. This centers them.
        if (pushOrigin.transform.position.x < pushable.transform.position.x)
            postPushX = (int)(postPushX + 0.5f);

        // Calculate y position 
        postPushY = (int)(pushable.transform.position.y + pushDir.y * numPushed);
        // If player walks into range, they won't be in center of tile. This centers them.
        if (pushOrigin.transform.position.y < pushable.transform.position.y)
            postPushY = (int)(postPushY + 0.5f);
        
        // Prevent out of bounds pushes
        if (postPushX < 0) {postPushX = 0;}
        else if (postPushX >= gridManager.getWidth()) {postPushX = gridManager.getWidth() - 1;}
        if (postPushY < 0) {postPushY = 0;}
        else if (postPushY >= gridManager.getWidth()) {postPushY = gridManager.getHeight() - 1;}

        Tile endTile = GridManager.grid[postPushX, postPushY];
        
        // There's a character at the tile we're being pushed into
        if (endTile.gameObjectAbove != null && (endTile.gameObjectAbove.tag == "Enemy" || endTile.gameObjectAbove.tag == "Player")){
            dealCrashDamage(endTile.gameObjectAbove, pushable);
            postPushX = (int)pushable.transform.position.x;
            postPushY = (int)pushable.transform.position.y;
        }

        // Set character position
        pushable.transform.position = new Vector2(postPushX, postPushY);

        if (pushable.tag == "Enemy"){
            pushable.GetComponent<EnemyStateManager>().stopEnemyMovement();
        } else if (pushable.tag == "Player"){
            pushable.GetComponent<PlayerMovement>().endMove();
        }
        Debug.Log($"Player new position is {pushable.transform.position}");
    }
    // Pulls pullable towards pullOrigin in pullDir direction numPulled tiles 
    void pullGO(GameObject pullOrigin, Vector2 pullDir, int numPulled, GameObject pullable){
        Debug.Log($"Pulling in {pullDir.ToString()}");
        // Use a variable first so as to not modify player position. Check if someone there. If so,
        // deal more damage and don't move into that tile
        int postPullX = 0;
        int postPullY = 0;
        
        // "Normalize" vector while keeping negative, then decide which neighbor to go to
        pullDir = new Vector2(pullDir.x/pullDir.magnitude, pullDir.y/pullDir.magnitude);
        if (pullDir.x > 0) { pullDir.x = 1;} else if (pullDir.x < 0) { pullDir.x = -1;}
        if (pullDir.y > 0) { pullDir.y = 1;} else if (pullDir.y < 0) { pullDir.y = -1;}

        Debug.Log($"Adjusted pullDir is {pullDir}");

        // Calculate x position        
        postPullX = (int)(pullable.transform.position.x + pullDir.x * numPulled);
        // If player walks into range, they won't be in center of tile. This centers them.
        if (pullOrigin.transform.position.x > pullable.transform.position.x)
            postPullX = (int)(postPullX + 0.5f);

        // Calculate y position 
        postPullY = (int)(pullable.transform.position.y + pullDir.y * numPulled);
        // If player walks into range, they won't be in center of tile. This centers them.
        if (pullOrigin.transform.position.y > pullable.transform.position.y)
            postPullY = (int)(postPullY + 0.5f);
        
        // Prevent out of bounds pulls
        if (postPullX < 0) {postPullX = 0;}
        else if (postPullX >= gridManager.getWidth()) {postPullX = gridManager.getWidth() - 1;}
        if (postPullY < 0) {postPullY = 0;}
        else if (postPullY >= gridManager.getWidth()) {postPullY = gridManager.getHeight() - 1;}

        Tile endTile = GridManager.grid[postPullX, postPullY];
        // There's a character at the tile we're being pulled into
        if (endTile.gameObjectAbove != null && (endTile.gameObjectAbove.tag == "Enemy" || endTile.gameObjectAbove.tag == "Player")){
            dealCrashDamage(endTile.gameObjectAbove, pullable);
            postPullX = (int)pullable.transform.position.x;
            postPullY = (int)pullable.transform.position.y;
        }

        // Set character position
        pullable.transform.position = new Vector2(postPullX, postPullY);

        if (pullable.tag == "Enemy"){
            pullable.GetComponent<EnemyStateManager>().stopEnemyMovement();
        } else if (pullable.tag == "Player"){
            pullable.GetComponent<PlayerMovement>().endMove();
        }
        Debug.Log($"Player new position is {pullable.transform.position}");
    }
    private void dealCrashDamage(GameObject char1, GameObject char2){
        dealDamageToChar(char1, CRASH_DMG);
        dealDamageToChar(char2, CRASH_DMG);
    }
    private void dealDamageToChar(GameObject character, int damageAmount){
        if (character.gameObject.tag == "Player")
            character.gameObject.GetComponentInParent<PlayerStats>().DealDamage(damageAmount);
        else
            character.gameObject.GetComponent<EnemyStateManager>().DealDamage(damageAmount);
    }
    // Works recursively. Pulls units x tiles away, only ONCE
    private void stormDownEffect(Tile startTile, int range, List<Tile> neighborsVisited, List<GameObject> charactersPulled){
        // Debug.Log($"My name is {startTile.name} and I have {startTile.neighbors.Count} neighbors");
        
        // Base case: We've looked as many tiles away as desired
        if (range == 0){
            return;
        }

        // Look at current tiles neighbors
        foreach(Tile neighbor in startTile.neighbors){
            // If haven't visited before, do effect
            // neighbor.GetComponent<Renderer>().material.color = Color.yellow; // TESTING NEIGHBORS INDICATOR
            if (!neighborsVisited.Contains(neighbor)){
                Debug.Log($"Unvisited tile at {neighbor.name}");
                // Make sure there's something to pull and that it's a character
                if (neighbor.gameObjectAbove != null && (neighbor.gameObjectAbove.tag == "Enemy" || neighbor.gameObjectAbove.tag == "Player")){
                    if (!charactersPulled.Contains(neighbor.gameObjectAbove)){
                        dealDamageToChar(neighbor.gameObjectAbove, STORM_DMG);
                        pullGO(this.gameObject, this.transform.position - neighbor.transform.position, 1, neighbor.gameObjectAbove);
                        charactersPulled.Add(neighbor.gameObjectAbove);
                    }
                }
                neighborsVisited.Add(startTile);
                // Repeat but looking at one less set of neighbors, starting at neighbor
                stormDownEffect(neighbor, range - 1, neighborsVisited, charactersPulled);
            }
        }
    }
    private void sandStormDownEffect(Tile startTile, int range, List<Tile> neighborsVisited, List<GameObject> charactersPushed){
        // Debug.Log($"My name is {startTile.name} and I have {startTile.neighbors.Count} neighbors");
        // Base case: We've looked as many tiles away as desired
        if (range == 0){
            return;
        }

        // Look at current tiles neighbors
        foreach(Tile neighbor in startTile.neighbors){
            // neighbor.GetComponent<Renderer>().material.color = Color.yellow; // TESTING NEIGHBORS INDICATOR
            // If haven't visited before, do effect
            if (!neighborsVisited.Contains(neighbor)){
                Debug.Log($"Unvisited tile at {neighbor.name}");
                // Make sure there's something to pull and that it's a character
                if (neighbor.gameObjectAbove != null && (neighbor.gameObjectAbove.tag == "Enemy" || neighbor.gameObjectAbove.tag == "Player")){
                    if (!charactersPushed.Contains(neighbor.gameObjectAbove)){
                        dealDamageToChar(neighbor.gameObjectAbove, SANDSTORM_DMG);
                        pushGO(this.gameObject, neighbor.transform.position -  this.transform.position, 1, neighbor.gameObjectAbove);
                        charactersPushed.Add(neighbor.gameObjectAbove);
                    }
                }
                neighborsVisited.Add(startTile);
                // Repeat but looking at one less set of neighbors, starting at neighbor
                sandStormDownEffect(neighbor, range - 1, neighborsVisited, charactersPushed);
            }
        }
    }
    private void fireballEffect(Tile startTile, int range, List<Tile> neighborsVisited, List<GameObject> charactersDamaged){
        // Debug.Log($"My name is {startTile.name} and I have {startTile.neighbors.Count} neighbors");
        // Base case: We've looked as many tiles away as desired
        if (range == 0){
            return;
        }

        // Look at current tiles neighbors
        foreach(Tile neighbor in startTile.neighbors){
            // neighbor.GetComponent<Renderer>().material.color = Color.yellow; // TESTING NEIGHBORS INDICATOR
            // If haven't visited before, do effect
            if (!neighborsVisited.Contains(neighbor)){
                Debug.Log($"Unvisited tile at {neighbor.name}");
                // Make sure there's something to pull and that it's a character
                if (neighbor.gameObjectAbove != null && (neighbor.gameObjectAbove.tag == "Enemy" || neighbor.gameObjectAbove.tag == "Player")){
                    if (!charactersDamaged.Contains(neighbor.gameObjectAbove)){
                        dealDamageToChar(neighbor.gameObjectAbove, FIREBALL_DMG);
                        charactersDamaged.Add(neighbor.gameObjectAbove);
                    }
                }
                neighborsVisited.Add(startTile);
                // Repeat but looking at one less set of neighbors, starting at neighbor
                fireballEffect(neighbor, range - 1, neighborsVisited, charactersDamaged);
            }
        }
    }
}