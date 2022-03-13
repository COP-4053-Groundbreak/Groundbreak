using UnityEngine;
using System.Collections.Generic;

public class Effect : MonoBehaviour {

    // ID obtained from adding two elements
    int id;
    string effectName;
    public Tile tileUnderEffect;
    [SerializeField] public int myDuration = -1;
    public Animator myAnimator;
        
    public GridManager gridManager;
    // int posX,posY;
    GameObject currRoom;
    public Vector2 myPos;

    private void Start()
    {
        FindObjectOfType<FindNewGridManager>().OnGridChanged += GridChanged;
        
    }

    // TOD0: FIX EFFECTS USING TRANSFORM.POSITION
    // FOR EFFECT POS: tileUnderEffect.GetX(), tileUnderEffect.GetY();
    // FOR ENEMY POS: enemyX, enemyY in EnemyStateManager;
    // FOR PLAYER POS: playerX, playerY in PlayerMovement;

    // Some effects should be processed immediately upon creation (i.e. mud, smoke)
    public void Initialize(Element a, Element b, Vector2 pos){
          id = (int)a + (int)b;
          gridManager = FindObjectOfType<GridManager>();  
          myPos = pos;
          myAnimator = GetComponent<Animator>();
          
          //currRoom = gridManager.transform.parent.gameObject;
          //Vector2 localPos = currRoom.transform.InverseTransformPoint(transform.position);
          //posX = (int)(localPos.x);
          //posY = (int)(localPos.y);
          // Debug.LogWarning($"Init effect at {pos.x},{pos.y}");
          tileUnderEffect = gridManager.getTile(pos.x, pos.y);
          myAnimator.SetInteger("effectID", id);
          switch(id) {
              case ((int)Element.Air + (int)Element.Earth): // Sandstorm 
                effectName = "Sandstorm";
                Debug.Log("Sandstorm down effect time!"); 
                sandStormDownEffect(tileUnderEffect, ReactionManager.SANDSTORM_RANGE, new List<Tile>(), new List<GameObject>());
                myDuration = ReactionManager.SANDSTORM_DUR;
                this.transform.localScale = new Vector2(5,5);
                this.GetComponent<BoxCollider2D>().size = new Vector2(0.1f,0.1f);
                break;
            case ((int)Element.Earth + (int)Element.Fire): // Magma 
                effectName = "Magma";
                if (tileUnderEffect.gameObjectAbove != null){
                    if (tileUnderEffect.gameObjectAbove.tag == "Player" || tileUnderEffect.gameObjectAbove.tag == "Enemy"){
                        dealDamageToChar(tileUnderEffect.gameObjectAbove, ReactionManager.MAGMA_DMG);
                    }
                }
                myDuration = ReactionManager.MAGMA_DUR;
                break;
            case ((int)Element.Water + (int)Element.Earth): // Mud
                effectName = "Mud";
                tileUnderEffect.setMovementModifier(tileUnderEffect.getMovementModifier() - ReactionManager.MUD_DEBUFF);
                myDuration = ReactionManager.MUD_DUR;
                break;
            case ((int)Element.Water + (int)Element.Fire): // Smoke 
                effectName = "Smoke";
                myDuration = ReactionManager.SMOKE_DUR;
                this.transform.localScale = new Vector2(5,5);
                this.GetComponent<BoxCollider2D>().size = new Vector2(0.1f,0.1f);
                this.transform.position = this.transform.position + new Vector3(0,0.5f);
                break;
            case ((int)Element.Air + (int)Element.Fire): // Fireball 
                effectName = "Fireball";
                if (tileUnderEffect.gameObjectAbove != null)
                    dealDamageToChar(tileUnderEffect.gameObjectAbove, ReactionManager.FIREBALL_DMG);
                fireballEffect(tileUnderEffect, ReactionManager.FIREBALL_RANGE, new List<Tile>(), new List<GameObject>());
                this.transform.localScale = new Vector2(5,5);
                this.GetComponent<BoxCollider2D>().size = new Vector2(0.1f,0.1f);
                myDuration = ReactionManager.FIREBALL_DUR;
                transform.position = transform.position - new Vector3(-0.5f, 0.5f);
                break;
            case ((int)Element.Air + (int)Element.Water): // Storm 
                effectName = "Storm";
                Debug.Log("Storm down effect time!"); 
                stormDownEffect(tileUnderEffect, ReactionManager.STORM_RANGE, new List<Tile>(), new List<GameObject>());
                myDuration = ReactionManager.STORM_DUR;
                this.transform.localScale = new Vector2(5,5);
                this.GetComponent<BoxCollider2D>().size = new Vector2(0.1f,0.1f);
                break;
            default:
                Debug.LogWarning("You are never supposed to be here!");
                break;
        }
        this.name = effectName;
        //GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Sprites/Effects/{effectName}");
        tileUnderEffect.setEffect(this);
    }
    
    public void reduceFireballDuration(){
        ReactionManager.reduceDuration(this);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Entered effect collider!");
        if (other == null || other.gameObject == null)
            return;
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

                    dealDamageToChar(other.gameObject, ReactionManager.SANDSTORM_DMG);
                    goToOpposite(this.gameObject, other.gameObject);
                    // pushGO(this.gameObject, new Vector2(rand[0], rand[1]), 1, other.gameObject);
                    break;
                case ((int)Element.Earth + (int)Element.Fire): // Magma
                    Debug.Log("Entered magma!");
                    dealDamageToChar(other.gameObject, ReactionManager.MAGMA_DMG);
                    break;
                case ((int)Element.Water + (int)Element.Earth): // Mud
                    // Don't think anything needs to be done if mud is entered, it's an effect that
                    // impacts the player BEFORE entering it and not while on it.
                    if (other.gameObject.tag == "Player"){
                        PlayerMovement moveMng = other.gameObject.GetComponent<PlayerMovement>();
                        if (moveMng.currentMovementRemaining > 0){
                            moveMng.currentMovementRemaining = 0;
                        }
                    } else if (other.gameObject.tag == "Enemy"){
                        EnemyStateManager enemyState = other.gameObject.GetComponent<EnemyStateManager>();
                        if (enemyState.enemyMovementRemaining > 0){
                            other.gameObject.transform.position = this.transform.position;
                            enemyState.stopEnemyMovement();
                            enemyState.enemyMovementRemaining = 2;
                            enemyState.isEnemyTurn = false;
                        }
                    }
                    break;
                case ((int)Element.Water + (int)Element.Fire): // Smoke
                    if (other.gameObject.tag == "Player"){
                        other.gameObject.GetComponent<PlayerActions>().throwRange -= ReactionManager.SMOKE_RANGE_PLAYER_MOD;
                    } else if (other.gameObject.tag == "Enemy"){
                        other.gameObject.GetComponent<EnemyStateManager>().visibilityRange -= ReactionManager.SMOKE_RANGE_ENEMY_MOD;
                    }
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
                    dealDamageToChar(other.gameObject, ReactionManager.STORM_DMG);
                    break;
            }
        }
    }
    public void goToOpposite(GameObject tpOrigin, GameObject character){
        int charX = 0, charY = 0;
        if (character.tag == "Player"){
            charX = character.GetComponent<PlayerMovement>().playerX;
            charY = character.GetComponent<PlayerMovement>().playerY;
        } else if (character.tag == "Enemy"){
            charX = character.GetComponent<EnemyStateManager>().enemyX;
            charY = character.GetComponent<EnemyStateManager>().enemyY;
        }
        Debug.Log($"Character is at {charX},{charY}");
        Debug.Log($"Character global position is {character.transform.position}");

        int newX = 0;
        int newY = 0;

        int tempX = (int)tpOrigin.GetComponent<Effect>().myPos.x;
        int tempY = (int)tpOrigin.GetComponent<Effect>().myPos.y;
        Debug.Log(tempX + "qwer,qwer" + tempY);

        if (tpOrigin.transform.position.y > character.transform.position.x){
            Debug.Log("Character came from under!");
            if (tempY + 1 > gridManager.getHeight()){
                newY = tempY - 1;
            } else {
                newY = tempY + 1;
            }  
            Debug.Log("newY new value" + newY +"qwer,qwer");
        } else if (tpOrigin.transform.position.y < character.transform.position.y){
            Debug.Log("Character came from above!");
            if (tempY - 1 < 0){
                newY = tempY + 1;
            } else {
                newY = tempY - 1;
            }
            Debug.Log("newY new value" + newY +"qwer,qwer");  
        } else {
            newY = tempY;
        }
        if (tpOrigin.transform.position.x < character.transform.position.x){
            Debug.Log("Character came from right!");
            if (tempX + 1 > gridManager.getWidth()){
                newX = tempX - 1;
            } else {
                newX = tempX + 1;
            }  
        } else if (tpOrigin.transform.position.x > character.transform.position.x){
            Debug.Log("Character came from left!");
            if (tempX - 1 < 0){
                newX = tempX + 1;
            } else {
                newX = tempX - 1;
            }  
        } else {
            newX = tempX;
        }
        if (gridManager.getTile(newX, newY).gameObjectAbove != null){
            newX = charX;
            newY = charY;
            dealCrashDamage(character, null);
        }
        character.transform.position = gridManager.getTile(newX, newY).transform.position;
        Debug.Log("TP DESTINATION IS " + newX + "qwer,qwer" + newY);
        if (character.tag == "Enemy"){
            character.GetComponent<EnemyStateManager>().stopEnemyMovement();
        } else if (character.tag == "Player"){
            character.GetComponent<PlayerMovement>().endMove();
            character.GetComponent<PlayerMovement>().UpdateTilesAfterMove();
        }
    }

    // Pushes pushable towards pushOrigin in pushDir direction numPushed tiles
    public void pushGO(GameObject pushOrigin, Vector2 pushDir, int numPushed, GameObject pushable){
        Debug.Log($"Pushing in {pushDir.ToString()}");
        // Use a variable first so as to not modify player position. Check if someone there. If so,
        // deal more damage and don't move into that tile
        int pushableX = 0;
        int pushableY = 0;
        if (pushable.gameObject.tag == "Enemy") {
            pushableX = pushable.gameObject.GetComponent<EnemyStateManager>().enemyX;
            pushableY = pushable.gameObject.GetComponent<EnemyStateManager>().enemyY;
        } else if (pushable.gameObject.tag == "Player") {
            pushableX = pushable.gameObject.GetComponent<PlayerMovement>().playerX;
            pushableY = pushable.gameObject.GetComponent<PlayerMovement>().playerY;
        } else {
            Debug.Log("Why are we here?");
        }
        // Debug.Log($"Our pushable is {pushable.name}");
        // Debug.Log($"These are the x and y of pushable {pushableX} {pushableY}");
        
        int postPushX = 0;
        int postPushY = 0;
        
        // "Normalize" vector while keeping negative, then decide which neighbor to go to
        pushDir = new Vector2(pushDir.x/pushDir.magnitude, pushDir.y/pushDir.magnitude);
        if (pushDir.x > 0) { pushDir.x = 1;} else if (pushDir.x < 0) { pushDir.x = -1;}
        if (pushDir.y > 0) { pushDir.y = 1;} else if (pushDir.y < 0) { pushDir.y = -1;}

        // if (pushDir.x > 0)
            // Debug.Log("Pushable will be pushed right!");
        // else if(pushDir.x < 0) {
            // Debug.Log("Pushable will be pushed left!");
        // }

        // if (pushDir.y > 0){
            // Debug.Log("Pushable will be pushed up!");
        // } else if (pushDir.y < 0){
            // Debug.Log("Pushable will be pushed down!");
        // }

        // Calculate x position        
        postPushX = (int)(pushableX + pushDir.x * numPushed);
        
        Vector2 tilePos = gridManager.getTile(pushableX, pushableY).transform.position;
        if (pushable.transform.position.x < tilePos.x){
            Debug.Log("Guy moved to right into this effect");
        } else if (pushable.transform.position.x > tilePos.x){
            Debug.Log("Guy moved to the left into this effect");
        }
        // If player walks into range, they won't be in center of tile. This centers them.
        if (pushOrigin.transform.position.x < pushableX)
            postPushX = (int)(postPushX + 0.5f);

        // Calculate y position 
        postPushY = (int)(pushableY + pushDir.y * numPushed);
        // If player walks into range, they won't be in center of tile. This centers them.
        if (pushOrigin.transform.position.y < pushableY)
            postPushY = (int)(postPushY + 0.5f);
        Debug.Log($"Pushable should be pushed into the tile at {postPushX} and {postPushY}");

        // Prevent out of bounds pushes
        if (postPushX < 0) {postPushX = 0;}
        else if (postPushX >= gridManager.getWidth()) {postPushX = gridManager.getWidth() - 1; dealCrashDamage(pushable, null);}
        if (postPushY < 0) {postPushY = 0;}
        else if (postPushY >= gridManager.getWidth()) {postPushY = gridManager.getHeight() - 1; dealCrashDamage(pushable, null);}

        Tile endTile = gridManager.getTile(postPushX, postPushY);
        Debug.LogWarning($" Pushing {pushable.gameObject.name} to {new Vector2(postPushX, postPushY)}");
        
        // There's a character at the tile we're being pushed into
        if (endTile.gameObjectAbove != null && (endTile.gameObjectAbove.tag == "Enemy" || endTile.gameObjectAbove.tag == "Player")){
            Debug.LogWarning("Someone's here!");
            dealCrashDamage(endTile.gameObjectAbove, pushable);
            postPushX = pushableX;
            postPushY = pushableY;
        }

        // Set character position
        pushable.transform.position = gridManager.getTile(postPushX, postPushY).transform.position;

        if (pushable.tag == "Enemy"){
            pushable.GetComponent<EnemyStateManager>().stopEnemyMovement();
        } else if (pushable.tag == "Player"){
            pushable.GetComponent<PlayerMovement>().endMove();
            pushable.GetComponent<PlayerMovement>().UpdateTilesAfterMove();
        }
        Debug.Log($"Player new position is {pushable.transform.position}");

    }
    // Pulls pullable towards pullOrigin in pullDir direction numPulled tiles 
    public void pullGO(GameObject pullOrigin, Vector2 pullDir, int numPulled, GameObject pullable){
        Debug.Log($"Pulling in {pullDir.ToString()}");
        // Use a variable first so as to not modify player position. Check if someone there. If so,
        // deal more damage and don't move into that tile
        int postPullX = 0;
        int postPullY = 0;
        
        int pullableX = 0;
        int pullableY = 0;
        if (pullable.gameObject.tag == "Enemy") {
            pullableX = pullable.gameObject.GetComponent<EnemyStateManager>().enemyX;
            pullableY = pullable.gameObject.GetComponent<EnemyStateManager>().enemyY;
        } else if (pullable.gameObject.tag == "Player") {
            pullableX = pullable.gameObject.GetComponent<PlayerMovement>().playerX;
            pullableY = pullable.gameObject.GetComponent<PlayerMovement>().playerY;
        } else {
            Debug.Log("Why are we here?");
        }
        
        // "Normalize" vector while keeping negative, then decide which neighbor to go to
        pullDir = new Vector2(pullDir.x/pullDir.magnitude, pullDir.y/pullDir.magnitude);
        if (pullDir.x > 0) { pullDir.x = 1;} else if (pullDir.x < 0) { pullDir.x = -1;}
        if (pullDir.y > 0) { pullDir.y = 1;} else if (pullDir.y < 0) { pullDir.y = -1;}

        // Debug.Log($"Adjusted pullDir is {pullDir}");

        // Calculate x position        
        postPullX = (int)(pullableX + pullDir.x * numPulled);
        // If player walks into range, they won't be in center of tile. This centers them.
        if (pullOrigin.transform.position.x > pullable.transform.position.x)
            postPullX = (int)(postPullX + 0.5f);

        // Calculate y position 
        postPullY = (int)(pullableY + pullDir.y * numPulled);
        // If player walks into range, they won't be in center of tile. This centers them.
        if (pullOrigin.transform.position.y > pullable.transform.position.y)
            postPullY = (int)(postPullY + 0.5f);
        
        // Prevent out of bounds pulls
        if (postPullX < 0) {postPullX = 0;}
        else if (postPullX >= gridManager.getWidth()) {postPullX = gridManager.getWidth() - 1; dealCrashDamage(pullable, null);}
        if (postPullY < 0) {postPullY = 0;}
        else if (postPullY >= gridManager.getWidth()) {postPullY = gridManager.getHeight() - 1; dealCrashDamage(pullable, null);}

        Tile endTile = gridManager.getTile(postPullX, postPullY);
        
        // There's a character at the tile we're being pulled into
        if (endTile.gameObjectAbove != null && (endTile.gameObjectAbove.tag == "Enemy" || endTile.gameObjectAbove.tag == "Player")){
            dealCrashDamage(endTile.gameObjectAbove, pullable);
            postPullX = (int)pullableX;
            postPullY = (int)pullableY;
        }

        // Set character position
        pullable.transform.position = gridManager.getTile(postPullX, postPullY).transform.position;

        if (pullable.tag == "Enemy"){
            pullable.GetComponent<EnemyStateManager>().stopEnemyMovement();
        } else if (pullable.tag == "Player"){
            pullable.GetComponent<PlayerMovement>().endMove();
            pullable.GetComponent<PlayerMovement>().UpdateTilesAfterMove();
        }
        Debug.Log($"Player new position is {pullable.transform.position}");
    }
    private void dealCrashDamage(GameObject char1, GameObject char2){
        if (char1 != null)
            dealDamageToChar(char1, ReactionManager.CRASH_DMG);
        if (char2 != null)
            dealDamageToChar(char2, ReactionManager.CRASH_DMG);
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
                        dealDamageToChar(neighbor.gameObjectAbove, ReactionManager.STORM_DMG);
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
                //neighbor.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
                Debug.Log($"Unvisited tile at {neighbor.name}");
                if (neighbor.gameObjectAbove != null) Debug.Log($"On Tile at {neighbor.transform.position} there is a {neighbor.gameObjectAbove.name}");
                // Make sure there's something to pull and that it's a character
                if (neighbor.gameObjectAbove != null && (neighbor.gameObjectAbove.tag == "Enemy" || neighbor.gameObjectAbove.tag == "Player")){
                    if (!charactersPushed.Contains(neighbor.gameObjectAbove)){
                        dealDamageToChar(neighbor.gameObjectAbove, ReactionManager.SANDSTORM_DMG);
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
                        dealDamageToChar(neighbor.gameObjectAbove, ReactionManager.FIREBALL_DMG);
                        charactersDamaged.Add(neighbor.gameObjectAbove);
                    }
                }
                neighborsVisited.Add(startTile);
                // Repeat but looking at one less set of neighbors, starting at neighbor
                fireballEffect(neighbor, range - 1, neighborsVisited, charactersDamaged);
            }
        }
    }
    private void GridChanged(object sender, System.EventArgs e)
    {
        gridManager = FindObjectOfType<GridManager>();
    }
}