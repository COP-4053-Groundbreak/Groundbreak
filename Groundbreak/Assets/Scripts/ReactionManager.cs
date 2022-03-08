using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionManager : MonoBehaviour
{
    public static GameObject effectPrefab;
    [SerializeField] static GridManager room;
    public static Dictionary<int, Sprite> comboToVisual = new Dictionary<int, Sprite>();

    

    private void Start() {
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
        GameObject a = Instantiate(effectPrefab, staticTile.transform.position, Quaternion.identity);
        a.GetComponent<Effect>().Initialize(thrownElem, staticElem);
        return a.GetComponent<Effect>();
    }

    public static void catchElement(Element thrownElem, GameObject thrownAt){
        Debug.Log("In catch element!");
        // Does the tile thrown at have an enemy above it?
        // If so, then thrownAt will become an enemy gameobject
        // If not, then just do the normal tile
        if (thrownAt.GetComponent<Tile>().gameObjectAbove != null)
            thrownAt = thrownAt.GetComponent<Tile>().gameObjectAbove;
        if (thrownAt.tag == "Tile"){
            Debug.Log("Element was thrown at a tile!");
            thrownAt.GetComponent<Tile>().myEffect = TileOnTile(thrownElem, thrownAt.GetComponent<Tile>());
        } else if (thrownAt.tag == "Enemy"){
            Debug.Log("Element was thrown at an enemy!");
            Tile tileUnderEnemy = room.getTile(thrownAt.transform.position.x, thrownAt.transform.position.y);
            tileUnderEnemy.myEffect = TileOnEnemy(thrownElem, thrownAt);
        } else if (thrownAt.tag == "Ability"){
            // myEffect = myReactionManager.AbilityOnTile(other.gameObject.GetComponent<Ability>(), this);
        } else {
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

        // Tile and enemy have same element
        // Do we consume enemy element? We do spread though
        if (thrownElem == enemyElem){
            Debug.Log("Spread the element!");
            // Non-Base on same Non-Base
            List<Tile> neighbors = room.getTile((int) enemy.transform.position.x, (int)enemy.transform.position.y).neighbors;

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
        GameObject a = Instantiate(effectPrefab, enemy.transform.position, Quaternion.identity);
        a.GetComponent<Effect>().Initialize(thrownElem, enemyElem);
        return a.GetComponent<Effect>();
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
}
