using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // 10 movement speed is 1 tile, used so we can have say 2 tiles take 3 points of movement without needing float math
    [SerializeField] int movementSpeed;
    Pathfinding pathfinding;

    [SerializeField] int currentMovementRemaining;

    [SerializeField] GameObject arrowPrefab;
    [SerializeField] GameObject pathHolder;

    [SerializeField] float slideSpeed = 1f;
    List<Transform> slidingPath;
    bool isSliding = false;
    int waypointIndex = 0;

    Animator playerAnimator;
    SpriteRenderer playerSpriteRenderer;

    [SerializeField] float freeMoveSpeed = 5f;
    TurnLogic turnLogic;
    Rigidbody2D playerRigidbody2D;
    bool isFreemoving = false;

    // Start is called before the first frame update
    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();

        movementSpeed = gameObject.GetComponent<PlayerStats>().GetMovementPerTurn();
        UpdateTilesAfterMove();
        pathfinding = FindObjectOfType<Pathfinding>();
        currentMovementRemaining = movementSpeed;

        turnLogic = FindObjectOfType<TurnLogic>();
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        playerRigidbody2D.freezeRotation = true;
    }

    private Vector2 freeMovementDistance = Vector3.zero;

    private void Update()
    {
        if (!turnLogic.isCombatPhase) 
        {
            float xInput = Input.GetAxisRaw("Horizontal");
            float yInput = Input.GetAxisRaw("Vertical");
            isFreemoving = (xInput != 0 || yInput != 0);
            freeMovementDistance = new Vector2(xInput, yInput);
            if (isFreemoving)
            {
                playerAnimator.SetBool("IsWalking", true);
            }
            else 
            {
                playerAnimator.SetBool("IsWalking", false);
            }

            if (xInput <= 0)
            {
                playerSpriteRenderer.flipX = true;
            }
            else 
            {
                playerSpriteRenderer.flipX = false;
            }
        }
        // Issliding set when we move a player, in update actually move the sprite every frame;
        else if (isSliding)
        {
            SlideThisObjectAlongPath(slidingPath);
        }
    }
    private void FixedUpdate()
    {
        if (!turnLogic.isCombatPhase)
        {
            playerRigidbody2D.MovePosition(playerRigidbody2D.position + freeMovementDistance * freeMoveSpeed * Time.fixedDeltaTime);
        }
        playerRigidbody2D.freezeRotation = true;
    }


    // Move player to a spot if they have the movement points remaining
    public void MovePlayer(int distance, float x, float y) 
    {
        if (isSliding || !turnLogic.isCombatPhase) 
        {
            return;
        }
        // Find a path
        slidingPath = pathfinding.FindPathWaypoints((int)gameObject.transform.position.x, (int)gameObject.transform.position.y, (int)x, (int)y);
        if (slidingPath == null) 
        {
            return;
        }
        
        TilePathNode endNode = slidingPath[0].gameObject.GetComponent<TilePathNode>();
        // If path cost is less than movement remaining, take it
        if (endNode.fCost <= currentMovementRemaining)
        {
            currentMovementRemaining -= endNode.fCost;
            slidingPath.Reverse();
            isSliding = true;
            playerAnimator.SetBool("IsWalking", true);
            UpdateTilesAfterMove();
        }
    }

    // Takes a transform path, slides sprite along the path
    void SlideThisObjectAlongPath(List<Transform> path)
    {
        
        if (waypointIndex <= path.Count - 1)
        {
            var targetPos = path[waypointIndex].position;
            var movementThisFrame = slideSpeed * Time.deltaTime;
            ClearLine();

            var newPos = (gameObject.transform.position.x + targetPos.x) / (int)2;

            if (newPos < transform.position.x)
            {
                playerSpriteRenderer.flipX = true;
            }
            else if (newPos > transform.position.x)
            {
                playerSpriteRenderer.flipX = false;
            }


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
            playerAnimator.SetBool("IsWalking", false);
            UpdateTilesAfterMove();
            PossiblyShowTile();
        }
    }

    // If the player is mousing over a tile when movement ends, show arrow to that tile
    public void PossiblyShowTile()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), -Vector2.up);
        if (hit) 
        {
            ShowLine(hit.transform.position.x, hit.transform.position.y);
        }
    }

    // Draws line along the path the character takes
    public void ShowLine(float x, float y) 
    {
        if (isSliding || turnLogic.isThrowPhase || !turnLogic.isCombatPhase) 
        {
            return;
        }
        List<Transform> path = pathfinding.FindPathWaypoints((int)gameObject.transform.position.x, (int)gameObject.transform.position.y, (int)x, (int)y);
        if (path != null)
        {
            path.Add(transform);
            Transform endNode = path[0];
            if (endNode.gameObject.GetComponent<TilePathNode>().fCost <= currentMovementRemaining)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    // Gets orientation of the arrows correct
                    GameObject thisArrow;
                    if (path[i].position.x > path[i + 1].position.x)
                    {
                        thisArrow = Instantiate(arrowPrefab, path[i].position, Quaternion.Euler(new Vector3(0, 0, 0)), pathHolder.transform);
                        thisArrow.transform.position += new Vector3(-0.5f, 0, 0);
                    }
                    else if (path[i].position.x < path[i + 1].position.x)
                    {
                        thisArrow = Instantiate(arrowPrefab, path[i].position, Quaternion.Euler(new Vector3(0, 0, 180)), pathHolder.transform);
                        thisArrow.transform.position += new Vector3(0.5f, 0, 0);
                    }
                    else if (path[i].position.y > path[i + 1].position.y)
                    {
                        thisArrow = Instantiate(arrowPrefab, path[i].position, Quaternion.Euler(new Vector3(0, 0, 90)), pathHolder.transform);
                        thisArrow.transform.position += new Vector3(0, -0.5f, 0);
                    }
                    else if (path[i].position.y < path[i + 1].position.y)
                    {
                        thisArrow = Instantiate(arrowPrefab, path[i].position, Quaternion.Euler(new Vector3(0, 0, -90)), pathHolder.transform);
                        thisArrow.transform.position += new Vector3(0, 0.5f, 0);
                    }
                    else 
                    {

                    }
                    
                }
            }
        }
    }

    public void ClearLine() 
    {
        foreach (Transform child in pathHolder.transform) 
        {
            Destroy(child.gameObject);
        }
    }

    public void ResetMovement()
    {
        currentMovementRemaining = movementSpeed;
    }

    // Calculates the distance from each tile to the player
    // Needed if we want to say highlight tiles that they can move to
    private void UpdateTilesAfterMove()
    {
        TileClickable[] TileList = FindObjectsOfType<TileClickable>();
        foreach (TileClickable tile in TileList)
        {
            tile.updateDistanceToPlayer();
        }
    }
}
