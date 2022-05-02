using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Seed : MonoBehaviour
{
    private string dump;
    public static string gameSeed;
    public static bool playerInput = false;
    public int currentSeed = 0;
    public string maybe;


    public void Awake()
    {
        Debug.LogWarning(gameSeed);
        //Debug.LogWarning(playerInput);

        if (!playerInput)
        {
            int tempSeed = (int)System.DateTime.Now.Ticks;
            gameSeed = tempSeed.ToString();
            currentSeed = tempSeed;

            Debug.LogWarning(gameSeed);
            Debug.LogWarning(currentSeed);
            Random.InitState(tempSeed);
        }
        else
        {
            int tempSeed;

            if (isNumeric(gameSeed))
            {
                Debug.LogWarning("Seed is Numeric");
                tempSeed = System.Int32.Parse(gameSeed);
            }
            else
            {
                tempSeed = gameSeed.GetHashCode();
            }
            currentSeed = tempSeed;

            Debug.LogWarning(gameSeed);
            Debug.LogWarning(currentSeed);
            Random.InitState(tempSeed);
        }
    }

    public static bool isNumeric(string s)
    {
        return int.TryParse(s, out int n);
    }
}

/*
        const string chars = "ABCDEFGHIJKLMNOPRSTUVWXYZabcdefghijklmnopqrstupwxyz0123456789";
        if (!playerInput){
            //gameSeed = "";
            for (int i = 0; i < 8; i++)
            {
                gameSeed += chars[Random.Range(0, chars.Length)];
            }
        }
        */
