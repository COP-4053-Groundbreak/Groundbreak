using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
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

    // health stuff
    public int startHealth = 100;
    public Transform pfHealthBar;
    public HealthSystem healthSystem;
    public bool alive = true;

    public int movement = 2;
    public int armor;
    public int initiative;
    // integer, each block will be 1 unit or however we coded it. 
    public int visibilityRange = 7;
    // attack stats:
    // posible for enemy to miss, 0 miss, 1 attack. rename this to attackChance or something. 
    public bool canAttack;
    // how much damage the enemy will do if canAttack is 1.
    public int attackDamage;

    // stuff for pathfinding.
    public int width;
    public int height;

    public float period = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        // get sprite renderer
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        // health system stuff 
        healthSystem = new HealthSystem(startHealth);
        // healthSystem.Heal(110);
        // Debug.Log("Health: " + healthSystem.GetHealthPercent());

        // Transform healthBarTransform = Instantiate(pfHealthBar, new Vector3(0, 10), Quaternion.identity );
        Transform healthBarTransform = Instantiate(pfHealthBar, gameObject.transform);
        Vector3 healthBarLocalPosition = new Vector3(0, (float)1.50);
        healthBarTransform.localPosition = healthBarLocalPosition;

        HealthBar healthBar = healthBarTransform.GetComponent<HealthBar>();
        healthBar.Setup(healthSystem);

        // path finding stuff 
        width = FindObjectOfType<GridManager>().getWidth();
        height = FindObjectOfType<GridManager>().getHeight();
        pathfinding = FindObjectOfType<Pathfinding>();
        currentState = PatrollingState;
        // "this" is a refrence to the context of the enemy. 
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(this != null){
            currentState.UpdateState(this);
        }
        if(healthSystem.GetHealth() <= 0 && alive == true){
            Destroy(gameObject, (float)3);
            SwitchState(DeathState);
        }


        // eventually this will move into attack state, and i will check if it is a warrior skeleton. Maybe a switch case depending on which skeleton it is, and do the correct attack.
        // switch(type),  case warrior :code below: , case archer :new code for distance shooting(should be easy just multiple these values by 2 or however far it can shoot):
        // get enemy pos
        Vector2 enemyPos = gameObject.transform.position;
        // Debug.Log("Eenemy pos " + enemyPos);
        //get player pos
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // .transform.position; //gameObject.GetComponent<Player>().transform.position;
        Vector2 playerPos = player.transform.position;
        // Debug.Log("Player position: " + playerPos);
        var distanceBetweenPlayerAndEnemy = Vector2.Distance(enemyPos,playerPos);
        // Debug.Log("Distance between enemy and player: " + distanceBetweenPlayerAndEnemy);
        // check if melee enemy is within a 1 block radius of player. && will have to check which state we are in and if its enemy turn (not implemented yet)
        if(distanceBetweenPlayerAndEnemy <= 1.42 && attackCounter == 0){
            // play animation.
            animator.SetBool("isAttacking", true);
            // sets attackCounter to 1 so we do not attack again and play the animation twice.
            attackCounter = 1;

            // deal damage to player
            player.GetComponent<PlayerStats>().DealDamage(30);
            Debug.Log("Enemy Attacked the Player!!!");
        }
        else if(distanceBetweenPlayerAndEnemy > 1.42){
            // reset the attack counter so we can attack again if enemy goes back in range. 
            attackCounter = 0;
        }
        if(attackCounter == 1){
            // Debug.Log("We attacked!");
            Invoke("TurnOffAnimation", 1);
            // wait 1 second turn off animation. 
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
        // does the hurt animation.
        animator.Play("Hurt", 0 ,0.5f);
        // animator.SetBool("TakeDamage", true);
        // deals damage via the health system. 
        healthSystem.Damage(damage);
        // animator.SetBool("TakeDamage", false);
    }

    public void MoveEnemy(float x, float y) 
    {
        // Find a path
        slidingPath = pathfinding.FindPathWaypoints((int)gameObject.transform.position.x, (int)gameObject.transform.position.y, (int)x, (int)y);
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
            Debug.Log(newPos);
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
            waypointIndex = 0;
            isSliding = false;
            animator.SetBool("isMoving", false);
        }
    }
}
