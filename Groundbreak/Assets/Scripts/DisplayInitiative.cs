using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class DisplayInitiative : MonoBehaviour
{
    private void Start()
    {
        FindObjectOfType<FindNewGridManager>().OnGridChanged += GridChanged;
    }
    public void SetList(List<GameObject> actorList) 
    {
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

        int i = 0;
        foreach (GameObject gameObject in actorList) 
        {
            // Create text object for this enemy
            GameObject text = new GameObject();
            text.AddComponent<TextMeshProUGUI>();
            text.GetComponent<TextMeshProUGUI>().color = Color.black;
            text.GetComponent<TextMeshProUGUI>().fontSize = 24;
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
            Instantiate(text, new Vector3(transform.position.x - 40, transform.position.y - 30 * i), transform.rotation, transform);
            i++;
        }
    }

    // Clear list on room change
    private void GridChanged(object sender, System.EventArgs e)
    {
        foreach (Transform child in transform) 
        {
            Destroy(child.gameObject);
        }
    }
}
