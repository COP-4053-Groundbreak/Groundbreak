using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InitiativeText : MonoBehaviour
{
    public bool isAlive = true;
    public int ID;

    public void CheckAlive(int incomingID)
    {
        if (incomingID == ID) 
        {
            isAlive = false;
            GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
        }
    }
}
