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
        const string chars = "ABCDEFGHIJKLMNOPRSTUVWXYZabcdefghijklmnopqrstupwxyz0123456789";
        if (!playerInput)
        {
            gameSeed = "";
            //gameSeed = "";
            for (int i = 0; i < 8; i++)
            {
                gameSeed += chars[Random.Range(0, chars.Length)];
            }
            //Debug.LogWarning("RANDOM INPUT:" + gameSeed);
        }
        else 
        {
            //Debug.LogWarning("PLAYER INPUT:" + gameSeed);
        }
        int num = 0;
        if (playerInput)
        {
            for (int i = 0; i < gameSeed.Length - 1; i++)
            {
                //Debug.LogError(gameSeed[i]);
                num += gameSeed[i];
            }
        }
        else 
        {
            for (int i = 0; i <= gameSeed.Length - 1; i++)
            {
                //Debug.LogError(gameSeed[i]);
                num += gameSeed[i];
            }
        }
        Debug.LogWarning(gameSeed);
        Debug.LogError(num);
        Random.InitState(num); 

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
