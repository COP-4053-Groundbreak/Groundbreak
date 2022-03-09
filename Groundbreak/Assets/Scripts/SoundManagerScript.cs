using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    // make a sound of type AudioClip
    public static AudioClip swordSound;
    public static AudioClip arrowShotSound;
    public static AudioClip spellCastSound;
    public static AudioClip skeletonHurtSound;
    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        // loading sound from resources folder.
        swordSound = Resources.Load<AudioClip> ("sword");
        arrowShotSound = Resources.Load<AudioClip> ("arrowshot");
        spellCastSound = Resources.Load<AudioClip> ("spellcast");
        skeletonHurtSound = Resources.Load<AudioClip> ("skeletonhurt");

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
            case "arrowshot":
                audioSrc.PlayOneShot (arrowShotSound);
                break;
            case "spellcast":
                audioSrc.PlayOneShot (spellCastSound);
                break;
            case "skeletonhurt":
                audioSrc.PlayOneShot (skeletonHurtSound);
                break;
        }
    }
}
