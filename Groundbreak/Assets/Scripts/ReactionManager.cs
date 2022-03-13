using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ReactionManager : MonoBehaviour
{
    public static GameObject effectPrefab;
    [SerializeField] static GridManager gridManager;
    public static Dictionary<int, Sprite> comboToVisual = new Dictionary<int, Sprite>();

    public static List<Effect> existingEffects;

    //  [SerializeField] public static bool elementOnEnemy;

    // Game balance variables for effects
    // MAGMA VARIABLES:
    [SerializeField] public static int MAGMA_DMG = 12;
    [SerializeField] public static int MAGMA_DUR = 2;

    // SANDSTORM VARIABLES:
    [SerializeField] public static int SANDSTORM_DMG = 5;
    [SerializeField] public static int SANDSTORM_DUR = 8;
    [SerializeField] public static int SANDSTORM_RANGE = 2;
    
    // STORM VARIABLES:
    [SerializeField] public static int STORM_RANGE = 2;
    [SerializeField] public static int STORM_DMG = 5;
    [SerializeField] public static int STORM_DUR = 8;
    
    // FIREBALL VARIABLES:
    [SerializeField] public static int FIREBALL_DMG = 30;
    [SerializeField] public static int FIREBALL_RANGE = 2;
    [SerializeField] public static int FIREBALL_DUR = 0;

    // SMOKE VARIABLES:
    [SerializeField] public static int SMOKE_RANGE_PLAYER_MOD = 1;
    [SerializeField] public static int SMOKE_RANGE_ENEMY_MOD = 2;
    [SerializeField] public static int SMOKE_DUR = 2;

    // MUD VARIABLES
    [SerializeField] public static int MUD_DUR = 2;
    [SerializeField] public static int MUD_DEBUFF = 1;

    // Damage from two units colliding from a push or pull
    [SerializeField] public static int CRASH_DMG = 5;
    // THROW DAMAGE
    [SerializeField] public static int THROW_DMG = 10;

    private void Start() {
        gridManager = FindObjectOfType<GridManager>();
        FindObjectOfType<FindNewGridManager>().OnGridChanged += GridChanged;

        existingEffects = new List<Effect>();
        effectPrefab = Resources.Load("Effect") as GameObject;
        if (effectPrefab == null)
            Debug.Log("WHY");
        
        // Add air reactions to table
        comboToVisual.Add((int)Element.Air + (int)Element.Earth, Resources.Load<Sprite>("Assets/Sprites/Effects/sandstorm.png"));
        comboToVisual.Add((int)Element.Air + (int)Element.Fire, Resources.Load<Sprite>("Assets/Sprites/Effects/spreadfire.png"));
        comboToVisual.Add((int)Element.Air + (int)Element.Water, Resources.Load<Sprite>("Assets/Sprites/Effects/storm.png"));

        // Add earth reactions to table
        comboToVisual.Add((int)Element.Earth + (int)Element.Water, Resources.Load<Sprite>("Assets/Sprites/Effects/mud.png"));
        comboToVisual.Add((int)Element.Earth + (int)Element.Fire, Resources.Load<Sprite>("Assets/Sprites/Effects/magma.png"));

        // Add fire reactions to table
        comboToVisual.Add((int)Element.Fire + (int)Element.Water, Resources.Load<Sprite>("Assets/Sprites/Effects/smoke.png"));
    }
    
    // Reactions will always occur between two different objects, each with an element
    // First tile is grabbed one, second tile is stationary one
    // Reaction created when two tiles overlap
    // Different types of tile on tile reactions:
    //  Non-Base element on Base element => place element on Non-Base tile
    //  Non-Base element on different Non-Base element => AoE element reaction
    //  Non-Base element on same Non-Base element => Surrounding tiles imbued with element
        
    public static Effect TileOnTile(Element thrownElem, Tile staticTile){
        Element staticElem = staticTile.getElement();

        // No reaction on void tiles (How are you even throwing these?)
        if (thrownElem == Element.Void)
            return null;
        // Throw element onto base or void => tile becomes of that element
        if (staticElem == Element.Base || staticElem == Element.Void){
            staticTile.setElement(thrownElem);
            return null;
        }

        // Tiles have same element
        // Consume the element of both tiles and make every tile around it the same element
        if (thrownElem == staticElem){
            Debug.Log("Spread the element!");
            // Non-Base on same Non-Base

            List<Tile> neighbors = staticTile.neighbors;
            Debug.Log($"Number of neighbors {neighbors.Count}");
            // Change the element of each neighbor
            foreach (Tile neighbor in neighbors){
                if (neighbor.myElement != Element.Void)
                    neighbor.setElement(thrownElem);
            }

            // Consume the element
            staticTile.setElement(Element.Base);

            return null;
        } 
        
        Debug.Log("Effect will happen!");
        // Two elements are interacting, create an effect!
        TilePathNode tp = staticTile.GetComponent<TilePathNode>();
        Vector2 pos = new Vector2(tp.GetX(), tp.GetY());
        return createEffect(thrownElem, staticElem, staticTile.transform.position);
    }
    // Makes an effect combining two elements at pos
    public static Effect createEffect(Element element1, Element element2, Vector2 instantiatePos){
        // Make sure position is on integer value
        instantiatePos = new Vector2((int)instantiatePos.x, (int)instantiatePos.y);
        //Debug.LogWarning("Absolute position of effect to be created is: " + instantiatePos);
        Vector2 gridPos = gridManager.transform.InverseTransformPoint(instantiatePos) + new Vector3(5,5,0);
        // Debug.LogWarning("Grid position of effect to be created is: " + gridPos);
        
        
        // Make sure there isn't an effect already here. If there is, delete it.
        for (int i = 0; i < existingEffects.Count; i++){
            Effect eff = existingEffects[i];
            TilePathNode tp = eff.tileUnderEffect.GetComponent<TilePathNode>();
            Vector2 effPos = new Vector2(tp.GetX(), tp.GetY());
            
            // There's already an element existing in this tile, create a new effect while
            // removing the previous one
            if (effPos == gridPos){
                Debug.Log("Removing effect");
                existingEffects.Remove(eff);
                Destroy(eff.gameObject);
            }
        }


        Effect newEffect = Instantiate(effectPrefab, instantiatePos, Quaternion.identity, gridManager.transform.parent).GetComponent<Effect>();
        existingEffects.Add(newEffect);
        newEffect.Initialize(element1, element2, gridPos);

        return newEffect;
    }
    public static void catchElement(Element thrownElem, GameObject thrownAt){
        Debug.Log($"In catch element! {thrownAt.tag} is catching");
        // Does the tile thrown at have an enemy above it?
        // If so, then thrownAt will become an enemy gameobject
        // If not, then just do the normal tile
        if (thrownAt.GetComponent<Tile>().gameObjectAbove != null)
            thrownAt = thrownAt.GetComponent<Tile>().gameObjectAbove;
        Debug.Log($"Upon further look, {thrownAt.name} is catching");
        if (thrownAt.tag == "Tile"){
            Debug.Log("Element was thrown at a tile!");
            thrownAt.GetComponent<Tile>().myEffect = TileOnTile(thrownElem, thrownAt.GetComponent<Tile>());
        } else if (thrownAt.tag == "Enemy"){
            Tile tileUnderEnemy;
            Debug.Log("Element was thrown at an enemy!");
            if(thrownAt.name.Contains("Zombie")){
                EnemyStateManager bossESM = thrownAt.GetComponent<EnemyStateManager>();
                bossESM.elementOnEnemy = true;
                if (bossESM.animator.GetBool("isBlocking")){
                    PlayerMovement playerMov = FindObjectOfType<PlayerMovement>();
                    tileUnderEnemy = gridManager.getTile(playerMov.playerX, playerMov.playerY);
                    tileUnderEnemy.myEffect = TileOnTile(thrownElem, gridManager.getTile(playerMov.playerX, playerMov.playerY));
                    dealDamageToChar(gridManager.getTile(playerMov.playerX, playerMov.playerY).gameObjectAbove, THROW_DMG);
                    return ;
                }
            }
            dealDamageToChar(gridManager.getTile(thrownAt.GetComponent<EnemyStateManager>().enemyX, thrownAt.GetComponent<EnemyStateManager>().enemyY).gameObjectAbove, THROW_DMG);
            tileUnderEnemy = gridManager.getTile(thrownAt.GetComponent<EnemyStateManager>().enemyX, thrownAt.GetComponent<EnemyStateManager>().enemyY);
            tileUnderEnemy.myEffect = TileOnEnemy(thrownElem, thrownAt);
        } else if (thrownAt.tag == "Ability"){
            // myEffect = myReactionManager.AbilityOnTile(other.gameObject.GetComponent<Ability>(), this);
        } else {
            Debug.Log("Not throwing at enemy or tile! Throwing at " + thrownAt.name);
            // no elemental reaction here I believe
        }
    }
    public static Effect TileOnEnemy(Element thrownElem, GameObject enemy){
        Debug.Log("Enemy catching element!");
        Element enemyElem = enemy.GetComponent<EnemyStateManager>().myElement;

        // No reaction on void tiles (How are you even throwing these?)
        if (thrownElem == Element.Void)
            return null;
        // Throw element onto base or void => No reaction (Maybe react with tile under?)
        if (enemyElem == Element.Base || enemyElem == Element.Void){ 
            return null;
        }

        EnemyStateManager esm = enemy.GetComponent<EnemyStateManager>();
        // Tile and enemy have same element
        // Do we consume enemy element? We do spread though
        if (thrownElem == enemyElem){
            Debug.Log("Spread the element!");
            // Non-Base on same Non-Base
            
            List<Tile> neighbors = gridManager.getTile((int) esm.enemyX, (int)esm.enemyY).neighbors;

            // Change the element of each neighbor
            foreach (Tile neighbor in neighbors){
                if (neighbor.myElement != Element.Void)
                    neighbor.setElement(thrownElem);
            }

            // Consume the element
            enemy.GetComponent<EnemyStateManager>().myElement = Element.Base;

            return null;
        } 
        
        Debug.Log("Effect will happen!");
        // Two elements are interacting, create an effect!
        return createEffect(thrownElem, enemyElem, enemy.transform.position);
    }
    public Effect EnemyOnTile(){
        return null;
    }
    public Effect AbilityOnTile(){
        return null;
    }
    public static int elementToIdx(Element a){
        switch(a) {
            case Element.Air:
                Debug.Log("Air index");
                return 0;
            case Element.Earth:
                return 1;
            case Element.Fire:
                return 2;
            case Element.Water:
                return 3;
            case Element.Base:
                return 4;
            default:
                return 5;
        }
    }
    public static void destroyAllEffects(){
        foreach (Effect eff in existingEffects){
            Destroy(eff.gameObject);
        }
        existingEffects = new List<Effect>();
    }
    public static void reduceDuration(Effect eff){
        eff.myDuration--;
        // Debug.Log("Reducing duration to " + eff.myDuration);
        if (eff.myDuration <= 0){
            Debug.Log("DEEEESTRUCTIOOOOOON");
            existingEffects.Remove(eff);
            Destroy(eff.gameObject);
        }
    }

    private static void dealDamageToChar(GameObject character, int damageAmount){
        if (character.gameObject.tag == "Player")
            character.gameObject.GetComponentInParent<PlayerStats>().DealDamage(damageAmount);
        else
            character.gameObject.GetComponent<EnemyStateManager>().DealDamage(damageAmount);
    }

    private void GridChanged(object sender, System.EventArgs e)
    {
        gridManager = FindObjectOfType<GridManager>();
    }
}
