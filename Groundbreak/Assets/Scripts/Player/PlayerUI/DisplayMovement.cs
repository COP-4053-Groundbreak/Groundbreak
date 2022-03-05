using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayMovement : MonoBehaviour
{
    public void DisplayMovementText(int movement)
    {
        this.GetComponent<TextMeshProUGUI>().text = movement.ToString();
    }
}
