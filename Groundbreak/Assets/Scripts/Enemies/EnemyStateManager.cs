using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
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
    public static bool elementOnEnemy;
    // currentState will hold a reference to the active state in the state machine. 
    EnemyBaseState currentState;
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

    bool triedToBlock = false;
    bool playSound = false;

    // movement
    public List<Transform> listOfTiles;

    // health stuff
    public int startHealth = 100;
    public Transform pfHealthBar;
    public HealthSystem healthSystem;
    public bool alive = true;

    // enemy movement 
    public int enemyMovementRemaining;
    public int armor;
    public int initiative = 0;
    // integer, each block will be 1 unit or however we coded it. 
    public int visibilityRange = 7;
    // attack stats:
    // posible for enemy to miss, 0 miss, 1 attack. rename this to attackChance or something. 
    public bool canAttack;
    // how much damage the enemy will do if canAttack is 1.
    public int attackDamage;
    [SerializeField] public Element myElement;

    // turn logic
    TurnLogic turnLogic;


    // stuff for pathfinding.
    public int width;
    public int height;

    public float period = 0.0f;
    int randomBlockChance;

    // Enemy position variables -N
    public int enemyX;
    public int enemyY;
    // Start is called before the first frame update
    void Start()
    {
        turnLogic = FindObjectOfType<TurnLogic>();
        enemyMovementRemaining = 2;
        // get sprite renderer
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        // health system stuff 
        healthSystem = new HealthSystem(startHealth);
        // Health bar stuff
        // Transform healthBarTransform = Instantiate(pfHealthBar, new Vector3(0, 10), Quaternion.identity );
        Transform healthBarTransform = Instantiate(pfHealthBar, gameObject.transform);
        if(gameObject.name.Contains("Zombie")){
            Vector3 healthBarLocalPosition = new Vector3(0, (float)1.25);
            healthBarTransform.localPosition = healthBarLocalPosition;
            HealthBar healthBar = healthBarTransform.GetComponent<HealthBar>();
            healthBar.Setup(healthSystem);
        }
        else if(gameObject.name.Contains("Skeleton")){
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
        enemyX = (int)(localPos.x + 5f);
        enemyY = (int)(localPos.y + 5f);

        if (this != null && currentState != null){
            currentState.UpdateState(this);
        }
        if(healthSystem != null && healthSystem.GetHealth() <= 0 && alive == true){
            Destroy(gameObject, (float)3);
            SwitchState(DeathState);
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
                randomBlockChance = Random.Range(0, 3);
                if(randomBlockChance == 0){
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
            GameObject player = GameObject.FindGameObjectWithTag("Player"); // .transform.position; //gameObject.GetComponent<Player>().transform.position;
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
                    if(gameObject.name.Contains("Zombie")){
                        mySpriteRenderer.flipX = false;
                    }else{
                        mySpriteRenderer.flipX = true;
                    }
            }
            if(enemyX > playerPos.x && mySpriteRenderer != null) //  && enemy.mySpriteRenderer.transform.localScale.x > 0
            {
                    // flip the sprite
                    if(gameObject.name.Contains("Zombie")){
                        mySpriteRenderer.flipX = true;
                    }else{
                        mySpriteRenderer.flipX = false;
                    }
                    //  enemy.mySpriteRenderer.transform.localScale.x = -1;
            }

    
            //do attack or move.
            // check if melee enemy is within a 1 block radius of player. && will have to check which state we are in and if its enemy turn (not implemented yet)
            if((gameObject.name.Contains("Archer") || gameObject.name.Contains("Wizard"))  && distanceBetweenPlayerAndEnemy <= 2 && attackCounter == 0){
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
                // sets attackCounter to 1 so we do not attack again and play the animation twice.
                attackCounter = 1;
                // damage gets dealt when we turn off the animation. 
                isEnemyTurn = false;
            }
            else if((gameObject.name.Contains("Warrior") || gameObject.name.Contains("Zombie")) && distanceBetweenPlayerAndEnemy <= 1.42 && attackCounter == 0){
                // play animation.
                animator.SetBool("isAttacking", true);
                // play sound clip
                if(gameObject.name.Contains("Warrior")){
                    SoundManagerScript.PlaySound("sword");
                }
                if(gameObject.name.Contains("Zombie")){
                    SoundManagerScript.PlaySound("zombieattack");
                }
                // sets attackCounter to 1 so we do not attack again and play the animation twice.
                attackCounter = 1;
                isEnemyTurn = false;
            }
            // if we hit this we need to move closer to the player. 
            else if(distanceBetweenPlayerAndEnemy > 1.42){
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
                listOfTiles.RemoveAt(0);
                while(enemyMovementRemaining != 0){
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
                enemyMovementRemaining = 2;
                isEnemyTurn = false;
            }
            if(attackCounter == 1){
                // Debug.Log("We attacked!");
                Invoke("TurnOffAnimation", 1);
                StartCoroutine(DamageDelay(player));
                // wait 1 second turn off animation. 
            }
            // reset to idle state.

        }
    }

    private void TurnOffAnimation()
    {
        animator.SetBool("isAttacking", false);
    }

    IEnumerator DamageDelay(GameObject player) 
    {
        yield return new WaitForSeconds(2f);
        player.GetComponent<PlayerStats>().DealDamage(20);
    }

    public void SwitchState(EnemyBaseState state){
        currentState = state;
        state.EnterState(this);
    }

    public void DealDamage(int damage){
        if(animator.GetBool("isBlocking") == true && gameObject.name.Contains("Zombie")){
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
            stopEnemyMovement();
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
    }

    private void GridChanged(object sender, System.EventArgs e)
    {
        currRoom = FindObjectOfType<GridManager>().gameObject.transform.parent.gameObject;
    }
}
