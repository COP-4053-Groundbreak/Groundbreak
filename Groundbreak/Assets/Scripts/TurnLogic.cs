using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnLogic : MonoBehaviour
{
    bool isPlayerTurn = false;
    // Determines what action the player is currently performing
    public bool isMovementPhase = true;
    public bool isThrowPhase = false;
    // Determines if combat is taking place
    public bool isCombatPhase = false;

    float dummyTurnTime = 3f;
    // Player movment and actions references
    PlayerMovement playerMovement;
    PlayerActions playerActions;

    // List of players and enemies
    List<GameObject> actorList = new List<GameObject>();
    List<int> listOfInitative = new List<int>();    



    // Buttons and canvass references 
    [SerializeField] Button endTurnButton;
    [SerializeField] Button moveButton;
    [SerializeField] Button throwTileButton;
    [SerializeField] GameObject battleCanvas;

    // List of all tiles for free roaming pathfinding
    Tile[] tiles;
    // Empty gameobject to instantiate under void tiles to give them colliders
    [SerializeField] GameObject empty;

    int turnCount = 0;

    DisplayInitiative displayInitiative;

    private void Start()
    {
        displayInitiative = FindObjectOfType<DisplayInitiative>();
        FindObjectOfType<FindNewGridManager>().OnGridChanged += GridChanged;
        // Start in movephase and grey out move button
        moveButton.interactable = false;
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerActions = FindObjectOfType<PlayerActions>();
        // grab all the enemy state managers in the room and add their gameobjects to the list
        EnemyStateManager[] enemyStateManagers = UnityEngine.Object.FindObjectsOfType<EnemyStateManager>();
        foreach (EnemyStateManager enemy in enemyStateManagers) 
        {
            actorList.Add(enemy.gameObject);
        }
        // add player gameobject to list
        actorList.Add(FindObjectOfType<PlayerMovement>().gameObject);
        // Possibly needed on room change
        StartCombat();
        // Start not in players turn, check for initative in coroutine 
        endTurnButton.interactable = false;
        StartCoroutine(TurnCycle());
    }

    // Called by end turn button
    public void EndTurnPressed() 
    {
        if (isPlayerTurn) 
        {
            Debug.Log("Player ended their turn");
            isPlayerTurn = false;

            // Disable End Turn Button
            endTurnButton.interactable = false;

            // Start turn cycle again
            StartCoroutine(TurnCycle());

        }
    }

    // Switch to move phase
    public void MovePressed()
    {
        moveButton.interactable = false;
        throwTileButton.interactable = true;

        isMovementPhase = true;
        isThrowPhase = false;
    }

    // Switch to throw / pick up phase
    public void ThrowPressed()
    {
        if (!throwTileButton) 
        {
            Debug.Log("HERE");
        }
        throwTileButton.interactable = false;
        moveButton.interactable = true;

        isMovementPhase = false;
        isThrowPhase = true;
    }

    IEnumerator TurnCycle() 
    {
        // add temp object to get into while loop without messy do while
        listOfInitative.Add(-1);
        // lets loop while we have initatives to take care of.
        while(listOfInitative.Count != 0){
            // Remove temp object
            listOfInitative.Remove(-1);
            // If we have seen every actors once and the list is empty, repopulate it
            if (listOfInitative.Count == 0)
            {
                // This is equivilent to starting a turn
                turnCount++;
                Debug.Log("Turn " + turnCount + " has started");
                
                for (int i = 0; i < actorList.Count; i++)
                {
                    // Adding the initiatives of enemies to a list. 
                    if (actorList[i].GetComponent<EnemyStateManager>())
                    {
                        listOfInitative.Add(actorList[i].GetComponent<EnemyStateManager>().initiative);
                    }
                    // Adding the initiative of Player to a list. 
                    else
                    {
                        listOfInitative.Add(actorList[i].GetComponent<PlayerStats>().GetInitiative());
                    }
                }

                // Upon a new turn starting, reduce the duration of existing effects
                Debug.Log("how many effects? " + ReactionManager.existingEffects.Count);
                if (ReactionManager.existingEffects.Count > 0)
                    for (int i = 0; i < ReactionManager.existingEffects.Count; i++){
                        if (ReactionManager.existingEffects[i] == null)
                            continue;

                        int effectID = ReactionManager.existingEffects[i].gameObject.GetInstanceID();
                        Debug.Log("About to reduce duration");
                        ReactionManager.reduceDuration(ReactionManager.existingEffects[i]);
                        // Once we remove something, we have to make sure that we don't accidentally
                        // skip something in the list
                        Debug.Log($"{effectID} vs {ReactionManager.existingEffects[i].gameObject.GetInstanceID()}");
                        if (effectID != ReactionManager.existingEffects[i].gameObject.GetInstanceID()){
                            i--;
                        }
                    }                 
            }
            
            // loop thru actors and see which has highest initative.
            Debug.Log("Count:" +listOfInitative.Count);
            for (int i = 0; i < actorList.Count; i++){
                // check initative.
                if(listOfInitative.Count == 0){
                    break;
                }
                if (actorList[i].GetComponent<EnemyStateManager>())
                {
                    if (actorList[i].GetComponent<EnemyStateManager>().initiative == listOfInitative.Max())
                    {
                        displayInitiative.SetTurn();
                        actorList[i].GetComponent<EnemyStateManager>().isEnemyTurn = true;
                        // lets wait for enemy to finish animation.
                        yield return new WaitForSeconds(dummyTurnTime);
                        // set enemy turn to false.
                        for (int j = 0; j < actorList.Count; j++)
                        {
                            // check which enemy is on. set to false.
                            if (actorList[j].GetComponent<EnemyStateManager>())
                            {
                                
                                if (actorList[j].GetComponent<EnemyStateManager>().isEnemyTurn == true)
                                {
                                    actorList[j].GetComponent<EnemyStateManager>().isEnemyTurn = false;
                                    // allows enemy to attack next turn.
                                }
                                actorList[j].GetComponent<EnemyStateManager>().attackCounter = 0;
                            }
                        }
                        // delete the max from list and keep processing until while loop is donezo.
                        if (listOfInitative.Count != 0)
                        {
                            listOfInitative.Remove(listOfInitative.Max());
                        }
                    }
                }
                else if (listOfInitative.Count() != 0)
                {
                    // Player has highest initiative, its their phase of the turn
                    if (actorList[i].GetComponent<PlayerStats>().GetInitiative() == listOfInitative.Max())
                    {
                        displayInitiative.SetTurn();
                        // Reset player's movement points for new turn
                        playerMovement.ResetMovement();
                        playerActions.ResetActions();
                        isPlayerTurn = true;

                        // Enable End Turn Button
                        endTurnButton.interactable = true;
                        playerMovement.PossiblyShowTile();

                        listOfInitative.Remove(listOfInitative.Max());

                        // Wait for player turn to finish
                        yield return new WaitWhile(() => isPlayerTurn);
                        // The key statement, we have 2 threads after this yield from calling the coroutine in end turn, kill this one
                        yield break;
                    }
                }
            }


        }

        if (listOfInitative.Count() == 0)
        {
            Debug.Log("Turn over");
            // Start the loop up again, for when we have enemy1, enemy2, player, enemy3
            // enemy1, enemy2, player go, then when we come back, the list just contains enemy3
            // So go again, we always wait on player turn so this works
            StartCoroutine(TurnCycle());
        }

    }

    // Starts combat in a room
    public void StartCombat()
    {
        battleCanvas.SetActive(true);
        isCombatPhase = true;
        
        // Probably not needed as we never start combat in the same room again but just incase
        DestroyVoidColliders();
        playerActions.gameObject.GetComponent<Animator>().SetBool("IsWalking", false);
        SoundManagerScript.EndSound("footstep");

        displayInitiative.SetList(actorList);
        actorList.Add(FindObjectOfType<PlayerMovement>().gameObject);
        

    }

    // Ends combat in a room and generates colliders for impassable terrain
    public void EndCombat()
    {
        battleCanvas.SetActive(false);
        isCombatPhase = false;
        CreateVoidColliders();

        // Destroy all effects
        ReactionManager.destroyAllEffects();
    }

    // Destroys all children of void tiles
    void DestroyVoidColliders() 
    {
        tiles = FindObjectsOfType<Tile>();
        GameObject[] allChildren = new GameObject[transform.childCount];
        int i = 0;

        foreach (Tile tile in tiles)
        {
            if (tile.getElement() == Element.Void)
            {
                i = 0;
                //Find all child obj and store to that array
                foreach (Transform child in transform)
                {
                    allChildren[i] = child.gameObject;
                    i += 1;
                }
                //Now destroy them
                foreach (GameObject child in allChildren)
                {
                    DestroyImmediate(child.gameObject);
                }
            }
        }




    }

    // Creates children for void tiles and gives them box colliders
    void CreateVoidColliders() 
    {
        tiles = FindObjectsOfType<Tile>();
        foreach (Tile tile in tiles)
        {
            if (tile.getElement() == Element.Void)
            {
                Transform child = Instantiate(empty.transform, tile.transform);
                child.gameObject.AddComponent<BoxCollider2D>();
            }
        }
    }

    public bool GetIsPlayerTurn() 
    {
        return isPlayerTurn;
    }

    private void GridChanged(object sender, System.EventArgs e)
    {
        //StopAllCoroutines();
        EnemyStateManager[] enemyStateManagers = UnityEngine.Object.FindObjectsOfType<EnemyStateManager>();
        actorList.Clear();
        foreach (EnemyStateManager enemy in enemyStateManagers)
        {
            actorList.Add(enemy.gameObject);
        }
        // add player gameobject to list
        
        actorList.Add(FindObjectOfType<PlayerMovement>().gameObject);
        StartCombat();
        
        //Debug.LogWarning("AAAAAAA");
        listOfInitative.Clear();
        StopAllCoroutines();
        endTurnButton.interactable = false;
        StartCoroutine(TurnCycle());
        
    }

    public void ToggleEndTurn(bool toggle) 
    {
        endTurnButton.interactable = toggle;
    }

}
