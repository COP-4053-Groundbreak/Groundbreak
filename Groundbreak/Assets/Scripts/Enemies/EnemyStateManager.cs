using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
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

    // basic stats for enemy, figured i would add it here instead of making a seperate script. Will be easy to acess in other states by just doing enemy.[field].
    public int health = 5;
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
    // Start is called before the first frame update
    void Start()
    {
        width = FindObjectOfType<Pathfinding>().width;
        height = FindObjectOfType<Pathfinding>().height;
        pathfinding = FindObjectOfType<Pathfinding>();
        currentState = PatrollingState;
        // "this" is a refrence to the context of the enemy. 
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(EnemyBaseState state){
        currentState = state;
        state.EnterState(this);
    }

    public void MoveEnemy(float x, float y) 
    {
        // Find a path
        slidingPath = pathfinding.FindPathWaypoints((int)gameObject.transform.position.x, (int)gameObject.transform.position.y, (int)x, (int)y);
        if (slidingPath == null) 
        {
            return;
        }
        
        slidingPath.Reverse();
        isSliding = true;
        // gameObject.transform.SetPositionAndRotation(new Vector3(x, y, 0), new Quaternion(0, 0, 0, 0));
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
            waypointIndex = 0;
            isSliding = false;
        }
    }
}
