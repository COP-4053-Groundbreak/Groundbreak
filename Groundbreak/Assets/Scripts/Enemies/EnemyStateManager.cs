using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    // boolean for checking if its the enemies turn.
    public bool isEnemyTurn = false; 
    // sprite rednderer
    public SpriteRenderer mySpriteRenderer;
    // animator
    public Animator animator;
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

    // stuff for pathfinding.
    public int width;
    public int height;

    public float period = 0.0f;

    // Enemy position variables -N
    public int enemyX;
    public int enemyY;
    // Start is called before the first frame update
    void Start()
    {
        // set initative
/*        if(gameObject.name.Contains("Archer")){
            initiative = 4;
        }
        if(gameObject.name.Contains("Warrior")){
            initiative = 2;
        }
        if(gameObject.name.Contains("Wizard")){
            initiative = 6;
        }*/
        enemyMovementRemaining = 2;
        // get sprite renderer
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        // health system stuff 
        healthSystem = new HealthSystem(startHealth);
        // Health bar stuff
        // Transform healthBarTransform = Instantiate(pfHealthBar, new Vector3(0, 10), Quaternion.identity );
        Transform healthBarTransform = Instantiate(pfHealthBar, gameObject.transform);
        Vector3 healthBarLocalPosition = new Vector3(0, (float)1.50);
        healthBarTransform.localPosition = healthBarLocalPosition;
        HealthBar healthBar = healthBarTransform.GetComponent<HealthBar>();
        healthBar.Setup(healthSystem);

        // path finding stuff  COMMENTED FOR NOW, WE ARE GOING TO START IN IDLE STATE. 
        width = FindObjectOfType<GridManager>().getWidth();
        height = FindObjectOfType<GridManager>().getHeight();
        pathfinding = FindObjectOfType<Pathfinding>();
        currentState = IdleState;
        // "this" is a refrence to the context of the enemy. 
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate enemy position -N
        enemyX = (int)(transform.position.x + 5f);
        enemyY = (int)(transform.position.y + 5f);

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
        if(isEnemyTurn){
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
                    mySpriteRenderer.flipX = true;
            }
            if(enemyX > playerPos.x && mySpriteRenderer != null) //  && enemy.mySpriteRenderer.transform.localScale.x > 0
            {
                    // flip the sprite
                    mySpriteRenderer.flipX = false;
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
                // deal damage to player
                player.GetComponent<PlayerStats>().DealDamage(30);
                Debug.Log("Enemy Archer Attacked the Player!!!");
                isEnemyTurn = false;
            }
            else if((gameObject.name.Contains("Warrior") || gameObject.name.Contains("Zombie")) && distanceBetweenPlayerAndEnemy <= 1.42 && attackCounter == 0){
                // play animation.
                animator.SetBool("isAttacking", true);
                // play sound clip
                SoundManagerScript.PlaySound("sword");
                // sets attackCounter to 1 so we do not attack again and play the animation twice.
                attackCounter = 1;
                // deal damage to player
                player.GetComponent<PlayerStats>().DealDamage(30);
                Debug.Log("Enemy Attacked the Player!!!");
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
                Debug.Log((int)enemyPos.x + " " + (int)enemyPos.y + " " + (int)playerPos.x + " " + (int)playerPos.y);
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
                // wait 1 second turn off animation. 
            }
            // reset to idle state.

        }
    }

     private void TurnOffAnimation()
    {
        animator.SetBool("isAttacking", false);
    }

    public void SwitchState(EnemyBaseState state){
        currentState = state;
        state.EnterState(this);
    }

    public void DealDamage(int damage){
        // deals damage via the health system. 
        healthSystem.Damage(damage);
        // does the hurt animation.
        animator.Play("Hurt", 0 ,0.5f);
        // play hurt sound.
        if(gameObject.name.Contains("Skeleton")){
            SoundManagerScript.PlaySound("skeletonhurt");
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

        // if going right, flip.
        if(gameObject.transform.position.x + x > gameObject.transform.position.x && mySpriteRenderer != null) // && enemy.mySpriteRenderer.transform.localScale.x < 0 // x_random + x_value > 0
        {
                 // flip the sprite
                 mySpriteRenderer.flipX = true;
                // enemy.mySpriteRenderer.transform.localScale.x = 1;
        }
        // if going left flip to default. 
       if((int)gameObject.transform.position.x + x < gameObject.transform.position.x && mySpriteRenderer != null) //  && enemy.mySpriteRenderer.transform.localScale.x > 0
        {
                 // flip the sprite
                 mySpriteRenderer.flipX = false;
                //  enemy.mySpriteRenderer.transform.localScale.x = -1;
        }
        
        slidingPath.Reverse();
        isSliding = true;
        animator.SetBool("isMoving", true);
        // gameObject.transform.SetPositionAndRotation(new Vector3(x, y, 0), new Quaternion(0, 0, 0, 0));
    }

    public void SlideThisObjectAlongPath(List<Transform> path)
    {
        
        if (waypointIndex <= path.Count - 1)
        {
            var targetPos = path[waypointIndex].position;
            var movementThisFrame = slideSpeed * Time.deltaTime;
            var newPos = (gameObject.transform.position.x + targetPos.x) / (int)2;
            // Debug.Log(newPos);
            // Debug.Log("Game object Transform pos: " + gameObject.transform.position.x);
            // Debug.Log("new pos: " + newPos);
            if(newPos > transform.position.x){
                // Debug.Log("Going to the right");
                mySpriteRenderer.flipX = true;
            }
            else if (newPos < transform.position.x){
                // Debug.Log("Going to the left");
                mySpriteRenderer.flipX = false;
                // animator.SetBool("alive", false);
            }
            // if neither its standing still in the x plane


            transform.position = Vector2.MoveTowards(transform.position, targetPos, movementThisFrame);
            if (transform.position == targetPos)
            {
                waypointIndex++;
            }
        }
        else
        {
            stopEnemyMovement();
            // waypointIndex = 0;
            // isSliding = false;
            // animator.SetBool("isMoving", false);
        }
    }

    // paul here is the function u needed
    public void stopEnemyMovement(){
        waypointIndex = 0;
        isSliding = false;
        animator.SetBool("isMoving", false);
    }
}
