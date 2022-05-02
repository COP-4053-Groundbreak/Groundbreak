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
        dialogue.Add("Player: Whats that over there, A glove?");
        dialogue.Add("Use WSAD to move!");

        dialogue.Add("Glove: Finally, you have no idea how long ive been stuck down here!");
        dialogue.Add("Player: !?! You can talk????");
        dialogue.Add("Glove: You bet I can! Im no ordinary glove, i'm the legendary glove of -");
        dialogue.Add("Player: Don't care, can you help me get out?");
        dialogue.Add("Glove: I sure can! This dungeon is made up of tiles embued with elements and using my strength you can pick them up and throw them!");
        dialogue.Add("Player: How would that help?");
        dialogue.Add("Glove: When you throw a tile onto another tile, their elements will combine and create an effect. For example, if you threw a fire tile onto an air tile it would make a fireball!");
        dialogue.Add("Player: That sounds useful, lets go try it out!");

        dialogue.Add("Glove: Looks like there is a mean skeleton over there, since there is an enemy in this room, you are now in combat and cant move freely. You can see whos turn it is in the top right, you get to go first! ");
        dialogue.Add("The skeleton is far away now, so move closer to him by clicking on a tile within your movement range which you can see in the top left. A path of arrows will show the path you will take!");

        dialogue.Add("Glove: The skeleton is closer to you now and you can throw a tile at him. Right click on a tile to pick it up. You can also press [SPACE] to view the elemental symbols of each tile. ");
        dialogue.Add("Now, To make a fireball you need to combine a fire and air element. There are fire tiles around you and an air tile just below the skeleton. ");
        dialogue.Add("Since his element is water, designated by the water symbol by his health bar, any tile throw onto him will interact like it was thrown onto a water tile, not like the tile underneath them. ");
        dialogue.Add("To throw the tile, click the [throw tile] button on the bottom of the screen to switch to [throw tile] mode, and left click the air tile next to the skeleton.");

        dialogue.Add("Glove: Nice Throw! Now use the other tiles to finish him off. To view what reactions you can make and see your stats, press [TAB]!");

        dialogue.Add("Glove: Great you killed the skeleton! Now that the all enemies are dead, you will be able to free roam again! Go through the next door when you are ready.");

        dialogue.Add("Glove: Ooh this room has two skeletons! Remember, To view what reactions you can make press [TAB] To bring up the quick reference along with your stats. Good luck!");

        dialogue.Add("Glove: Congrats on clearing the room! You may have noticed the chest on the top right of the room, walk over to it and open it!");

        dialogue.Add("Glove: You now have one of each category of item and a health potion!. Press [i] to see them! Passive items like speed boots give you passive bonuses. Active items like the bow can be used in combat on a cooldown. ");
        dialogue.Add("Consumable items like the health potion can be used for a one time effect, Though be careful, if you use a potion like the speed potion outside of combat, it will run out before you reach the next room! ");

        dialogue.Add("Glove: Egads! A Coven of wizards! To use your bow, click on the bow icon on the bottom of the screen and left click the wizard closest to you. Great, now defeat the rest of the skeletons and finish this!");

        dialogue.Add("Glove:Good job getting this far, to get out of here you must go through two more floors, and the enemies ahead will do more damage than these ones did. But with your skill im sure you will prevail!");

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
        yield return new WaitForSeconds(1f);
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
        // Blocks player before getting glove
        if (index == 12) 
        {
            if (GameObject.Find("BLOCKER")) 
            {
                Destroy(GameObject.Find("BLOCKER"));
            }
        }
        // End tutorial
        if (index >= 26) 
        {
            SceneManager.LoadScene("Menu");
        }
        // Dialogue finished, wait for trigger
        if (index == 4 || index == 12 || index == 14 || index == 18 || index == 19 || index == 20 || index == 21 || index == 22 || index == 24 || index > 24) 
        {
            ChangeState(false);
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

    public int GetIndex() 
    {
        return index;
    }

}
