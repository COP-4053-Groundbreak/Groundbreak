using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    List<string> dialogue = new List<string>();
    int index = 0;
    bool roomCleared = false;
    [SerializeField] GameObject textGo;

    // Start is called before the first frame update
    void Start()
    {
        dialogue.Add("Player: Ugh my head");
        dialogue.Add("Player: Where the heck am I??");
        dialogue.Add("Player: I must have fallen into a dungeon.");
        dialogue.Add("Player: whats that over there, A glove?");
        dialogue.Add("Use WSAD to move!");

        dialogue.Add("Glove: Finally, you have no idea how long ive been stuck down here!");
        dialogue.Add("Player: !?! You can talk????");
        dialogue.Add("Glove: You bet I can! Im no ordinary glove, i'm the legendary glove of -");
        dialogue.Add("Player: Dont care");
        dialogue.Add("Glove: You seem to be stuck down here, but I can help get you out! This dungeon is made up of tiles embued with elements and using my strength you can pick them up and throw them!");
        dialogue.Add("Player: How would that help?");
        dialogue.Add("Glove: When you throw a tile onto another tile, their elements will combine and create an effect. For example, if you threw a fire tile onto an air tile it would make a fireball!");
        dialogue.Add("Player: That sounds useful, lets go try it out!");

        dialogue.Add("Glove: Looks like there is a mean skeleton over there, since there is an enemy in this room, you are now in combat and cant move freely. You can see whos turn it is in the top right, you get to go first! ");
        dialogue.Add("The skeleton is far away now, so move closer to him by clicking on a tile within your movement range which you can see in the top left. A path of arrows will show the path you will take!");

        dialogue.Add("Glove: The skeleton is closer to you now and you can throw a tile at him. Right click on a cyan air tile to pick it up. You can also press [SPACE] to view the elemental symbols of each tile. ");
        dialogue.Add("Now, To make a fireball you can either throw that tile onto a fire tile near the skeleton, or throw it directly onto the skeleton. ");
        dialogue.Add("Since his element is fire, designated by the fire symbol by his health bar, any tile throw onto him will interact like it was thrown onto a fire tile. ");
        dialogue.Add("To throw the tile, click the [throw tile] button on the bottom of the screen to switch to [throw tile] mode, and left click either the skeleton or a fire tile.");

        dialogue.Add("Glove: Great you killed the skeleton! Now that the all enemies are dead, you will be able to free roam again! Go through the next door when you are ready.");

        dialogue.Add("Glove: Ooh this room has two skeletons! To view what reactions you can make press [TAB] To bring up the quick reference along with your stats. Good luck!");

        dialogue.Add("Glove: Congrats on clearing the room! You may have noticed the chest on the top right of the room, walk over to it and open it!");

        dialogue.Add("Glove: You now have one of each category of item!. Press [i] to see them! Passive items like speed boots give you passive bonuses. Active items like the bow can be used in combat on a cooldown. ");
        dialogue.Add("Consumable items like the health potion can be used for a one time effect, Though be careful, if you use a potion like the speed potion outside of combat, it will run out before you reach the next room! ");

        dialogue.Add("Glove: Egads! A Coven of wizards! To use your bow, click on the bow icon on the bottom of the screen and left click the wizard closest to you. Great, now defeat the rest of the skeletons and finish this fight!");

        dialogue.Add("Glove:Good job getting this far, to get out of here you must go through two more floors. But with your skill im sure you will prevail!");

        StartCoroutine(WaitAndStart());
        ChangeState(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (roomCleared) 
        {
            IncrementIndex();
            roomCleared = false;
        }
    }

    IEnumerator WaitAndStart() 
    {
        yield return new WaitForSeconds(1.5f);
        ChangeState(true);
        DisplayText();
    }

    void DisplayText() 
    {
        if (index >= dialogue.Count)
        {
            ChangeState(false);
            return;
        }
        textGo.GetComponent<TextMeshProUGUI>().text = dialogue[index];
    }

    public void ClickedNext() 
    {
        if (index >= 25) 
        {
            SceneManager.LoadScene("Menu");
        }
        if (index == 4 || index == 12 || index == 14 || index == 18 || index == 19 || index == 20 || index == 21 || index == 23 || index > 23) 
        {
            ChangeState(false);
        }
        if (index == 14) 
        {
            StartCoroutine(WaitAndStart());
        }
        if (index >= dialogue.Count)
        {
            ChangeState(false);
        }
        else 
        {
            index++;
            DisplayText();
        }
    }

    void ChangeState(bool state) 
    {
        foreach (Transform child in transform) 
        {
            child.gameObject.SetActive(state);
        }
    }

    public void IncrementIndex() 
    {

        //index++;
        ChangeState(true);
        DisplayText();
    }

    public void RoomCleared() 
    {
        roomCleared = true;
    }

}
