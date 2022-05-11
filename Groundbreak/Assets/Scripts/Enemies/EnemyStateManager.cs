using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class EnemyStateManager : MonoBehaviour, IPointerDownHandler
{
    bool isPlayingFootstep = false;
    // zombie health at start of player round
    int zombieHealthStartOfPlayer;
    // boolean for checking if its the enemies turn.
    public bool isEnemyTurn = false; 
    // sprite rednderer
    public SpriteRenderer mySpriteRenderer;
    // animator
    public Animator animator;
    public bool elementOnEnemy;
    // currentState will hold a reference to the active state in the state machine. 
    EnemyBaseState currentState;
    Tile firstDestinationTile;
    // Rigidbody of the enemy, used for patrolling/movement
    // new Rigidbody2D rigidbody2D;
    Pathfinding pathfinding;
    // Sliding 
    [SerializeField] float slideSpeed = 1f;
    public List<Transform> slidingPath;
    public bool isSliding = false;
    int waypointIndex = 0;
    // instantiate  each concrete state
    public EnemyAttackState AttackState = new EnemyAttackState();
    public EnemyMoveState MoveState = new EnemyMoveState();
    public EnemyPatrollingState PatrollingState = new EnemyPatrollingState();
    public EnemyIdleState IdleState = new EnemyIdleState();
    public EnemyDeathState DeathState = new EnemyDeathState();

    // attacking
    public int attackCounter = 0;
    int archerDamage = 5;
    int swordDamage = 10;
    int greenswordDamage = 15;
    int greenMageDamage = 20;
    int mageDamage = 15;
    int bossDamage = 20;
    int goblinDamage = 20;
    int treeDamage = 10;
    int mushroomDamage = 5;
    int trollDamage = 25;

    int retreatCounter = 0;
    // blocking
    bool triedToBlock = false;
    bool playSound = false;

    // movement
    public List<Transform> listOfTiles;

    // health stuff
    public int startHealth = 50;
    public Transform pfHealthBar;
    public Transform elementSymbol;
    public HealthSystem healthSystem;
    public bool alive = true;

    // enemy movement 
    public int enemyMovementRemaining;
    public int initiative = 0;
    // integer, each block will be 1 unit or however we coded it. 
    public float visibilityRange;
    public float customRange;
    // attack stats:
    // posible for enemy to miss, 0 miss, 1 attack. rename this to attackChance or something. 
    // public bool canAttack;
    // how much damage the enemy will do if canAttack is 1.
    // public int attackDamage;
    [SerializeField] public Element myElement;

    // turn logic
    TurnLogic turnLogic;


    // stuff for pathfinding.
    public int width;
    public int height;

    public float period = 0.0f;

    float attackClipLength;
    int canRetreat;

    // Enemy position variables -N
    public int enemyX;
    public int enemyY;
    // Start is called before the first frame update
    void Start()
    {
        if (!currRoom)
        {
            currRoom = FindObjectOfType<GridManager>().gameObject.transform.parent.gameObject;
        }
        localPos = currRoom.transform.InverseTransformPoint(transform.position);
        enemyX = (int)(localPos.x + 5.5f);
        enemyY = (int)(localPos.y + 5.5f);
        if (SceneManager.GetActiveScene().name == "Tutorial") 
        {
            mageDamage = 10;
            swordDamage = 5;
        }


        if(gameObject.name.Contains("Zombie")){
            customRange = 3f;
        }
        else if(gameObject.name.Contains("Wizard")){
            customRange = 3f;
        }
        else if(gameObject.name.Contains("Archer")){
            customRange = 4f;
        }
        else if(gameObject.name.Contains("Troll")){
            customRange = 3f;
        }
        else{
            customRange = 1.42f;
        }
        visibilityRange = customRange;
        canRetreat = 1;
        // elementSymbol.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(elementToLoad);
        // get animation clip length
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach(AnimationClip clip in clips){
            if(clip.name == "Attack"){
                attackClipLength = clip.length;
            }
        }
        turnLogic = FindObjectOfType<TurnLogic>();
        if(gameObject.name.Contains("Mushroom")){
            enemyMovementRemaining = 5;
        }
        else if(gameObject.name.Contains("Goblin")){
            enemyMovementRemaining = 4;
        }
        else if(gameObject.name.Contains("Warrior")){
            enemyMovementRemaining = 2;
        }
        else if(gameObject.name.Contains("Troll")){
            enemyMovementRemaining = 2;
        }
        else{
            enemyMovementRemaining = 2;
        }
        // get sprite renderer
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        // health system stuff 
        healthSystem = new HealthSystem(startHealth);
        // Health bar stuff
        // Transform healthBarTransform = Instantiate(pfHealthBar, new Vector3(0, 10), Quaternion.identity );
        Transform healthBarTransform = Instantiate(pfHealthBar, gameObject.transform);
        Transform elementSymbolTransform = Instantiate(elementSymbol, gameObject.transform);
        if(gameObject.name.Contains("Zombie")){
            Vector3 healthBarLocalPosition = new Vector3(0, (float)1.25);
            healthBarTransform.localPosition = healthBarLocalPosition;
            HealthBar healthBar = healthBarTransform.GetComponent<HealthBar>();
            healthBar.Setup(healthSystem);
        }
        else if(gameObject.name.Contains("evil") || gameObject.name.Contains("Goblin") || gameObject.name.Contains("Mushroom")){
            // scale down element symbol. 
            Vector3 newScale = elementSymbolTransform.localScale;
            newScale *= 0.5f;
            elementSymbolTransform.localScale = newScale;
            // move element symbol
            Vector3 elementSymbolLocalPosition = new Vector3((float)-0.37, (float)0.32);
            elementSymbolTransform.localPosition = elementSymbolLocalPosition;

            // health bar position. 
            Vector3 healthBarLocalPosition = new Vector3((float)-0.05, (float)0.75);
            healthBarTransform.localPosition = healthBarLocalPosition;
            HealthBar healthBar = healthBarTransform.GetComponent<HealthBar>();
            // scale down health bar 
            Vector3 newScaleHealth = healthBarTransform.localScale;
            newScaleHealth *= 0.5f;
            healthBarTransform.localScale = newScaleHealth;
            // initiate health bar. 
            healthBar.Setup(healthSystem);
        }
        else if(gameObject.name.Contains("Skeleton") || gameObject.name.Contains("Troll")){
            if(gameObject.name.Contains("Troll")){
                Vector3 elementSymbolLocalPosition = new Vector3((float)-0.65, (float)0.75);
                elementSymbolTransform.localPosition = elementSymbolLocalPosition;
                Vector3 newScale = elementSymbolTransform.localScale;
                newScale *= 1.25f;
                elementSymbolTransform.localScale = newScale;
            }
            Vector3 healthBarLocalPosition = new Vector3(0, (float)1.60);
            healthBarTransform.localPosition = healthBarLocalPosition;
            HealthBar healthBar = healthBarTransform.GetComponent<HealthBar>();
            healthBar.Setup(healthSystem);
        }

        // path finding stuff  COMMENTED FOR NOW, WE ARE GOING TO START IN IDLE STATE. 
        width = FindObjectOfType<GridManager>().getWidth();
        height = FindObjectOfType<GridManager>().getHeight();
        pathfinding = FindObjectOfType<Pathfinding>();
        currentState = IdleState;
        // "this" is a refrence to the context of the enemy. 
        currentState.EnterState(this);
    }

    IEnumerator WaitAndSpawnLadder() 
    {
        
        yield return new WaitForSeconds(2.95f);

        if (gameObject.name.Contains("Troll"))
        {
            FindObjectOfType<PlayerActions>().playerStats.playerActiveItem = null;
            LevelManager.LoadWin();
        }
        else 
        {
            ReactionManager.gridManager.grid[enemyX, enemyY].setElement(Element.Base);
            Instantiate(Resources.Load("Ladder"), transform.position, transform.rotation);
        }
        
    }

    GameObject currRoom;
    Vector2 localPos;
    // Update is called once per frame
    void Update()
    {
        if (!currRoom) 
        {
            currRoom = FindObjectOfType<GridManager>().gameObject.transform.parent.gameObject;
        }
        localPos = currRoom.transform.InverseTransformPoint(transform.position);
        // Calculate enemy position -N
        enemyX = (int)(localPos.x + 5.5f);
        enemyY = (int)(localPos.y + 5.5f);

        if (this != null && currentState != null){
            currentState.UpdateState(this);
        }
        if(healthSystem != null && healthSystem.GetHealth() <= 0 && alive == true){
            if (this.gameObject.name.Contains("Zombie") || this.gameObject.name.Contains("Troll"))
                StartCoroutine(WaitAndSpawnLadder());
            Destroy(gameObject, (float)3);
            SwitchState(DeathState);
            if (turnLogic.GetIsPlayerTurn()) 
            {
                turnLogic.listOfInitative.Remove(this.initiative);
            }
            foreach (InitiativeText text in FindObjectsOfType<InitiativeText>()) 
            {
                text.CheckAlive(gameObject.GetInstanceID());
            }
            FindObjectOfType<DisplayInitiative>().Strikethrough(gameObject.GetInstanceID());
            turnLogic.CheckForRoomClear();
        }

        if (isSliding) 
        {
            SlideThisObjectAlongPath(slidingPath);
        }
        
        // if !isEnemyTurn, make sure we are in idle state. 

        // check if its enemy turn, if it is we will check if we are close enough to player. If not move. If we are in range attack.
        // if enemy turn

        // 50% chance to have shield up.
        if(turnLogic.GetIsPlayerTurn()){
            // get zombie health, if damage is taken during player turn it will heal
            if(gameObject.name.Contains("Zombie") && triedToBlock == false){
                zombieHealthStartOfPlayer = healthSystem.GetHealth();
                turnLogic = FindObjectOfType<TurnLogic>();
                if(turnLogic.turnCount % 2 == 0){
                    animator.SetBool("isBlocking", true);
                }                
                triedToBlock = true;
            }
        }

        // check if we are blocking, if so make zombie not take damage.
        if(gameObject.name.Contains("Zombie") && animator.GetBool("isBlocking") == true){
            int healAmmount =  zombieHealthStartOfPlayer - healthSystem.GetHealth();
            if(healthSystem.GetHealth() != 100){
                healthSystem.Heal(healAmmount);
            }
        }
        
        // play block sound if element hits enemy zombie, and play sound. 
        if(gameObject.name.Contains("Zombie") && elementOnEnemy == true && animator.GetBool("isBlocking") == true && playSound == false){
            SoundManagerScript.PlaySound("zombieblock");
            elementOnEnemy = false;
            playSound = true;
        }

        if(isEnemyTurn){
            if(gameObject.name.Contains("Zombie")){
                animator.SetBool("isBlocking", false);
                triedToBlock = false;
                playSound = false;
            }
            // check distance

            // get enemy pos now relitive -N
            Vector2 enemyPos = new Vector2(enemyX, enemyY);
            // Debug.Log("Eenemy pos " + enemyPos);
            //get player pos
            GameObject player = FindObjectOfType<PlayerMovement>().gameObject; // .transform.position; //gameObject.GetComponent<Player>().transform.position;
            // Position is now relitive -N
            Vector2 playerPos = new Vector2(player.GetComponent<PlayerMovement>().playerX, player.GetComponent<PlayerMovement>().playerY);
            // Debug.Log("Player position: " + playerPos);
            var distanceBetweenPlayerAndEnemy = Vector2.Distance(enemyPos,playerPos);
            // if deltaX positive, player is to the right, negative to the left
            var deltaXPLayerandEnemy = playerPos.x - enemyPos.x ;
            // if deltaY postiive, player is up, Negative he is down. 
            var deltaYPLayerandEnemy = playerPos.y - enemyPos.y;
            // Debug.Log("Delta x :" + deltaXPLayerandEnemy);

            // check if we need to flip sprite. 
            if(enemyX < playerPos.x && mySpriteRenderer != null) // && enemy.mySpriteRenderer.transform.localScale.x < 0 // x_random + x_value > 0
            {
                    // flip the sprite
                    if(gameObject.name.Contains("Zombie") || gameObject.name.Contains("Tree") || gameObject.name.Contains("Goblin") || gameObject.name.Contains("Mushroom") || gameObject.name.Contains("Troll") ){
                        mySpriteRenderer.flipX = false;
                    }else{
                        mySpriteRenderer.flipX = true;
                    }
            }
            if(enemyX > playerPos.x && mySpriteRenderer != null) //  && enemy.mySpriteRenderer.transform.localScale.x > 0
            {
                    // flip the sprite
                    if(gameObject.name.Contains("Zombie") || gameObject.name.Contains("Tree") || gameObject.name.Contains("Goblin") || gameObject.name.Contains("Mushroom") || gameObject.name.Contains("Troll")){
                        mySpriteRenderer.flipX = true;
                    }else{
                        mySpriteRenderer.flipX = false;
                    }
                    //  enemy.mySpriteRenderer.transform.localScale.x = -1;
            }

            //do attack or move.
            // check if melee enemy is within a 1 block radius of player. && will have to check which state we are in and if its enemy turn (not implemented yet)
            if((gameObject.name.Contains("Archer") || gameObject.name.Contains("Wizard") || gameObject.name.Contains("Zombie"))  && distanceBetweenPlayerAndEnemy <= visibilityRange && attackCounter == 0){
                Debug.Log("In attaack%");
                // play animation.
                animator.SetBool("isAttacking", true);
                // play archer sound
                if(gameObject.name.Contains("Archer")){
                    SoundManagerScript.PlaySound("arrowshot");
                }
                // play wizard sound
                if(gameObject.name.Contains("Wizard")){
                    SoundManagerScript.PlaySound("spellcast");
                }
                if(gameObject.name.Contains("Zombie")){
                    SoundManagerScript.PlaySound("zombieattack");
                }
                // sets attackCounter to 1 so we do not attack again and play the animation twice.
                attackCounter = 1;
                // damage gets dealt when we turn off the animation. 
                isEnemyTurn = false;
            }
            else if((gameObject.name.Contains("Warrior") || gameObject.name.Contains("Tree")  || gameObject.name.Contains("Goblin") || gameObject.name.Contains("Mushroom") || gameObject.name.Contains("Troll")  )  && distanceBetweenPlayerAndEnemy <= visibilityRange && attackCounter == 0){
                // play animation.
                animator.SetBool("isAttacking", true);
                // play sound clip
                if(gameObject.name.Contains("Warrior")){
                    SoundManagerScript.PlaySound("sword");
                }
                if(gameObject.name.Contains("Tree")){
                    SoundManagerScript.PlaySound("treeAttack");
                }
                if(gameObject.name.Contains("Goblin")){
                    SoundManagerScript.PlaySound("goblinAttack");
                }
                if(gameObject.name.Contains("Mushroom")){
                    SoundManagerScript.PlaySound("mushroomAttack");
                }
                if(gameObject.name.Contains("Troll")){
                    SoundManagerScript.PlaySound("trollAttack");
                }
                // sets attackCounter to 1 so we do not attack again and play the animation twice.
                attackCounter = 1;
                isEnemyTurn = false;
            }
            // if we get here, lets check if we are at 30% health or lower, if so lets retreat. 
            else if (healthSystem.GetHealth() <= 0.3 * healthSystem.getMaxHealth() && attackCounter == 0 && retreatCounter <= 1){
                Debug.Log("In retreat WHAT RETREAT COUNT ARE WE ON?");
                Debug.Log(retreatCounter);
                retreatCounter = retreatCounter + 1;
                if (isSliding) 
                {
                    SlideThisObjectAlongPath(slidingPath);
                }
                Tile tileUnderEnemy;
                tileUnderEnemy = ReactionManager.gridManager.getTile(enemyX, enemyY);
                while(canRetreat != 0){
                    //Debug.LogError("kill me");
                    foreach (Tile tile in tileUnderEnemy.neighbors) {
                        if(tile.getElement() == Element.Void){
                            continue;
                        }
                        Vector2 tilePosition = new Vector2(tile.gameObject.GetComponent<TilePathNode>().GetX(), tile.gameObject.GetComponent<TilePathNode>().GetY());
                        if(Vector2.Distance(tilePosition, playerPos) > distanceBetweenPlayerAndEnemy && tile.getElement() != Element.Void){
                            if(canRetreat == 1){
                                firstDestinationTile = tile;
                            }
                            // MoveEnemy(tilePosition.x, tilePosition.y);
                            canRetreat = canRetreat - 1;
                            if(canRetreat == 0){
                                Debug.Log("ding");
                                foreach(Tile finalDestinationTile in firstDestinationTile.neighbors) {
                                    if(finalDestinationTile.gameObjectAbove != null || finalDestinationTile.staticObjAbove != null){
                                        continue;
                                    }
                                    if(finalDestinationTile.getElement() == Element.Void){
                                        continue;
                                    }
                                    Vector2 finalDestPosition = new Vector2(finalDestinationTile.gameObject.GetComponent<TilePathNode>().GetX(), finalDestinationTile.gameObject.GetComponent<TilePathNode>().GetY());
                                    if(Vector2.Distance(finalDestPosition, playerPos) > Vector2.Distance(tilePosition, playerPos) && finalDestinationTile.getElement() != Element.Void){
                                        MoveEnemy(finalDestPosition.x, finalDestPosition.y);
                                    }
                                }
                                canRetreat = 0;
                            }
                        }
                    }
                    canRetreat = 0;
                }
                canRetreat = 1;
                isEnemyTurn = false;
                
            }
            // if we hit this we need to move closer to the player. 
            else if(distanceBetweenPlayerAndEnemy > 1.42){
                Debug.Log("In normal move%");
                if (isSliding) 
                {
                    SlideThisObjectAlongPath(slidingPath);
                }
                // reset the attack counter so we can attack again if enemy goes back in range. 
                attackCounter = 0;

                // find path before moving. 
                // Debug.Log((int)enemyPos.x + " " + (int)enemyPos.y + " " + (int)playerPos.x + " " + (int)playerPos.y);
                listOfTiles = pathfinding.FindPathWaypoints((int)enemyPos.x, (int)enemyPos.y, (int)playerPos.x, (int)playerPos.y);
                // lets strip off first tile, thats the player tile we do not want to land RIGHT ON the player, just next to him.
                if(listOfTiles == null){
                    return;
                }
                listOfTiles.RemoveAt(0);
                while(enemyMovementRemaining != 0){
                    // Debug.Log("START" + enemyMovementRemaining);
                    if(listOfTiles != null){
                        // null check if there is no path, it would crash. ex: enemy is stuck in a wall of void. 
                        if(listOfTiles != null){
                            int sizeOfList = listOfTiles.Count;
                            // check if we have 2 tiles left in list, if so just move to [1].
                            if(sizeOfList == 2){
                                // grabbing the 1 tile out of the 2, we do not want to go on top of the player. 
                                Transform destination = listOfTiles[sizeOfList - 1];
                                MoveEnemy((float)destination.gameObject.GetComponent<TilePathNode>().GetX(), (float)destination.gameObject.GetComponent<TilePathNode>().GetY());
                                enemyMovementRemaining = 0;
                                listOfTiles.RemoveAt(sizeOfList - 1);
                            }
                            else if(sizeOfList > 2){
                                // Grabing the 2nd to last tile from list, or we will grab sizeOfList - N, where N is the movement for the enemies. 
                                Transform destination = listOfTiles[sizeOfList - 1];
                                MoveEnemy((float)destination.gameObject.GetComponent<TilePathNode>().GetX(), (float)destination.gameObject.GetComponent<TilePathNode>().GetY());
                                enemyMovementRemaining = enemyMovementRemaining - 1;
                                listOfTiles.RemoveAt(sizeOfList - 1);
                            }
                        }
                    }
                }
                if(gameObject.name.Contains("Mushroom")){
                    enemyMovementRemaining = 5;
                }
                else if(gameObject.name.Contains("Goblin")){
                    enemyMovementRemaining = 4;
                }
                else if(gameObject.name.Contains("Warrior")){
                    enemyMovementRemaining = 2;
                }
                else if(gameObject.name.Contains("Troll")){
                    enemyMovementRemaining = 2;
                }
                else{
                    enemyMovementRemaining = 2;
                }
                isEnemyTurn = false;
            }


            if (attackCounter == 1){
                // Debug.Log("We attacked!");
                Invoke("TurnOffAnimation", 1);
                StartCoroutine(DamageDelay(player, enemyPos, playerPos));
                attackCounter = 0;
                // wait 1 second turn off animation. 
            }
            // reset to idle state.

        }
    }

    private void TurnOffAnimation()
    {
        animator.SetBool("isAttacking", false);
    }

    IEnumerator DamageDelay(GameObject player,Vector2 enemyPos, Vector2 playerPos) 
    {
        if(animator.GetBool("isAttacking") == true){
            Debug.Log(attackClipLength);
            yield return new WaitForSeconds(attackClipLength);
            int damageToDeal = 0;

            string temp;
            // remove clone tag on name
            if (gameObject.name.Contains("Clone"))
            {
                temp = gameObject.name.Substring(0, gameObject.name.Length - 7);
            }
            else
            {
                temp = gameObject.name;
            }


            switch (temp)
            {
            case "SkeletonArcher":
                damageToDeal = archerDamage;
                break;
            case "SkeletonWizard":
                damageToDeal = mageDamage;
                break;
            case "GreenSkeletonWizard":
                damageToDeal = greenMageDamage;
                break;
            case "SkeletonWarrior":
                damageToDeal = swordDamage;
                break;
            case "GreenSkeletonWarrior":
                damageToDeal = greenswordDamage;
                break;
            case "Fantasy Zombie":
                damageToDeal = bossDamage;
                break;
            case "Goblin":
                damageToDeal = goblinDamage;
                break;
            case "Mushroom":
                damageToDeal = mushroomDamage;
                break;
            case "evilTree":
                damageToDeal = treeDamage;
                break;
            case "Fantasy Troll":
                damageToDeal = trollDamage;
                break;
            }

            player.GetComponent<PlayerStats>().DealDamage(damageToDeal);

            if(gameObject.name.Contains("Troll")){
                 Vector2 diff = new Vector2(enemyPos.x -  playerPos.x,
                                                enemyPos.y - playerPos.y);
                    ReactionManager.pushGO(gameObject, -diff, 1, player);
            }
        }
    }

    public void SwitchState(EnemyBaseState state){
        currentState = state;
        state.EnterState(this);
    }

    public void DealDamage(int damage){
        if(gameObject.name.Contains("Zombie") && animator.GetBool("isBlocking") == true){
            return;
        }
        // deals damage via the health system. 
        healthSystem.Damage(damage);
        // does the hurt animation.
        animator.Play("Hurt", 0 ,0.5f);
        // play hurt sound.
        if(gameObject.name.Contains("Skeleton")){
            SoundManagerScript.PlaySound("skeletonhurt");
        }
        if(gameObject.name.Contains("Zombie")){
            SoundManagerScript.PlaySound("zombiehurt");
        }
        if(gameObject.name.Contains("Tree")){
            SoundManagerScript.PlaySound("treeHurt");
        }
        if(gameObject.name.Contains("Goblin")){
            SoundManagerScript.PlaySound("goblinHurt");
        }
        if(gameObject.name.Contains("Mushroom")){
            SoundManagerScript.PlaySound("mushroomHurt");
        }
        if(gameObject.name.Contains("Troll")){
            SoundManagerScript.PlaySound("trollHurt");
        }
        // animator.SetBool("TakeDamage", false);
    }

    public void MoveEnemy(float x, float y) 
    {
        // Find a path
        slidingPath = pathfinding.FindPathWaypoints((int)enemyX, (int)enemyY, (int)x, (int)y);
        // check if enemy is on tile. 
        if (slidingPath == null) 
        {
            return;
        }
    
        slidingPath.Reverse();
        isSliding = true;
        if (!isPlayingFootstep && gameObject.name.Contains("Skeleton")) 
        {
            SoundManagerScript.PlaySound("skeletonfootstep");
            isPlayingFootstep = true;
        }
        if (!isPlayingFootstep && gameObject.name.Contains("Zombie")) 
        {
            SoundManagerScript.PlaySound("zombiefootstep");
            isPlayingFootstep = true;
        }
        if (!isPlayingFootstep && gameObject.name.Contains("Tree")) 
        {
            SoundManagerScript.PlaySound("treeWalking");
            isPlayingFootstep = true;
        }
        if (!isPlayingFootstep && gameObject.name.Contains("Mushroom")) 
        {
            SoundManagerScript.PlaySound("mushroomWalk");
            isPlayingFootstep = true;
        }
        if (!isPlayingFootstep && gameObject.name.Contains("Goblin")) 
        {
            SoundManagerScript.PlaySound("goblinWalk");
            isPlayingFootstep = true;
        }
        if (!isPlayingFootstep && gameObject.name.Contains("Troll")) 
        {
            SoundManagerScript.PlaySound("trollWalk");
            isPlayingFootstep = true;
        }
        animator.SetBool("isMoving", true);
    }

    public void SlideThisObjectAlongPath(List<Transform> path)
    {
        
        if (waypointIndex <= path.Count - 1)
        {
            var targetPos = path[waypointIndex].position;
            var movementThisFrame = slideSpeed * Time.deltaTime;
            
            transform.position = Vector2.MoveTowards(transform.position, targetPos, movementThisFrame);
            if (transform.position == targetPos)
            {
                waypointIndex++;
            }
        }
        else
        {
            stopEnemyMovementAttack();
            // stopEnemyMovement();
        }
    }

    // paul here is the function u needed
    public void stopEnemyMovement(){
        waypointIndex = 0;
        isSliding = false;
        animator.SetBool("isMoving", false);
        isPlayingFootstep = false;
        if(gameObject.name.Contains("Skeleton")){
            SoundManagerScript.EndSound("skeletonfootstep");
        }
        if(gameObject.name.Contains("Zombie")){
            SoundManagerScript.EndSound("zombiefootstep");
        }
        if(gameObject.name.Contains("Tree")){
            SoundManagerScript.EndSound("treeWalking");
        }
        if(gameObject.name.Contains("Goblin")){
            SoundManagerScript.EndSound("goblinWalk");
        }
        if(gameObject.name.Contains("Mushroom")){
            SoundManagerScript.EndSound("mushroomWalk");
        }
        if(gameObject.name.Contains("Troll")){
            SoundManagerScript.EndSound("trollWalk");
        }

    }

    public void stopEnemyMovementAttack()
    {
        waypointIndex = 0;
        isSliding = false;
        animator.SetBool("isMoving", false);
        isPlayingFootstep = false;
        if (gameObject.name.Contains("Skeleton"))
        {
            SoundManagerScript.EndSound("skeletonfootstep");
        }
        if (gameObject.name.Contains("Zombie"))
        {
            SoundManagerScript.EndSound("zombiefootstep");
        }
        if (gameObject.name.Contains("Tree"))
        {
            SoundManagerScript.EndSound("treeWalking");
        }
        if (gameObject.name.Contains("Goblin"))
        {
            SoundManagerScript.EndSound("goblinWalk");
        }
        if (gameObject.name.Contains("Mushroom"))
        {
            SoundManagerScript.EndSound("mushroomWalk");
        }
        if (gameObject.name.Contains("Troll"))
        {
            SoundManagerScript.EndSound("trollWalk");
        }

        if (healthSystem.GetHealth() <= 0) 
        {
            return;
        }

        // get enemy pos now relitive -N
        Vector2 enemyPos = new Vector2(enemyX, enemyY);
        // Debug.Log("Eenemy pos " + enemyPos);
        //get player pos
        GameObject player = FindObjectOfType<PlayerMovement>().gameObject; // .transform.position; //gameObject.GetComponent<Player>().transform.position;
                                                                           // Position is now relitive -N
        Vector2 playerPos = new Vector2(player.GetComponent<PlayerMovement>().playerX, player.GetComponent<PlayerMovement>().playerY);
        // Debug.Log("Player position: " + playerPos);
        var distanceBetweenPlayerAndEnemy = Vector2.Distance(enemyPos, playerPos);

        //do attack or move.
        // check if melee enemy is within a 1 block radius of player. && will have to check which state we are in and if its enemy turn (not implemented yet)
        //Debug.LogError(distanceBetweenPlayerAndEnemy);
        if ((gameObject.name.Contains("Warrior") || gameObject.name.Contains("Tree") || gameObject.name.Contains("Goblin") || gameObject.name.Contains("Mushroom") || gameObject.name.Contains("Troll")) && distanceBetweenPlayerAndEnemy <= visibilityRange && attackCounter == 0)
        {
            //Debug.LogError("In attaack%");
            // play animation.
            animator.SetBool("isAttacking", true);
            // play sound clip
            if (gameObject.name.Contains("Warrior"))
            {
                SoundManagerScript.PlaySound("sword");
            }
            if (gameObject.name.Contains("Tree"))
            {
                SoundManagerScript.PlaySound("treeAttack");
            }
            if (gameObject.name.Contains("Goblin"))
            {
                SoundManagerScript.PlaySound("goblinAttack");
            }
            if (gameObject.name.Contains("Mushroom"))
            {
                SoundManagerScript.PlaySound("mushroomAttack");
            }
            if (gameObject.name.Contains("Troll"))
            {
                SoundManagerScript.PlaySound("trollAttack");
            }
            // sets attackCounter to 1 so we do not attack again and play the animation twice.
            attackCounter = 1;
            isEnemyTurn = false;
        }

        if (attackCounter == 1)
        {
            // Debug.Log("We attacked!");
            Invoke("TurnOffAnimation", 1);
            StartCoroutine(DamageDelay(player, enemyPos, playerPos));
            // wait 1 second turn off animation. 
        }

    }


    private void GridChanged(object sender, System.EventArgs e)
    {
        currRoom = FindObjectOfType<GridManager>().gameObject.transform.parent.gameObject;
    }

    private void OnDestroy()
    {
        if (FindObjectsOfType<EnemyStateManager>().Length == 0) 
        {
            if (FindObjectOfType<TurnLogic>())
            {
                FindObjectOfType<TurnLogic>().EndCombat();
            }
        }
    }
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        FindObjectOfType<GridManager>().grid[enemyX, enemyY].gameObject.GetComponent<TileClickable>().PointerDownLogic(eventData);
    }
}
