using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public static string gameSeed = "Default";
    public static bool playerInput = false;
    public int currentSeed = 0;


    public void Awake()
    {
        const string chars = "ABCDEFGHIJKLMNOPRSTUVWXYZ0123456789";
        if (!playerInput){
            gameSeed = new string(Enumerable.Repeat(chars,8).Select(s => s[(int)(Random.value * s.Length)]).ToArray());
        }
        currentSeed = gameSeed.GetHashCode();
        Random.InitState(currentSeed);
    }
}
