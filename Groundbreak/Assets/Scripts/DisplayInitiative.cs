using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class DisplayInitiative : MonoBehaviour
{
    [SerializeField] GameObject template;
    int numRepeated = 0;
    List<GameObject> list;

    private void FixedUpdate()
    {
        FindObjectOfType<FindNewGridManager>().OnGridChanged += GridChanged;
    }
    public void SetList(List<GameObject> actorList)
    {

        // Clear old list
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Find index of player
        int index = 0;
        list = actorList;
        foreach (GameObject gameObject in list)
        {
            if (gameObject.GetComponent<PlayerMovement>() == null)
            {
                index++;
            }
        }

        // Calculate initative list
        List<int> listOfInitative = new List<int>();
        for (int j = 0; j < actorList.Count; j++)
        {
            // Adding the initiatives of enemies to a list. 
            if (actorList[j].GetComponent<EnemyStateManager>())
            {
                listOfInitative.Add(actorList[j].GetComponent<EnemyStateManager>().initiative);
            }
            // Adding the initiative of Player to a list. 
            else
            {
                listOfInitative.Add(actorList[j].GetComponent<PlayerStats>().GetInitiative());
            }
        }

        // Remove player and sort
        list.RemoveAt(index);
        list = list.OrderBy(d => d.GetComponent<EnemyStateManager>().initiative).ToList();

        PlayerStats playerStats = FindObjectOfType<PlayerStats>();

        list.Reverse();
        bool flag = false;
        // Insert player back into list
        for (int j = 0; j < list.Count; j++)
        {
            if (playerStats.GetInitiative() > list.ElementAt(j).GetComponent<EnemyStateManager>().initiative)
            {
                flag = true;
                list.Insert(j, playerStats.gameObject);
                break;
            }
        }
        // If player has lowest initiative
        if (!flag)
        {
            list.Add(playerStats.gameObject);
        }

        // Create text
        int i = 0;
        foreach (GameObject subObject in list)
        {
            // Create text object for this enemy
            GameObject text = Instantiate(template);
            text.transform.SetParent(transform);

            float depth = gameObject.transform.lossyScale.z;
            float width = gameObject.transform.lossyScale.x;
            float height = gameObject.transform.lossyScale.y;

            Vector3 lowerLeftPoint = Camera.main.WorldToScreenPoint(new Vector3(gameObject.transform.position.x - width / 2, gameObject.transform.position.y - height / 2, gameObject.transform.position.z - depth / 2));
            Vector3 upperRightPoint = Camera.main.WorldToScreenPoint(new Vector3(gameObject.transform.position.x + width / 2, gameObject.transform.position.y + height / 2, gameObject.transform.position.z - depth / 2));

            float yPixelDistance = Mathf.Abs(lowerLeftPoint.y - upperRightPoint.y);

            text.transform.position = new Vector3(transform.position.x, transform.position.y - yPixelDistance * i);
            text.transform.localScale = new Vector3(1, 1, 1);
            text.name = subObject.name;
            string temp;
            // remove clone tag on name
            if (subObject.name.Contains("Clone"))
            {
                temp = subObject.name.Substring(0, subObject.name.Length - 7);
            }
            else
            {
                temp = subObject.name;
            }
            text.GetComponent<TextMeshProUGUI>().text = temp;
            text.GetComponent<InitiativeText>().ID = subObject.GetInstanceID();
            i++;
        }
        StartDisplay();
    }

    public void SetTurn() 
    {
        // Reset color
        foreach (Transform child in transform) 
        {
            child.gameObject.GetComponent<TextMeshProUGUI>().color = Color.black;

        }


        if (numRepeated >= transform.childCount) 
        {
            numRepeated = 0;
        }

        while (!transform.GetChild(numRepeated).GetComponent<InitiativeText>().isAlive) 
        {
            
            numRepeated++;
            if (numRepeated >= transform.childCount)
            {
                numRepeated = 0;
            }
        }


        if (numRepeated < list.Count)
        {
            transform.GetChild(numRepeated).GetComponent<TextMeshProUGUI>().color = Color.red;
        }
        else 
        {
            numRepeated = 0;
            transform.GetChild(numRepeated).GetComponent<TextMeshProUGUI>().color = Color.red;
        }
        numRepeated++;
    }


    private void GridChanged(object sender, System.EventArgs e)
    {
        numRepeated = 1;
        StartDisplay();
    }

    public void StartDisplay() 
    {
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.red;
    }

    public void Strikethrough(int instanceId) 
    {
        foreach (Transform child in transform) 
        {
            if (child.gameObject.GetComponent<InitiativeText>().ID == instanceId) 
            {
                child.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
            }
        }
    }


    public void ResetRepeat() 
    {
        numRepeated = 1;
    }

}


