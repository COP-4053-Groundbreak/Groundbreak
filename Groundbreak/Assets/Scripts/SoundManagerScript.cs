using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    // make a sound of type AudioClip
    // skeleton sounds
    public static AudioClip swordSound;
    public static AudioClip arrowShotSound;
    public static AudioClip spellCastSound;
    public static AudioClip skeletonHurtSound;
    public static AudioClip skeletonDeathSound;
    // zombie sounds
    public static AudioClip zombieHurtSound;
    public static AudioClip zombieDeathSound;
    public static AudioClip zombieAttackSound;
    public static AudioClip zombieBlockSound;

    // Footstep Sound
    public static AudioClip footstepSound;

    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        // loading sound from resources folder.
        swordSound = Resources.Load<AudioClip> ("sword");
        arrowShotSound = Resources.Load<AudioClip> ("arrowshot");
        spellCastSound = Resources.Load<AudioClip> ("spellcast");
        skeletonHurtSound = Resources.Load<AudioClip> ("skeletonhurt");
        skeletonDeathSound = Resources.Load<AudioClip> ("skeletondeath");
        zombieHurtSound = Resources.Load<AudioClip> ("zombiehurt");
        zombieDeathSound = Resources.Load<AudioClip> ("zombiedeath");
        zombieAttackSound = Resources.Load<AudioClip> ("zombieattack");
        zombieBlockSound = Resources.Load<AudioClip> ("zombieblock");
        footstepSound = Resources.Load<AudioClip>("footstep");

        // getting the audioSource component.
        audioSrc = GetComponent<AudioSource> ();
        audioSrc.volume = 0.1f;
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
            case "skeletondeath":
                audioSrc.PlayOneShot (skeletonDeathSound);
                break;
            case "zombiehurt":
                audioSrc.PlayOneShot (zombieHurtSound);
                break;
            case "zombiedeath":
                audioSrc.PlayOneShot (zombieDeathSound);
                break;
            case "zombieattack":
                audioSrc.PlayOneShot (zombieAttackSound);
                break;
            case "zombieblock":
                audioSrc.PlayOneShot (zombieBlockSound);
                break;
            case "footstep":
                audioSrc.clip = footstepSound;
                audioSrc.loop = true;
                audioSrc.Play();
                break;
        }
    }
    public static void EndSound(string clip) 
    {
        switch (clip)
        {
            case "footstep":
                audioSrc.clip = null;
                audioSrc.loop = false;
                break;
        }
    }
}
