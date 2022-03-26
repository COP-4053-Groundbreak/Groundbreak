using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayMovement : MonoBehaviour
{
    public void DisplayMovementText(int movement)
    {
        if (FindObjectOfType<TurnLogic>().isCombatPhase == false)
        {
            this.GetComponent<TextMeshProUGUI>().text = "N / A";
        }
        else
        {
            this.GetComponent<TextMeshProUGUI>().text = movement.ToString();
        }
    }
}
