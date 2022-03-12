using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // 10 movement speed is 1 tile, used so we can have say 2 tiles take 3 points of movement without needing float math
    [SerializeField] int movementSpeed;
    [SerializeField] public int currentMovementRemaining;
    Pathfinding pathfinding;

    // Gameobjects to instantiate the arrow that shows the path
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] GameObject pathHolder;

    // Speed the character slides at during turn based movement
    [SerializeField] float slideSpeed = 1f;
    List<Transform> slidingPath;

    // Bool to check when the player is moving during turn based movement
    public bool isSliding = false;
    int waypointIndex = 0;

    Animator playerAnimator;
    SpriteRenderer playerSpriteRenderer;
    Rigidbody2D playerRigidbody2D;

    // Speed the character slides at during non turn based movement
    [SerializeField] float freeMoveSpeed = 5f;
    TurnLogic turnLogic;
    // Bool to check when the player is moving during non turn based movement
    bool isFreemoving = false;

    DisplayMovement displayMovement;

    public int playerX;
    public int playerY;

    Tile[,] grid;

    [SerializeField] GameObject currentRoom;
    Vector3 localPos;

    bool isPlayingFootstep = false;

    [SerializeField] 
    // Start is called before the first frame update
    private void Start()
    {
        // Get references
        playerAnimator = GetComponent<Animator>();
        playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        movementSpeed = gameObject.GetComponent<PlayerStats>().GetMovementPerTurn();
        pathfinding = FindObjectOfType<Pathfinding>();
        turnLogic = FindObjectOfType<TurnLogic>();
        playerRigidbody2D = GetComponent<Rigidbody2D>();

        // update the distance to the player for each tile's internal value
        UpdateTilesAfterMove();
        
        // Set current movement null, update when its their turn
        currentMovementRemaining = 0;
        
        // Stops the player from rotating if they collide at non 90 degree angles
        playerRigidbody2D.freezeRotation = true;

        // Initilize movement text
        displayMovement = FindObjectOfType<DisplayMovement>();
        localPos = currentRoom.transform.InverseTransformPoint(transform.position);
        playerX = (int)(localPos.x + 5);
        playerY = (int)(localPos.y + 5);
        grid = FindObjectOfType<GridManager>().getGrid();
        FindObjectOfType<FindNewGridManager>().OnGridChanged += GridChanged;
        
        // Make sure tile under player knows player is above it
        StartCoroutine(waitAndNotifyTile());

    }

    private void Awake()
    {
            // Get references
            playerAnimator = GetComponent<Animator>();
            playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            movementSpeed = gameObject.GetComponent<PlayerStats>().GetMovementPerTurn();
            pathfinding = FindObjectOfType<Pathfinding>();
            turnLogic = FindObjectOfType<TurnLogic>();
            playerRigidbody2D = GetComponent<Rigidbody2D>();

            // update the distance to the player for each tile's internal value
            UpdateTilesAfterMove();

            // Set current movement null, update when its their turn
            currentMovementRemaining = 0;

            // Stops the player from rotating if they collide at non 90 degree angles
            playerRigidbody2D.freezeRotation = true;

            // Initilize movement text
            displayMovement = FindObjectOfType<DisplayMovement>();
            grid = FindObjectOfType<GridManager>().getGrid();
        
    }
    // Vector for free moving speed
    private Vector2 freeMovementDistance = Vector3.zero;
    private bool startFlag = false;
    private void Update()
    {
        
        localPos = currentRoom.transform.InverseTransformPoint(transform.position);
        playerX = (int)(localPos.x + 5.5);
        playerY = (int)(localPos.y + 5.5);
        if (!startFlag) 
        {
            UpdateTilesAfterMove();
            startFlag = true;
        }
        displayMovement.DisplayMovementText(currentMovementRemaining / 10);
        if (!turnLogic.isCombatPhase) 
        {
            // Get movement input and check if its 0
            float xInput = Input.GetAxisRaw("Horizontal");
            float yInput = Input.GetAxisRaw("Vertical");
            isFreemoving = (xInput != 0 || yInput != 0);
            freeMovementDistance = new Vector2(xInput, yInput);

            // Set animation for running
            if (isFreemoving)
            {
                playerAnimator.SetBool("IsWalking", true);
                if (!isPlayingFootstep) 
                {
                    SoundManagerScript.PlaySound("footstep");
                    isPlayingFootstep = true;
                }
            }
            else 
            {
                playerAnimator.SetBool("IsWalking", false);
                SoundManagerScript.EndSound("footstep");
                isPlayingFootstep = false;
            }

            // Set sprite flip for running
            if (xInput <= -0.1)
            {
                playerSpriteRenderer.flipX = true;
            }
            else if(xInput >= 0.1)
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
    // FixedUpdate for rigid body calculations
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
        slidingPath = pathfinding.FindPathWaypoints((int)(playerX), (int)(playerY), (int)x, (int)y);
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
            SoundManagerScript.PlaySound("footstep");
            turnLogic.ToggleEndTurn(false);
            isSliding = true;
            playerAnimator.SetBool("IsWalking", true);
            UpdateTilesAfterMove();
        }
        
    }

    // Takes a transform path, slides sprite along the path
    // Called every frame we are moving
    void SlideThisObjectAlongPath(List<Transform> path)
    {
        
        if (waypointIndex <= path.Count - 1)
        {
            var targetPos = path[waypointIndex].transform;
            // calculate how much movement we will do this frame
            var movementThisFrame = slideSpeed * Time.deltaTime;
            ClearLine();

            var newPos = (transform.position.x + targetPos.gameObject.transform.position.x) / (int)2;

            // Flip sprite if we move left
            if (newPos < transform.position.x)
            {
                playerSpriteRenderer.flipX = true;
            }
            else if (newPos > transform.position.x)
            {
                playerSpriteRenderer.flipX = false;
            }

            // Slide along path however much we do this frame
            transform.position = Vector2.MoveTowards(transform.position, targetPos.position, movementThisFrame);
            // When we get to the waypoint, move to the next
            if (transform.position  == targetPos.position)
            {
                waypointIndex++;
            }
        }
        else 
        {
            // Reset after we reach the end of the move
            endMove();
        }
    }
    public void endMove(){
        waypointIndex = 0;
        SoundManagerScript.EndSound("footstep");
        turnLogic.ToggleEndTurn(true);
        isSliding = false;
        playerAnimator.SetBool("IsWalking", false);
        UpdateTilesAfterMove();
        // Check if mouse is above tile and show line
        PossiblyShowTile();
    }

    // If the player is mousing over a tile when movement ends, show arrow to that tile
    public void PossiblyShowTile()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), -Vector2.up);
        if (hit) 
        {

            // Disabled for now as it causes nulls when trying to show tile 3/8
            //Debug.Log(hit.transform.position.x + " , " + hit.transform.position.y);
            if (!hit.rigidbody)
            {
                return;
            }
            if (hit.rigidbody.gameObject.GetComponent<TilePathNode>()) 
            {
                TilePathNode hitTile = hit.rigidbody.gameObject.GetComponent<TilePathNode>();
                ShowLine(hitTile.GetX(), hitTile.GetY());
            }
        }
    }

    // Draws line along the path the character takes
    public void ShowLine(float x, float y) 
    {
        
        // Dont show line if we are in throw phase, are moving, or not in combat
        if (!turnLogic) 
        {
            turnLogic = FindObjectOfType<TurnLogic>();
        }
        if (isSliding || turnLogic.isThrowPhase || !turnLogic.isCombatPhase || !turnLogic.GetIsPlayerTurn()) 
        {
            return;
        }
        // Calculate the path to take
        List<Transform> path = pathfinding.FindPathWaypoints((int)playerX, (int)playerY, (int)x, (int)y);
        
        if (path != null)
        {
            
            path.Add(grid[playerX, playerY].gameObject.transform);
            Transform endNode = path[0];
            // Instantiate all arrows along the path
            if (endNode.gameObject.GetComponent<TilePathNode>().fCost <= currentMovementRemaining)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    
                    // Gets orientation of the arrows correct
                    GameObject thisArrow;
                    TilePathNode thisPathNode = path[i].gameObject.GetComponent<TilePathNode>();
                    if (!path[i + 1].gameObject.GetComponent<TilePathNode>()) 
                    {
                        //Debug.Log(path[i + 1].gameObject);
                    }
                    //Debug.Log(thisPathNode.GetX() + " , " + path[i + 1].gameObject.GetComponent<TilePathNode>().GetX());
                    if (thisPathNode.GetX() > path[i + 1].gameObject.GetComponent<TilePathNode>().GetX())
                    {
                        thisArrow = Instantiate(arrowPrefab, thisPathNode.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)), pathHolder.transform);
                        thisArrow.transform.position += new Vector3(-0.5f, 0, 0);
                    }
                    else if (thisPathNode.GetX() < path[i + 1].gameObject.GetComponent<TilePathNode>().GetX())
                    {
                        thisArrow = Instantiate(arrowPrefab, thisPathNode.transform.position, Quaternion.Euler(new Vector3(0, 0, 180)), pathHolder.transform);
                        thisArrow.transform.position += new Vector3(0.5f, 0, 0);
                    }
                    else if (thisPathNode.GetY() > path[i + 1].gameObject.GetComponent<TilePathNode>().GetY())
                    {
                        thisArrow = Instantiate(arrowPrefab, thisPathNode.transform.position, Quaternion.Euler(new Vector3(0, 0, 90)), pathHolder.transform);
                        thisArrow.transform.position += new Vector3(0, -0.5f, 0);
                    }
                    else if (thisPathNode.GetY() < path[i + 1].gameObject.GetComponent<TilePathNode>().GetY())
                    {
                        thisArrow = Instantiate(arrowPrefab, thisPathNode.transform.position, Quaternion.Euler(new Vector3(0, 0, -90)), pathHolder.transform);
                        thisArrow.transform.position += new Vector3(0, 0.5f, 0);
                    }
                    else 
                    {
                        
                    }
                    
                }
            }
        }
    }

    // Destroy all the arrows
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
    public void UpdateTilesAfterMove()
    {
        TileClickable[] TileList = FindObjectsOfType<TileClickable>();
        foreach (TileClickable tile in TileList)
        {
            tile.updateDistanceToPlayer();
        }
    }

    private void GridChanged(object sender, System.EventArgs e)
    {
        currentRoom = FindObjectOfType<GridManager>().gameObject.transform.parent.gameObject;
        grid = FindObjectOfType<GridManager>().grid;
    }

    IEnumerator waitAndNotifyTile(){
        yield return new WaitForSeconds(1f);
        grid[playerX, playerY].gameObjectAbove = this.gameObject;
        yield break;
    }
}
