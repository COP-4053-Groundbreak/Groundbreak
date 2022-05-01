using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowSeed : MonoBehaviour
{
    void Update()
    {
        GetComponent<TextMeshProUGUI>().text = Seed.gameSeed;
    }
}
