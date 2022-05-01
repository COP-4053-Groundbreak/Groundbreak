using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SeedButton : MonoBehaviour
{
    GameObject SeedObject;
    GameObject SeedInputField;
    string input;

    private void Start()
    {
        SeedInputField = GameObject.FindGameObjectWithTag("Seed");
        SeedObject = this.transform.parent.Find("SeedInput").gameObject;
    }

    public void SeedOnClick()
    {
        if (SeedInputField == null)
            Debug.LogWarning("Fuck");
        input = SeedInputField.GetComponent<TextMeshPro>().text;


        Debug.Log(input);

        if(input != null || input != "")
        {
            Seed.gameSeed = input;
            Seed.playerInput = true;
            //Show Confirmation Message
        }
        else
        {
            //Show Error Message
        }
        

        Debug.Log(input);
    }
}
