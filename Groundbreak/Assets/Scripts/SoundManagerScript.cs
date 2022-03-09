using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip swordSound;
    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        // loading sound from resources folder.
        swordSound = Resources.Load<AudioClip> ("sword");

        // getting the audioSource component.
        audioSrc = GetComponent<AudioSource> ();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound (string clip){
        // switch statment to see which clip to play.
        switch(clip){
            case "sword":
                audioSrc.PlayOneShot (swordSound);
                break;
        }
    }
}
