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
        input = SeedInputField.GetComponent<TextMeshProUGUI>().text;

        if(input != null || input != "")
        {
            Debug.LogError(input + "Was Confirmed as the seed!");

            Seed.gameSeed = input;
            Seed.playerInput = true;

            Debug.LogWarning(Seed.gameSeed.GetHashCode() + " - Maybe");

            //Show Confirmation Message
            GameObject Error = SeedInputField.transform.parent.transform.parent.transform.parent.Find("ErrorMessage").gameObject;
            Error.SetActive(true);
            Error.GetComponent<TextMeshProUGUI>().text = "Seed Was Entered!";
            
        }
        else
        {
            //Show Error Message
            GameObject Error = SeedInputField.transform.parent.Find("ErrorMessage").gameObject;
            Error.SetActive(true);
            Error.GetComponent<TextMeshProUGUI>().text = "Invalid Seed!";
            
        }
        

        Debug.Log(input);
    }
}
