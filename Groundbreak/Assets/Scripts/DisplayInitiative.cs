using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class DisplayInitiative : MonoBehaviour
{
    [SerializeField] TMP_FontAsset font;

    public void SetList(List<GameObject> actorList)
    {

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        int index = 0;
        List<GameObject> list = actorList;
        foreach (GameObject gameObject in list)
        {
            if (gameObject.GetComponent<PlayerMovement>() == null)
            {
                index++;
            }
        }

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

        list.RemoveAt(index);
        list = list.OrderBy(d => d.GetComponent<EnemyStateManager>().initiative).ToList();

        PlayerStats playerStats = FindObjectOfType<PlayerStats>();

        list.Reverse();
        bool flag = false;
        for (int j = 0; j < list.Count; j++)
        {
            if (playerStats.GetInitiative() > list.ElementAt(j).GetComponent<EnemyStateManager>().initiative)
            {
                flag = true;
                list.Insert(j, playerStats.gameObject);
                break;
            }
        }

        if (!flag) 
        {
            list.Add(playerStats.gameObject);
        }

        int i = 0;
        foreach (GameObject gameObject in list)
        {
            // Create text object for this enemy
            GameObject text = new GameObject();
            text.AddComponent<TextMeshProUGUI>();
            text.transform.SetParent(transform);
            text.transform.position = new Vector3(transform.position.x, transform.position.y - 40 * i);
            text.GetComponent<TextMeshProUGUI>().color = Color.black;
            text.GetComponent<TextMeshProUGUI>().fontSize = 24;
            text.GetComponent<TextMeshProUGUI>().font = font;
            string temp;
            // remove clone tag on name
            if (gameObject.name.Contains("Clone"))
            {
                temp = gameObject.name.Substring(0, gameObject.name.Length - 7);
            }
            else
            {
                temp = gameObject.name;
            }
            text.GetComponent<TextMeshProUGUI>().text = temp;
            text.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Left;


            i++;
        }
    }
}


