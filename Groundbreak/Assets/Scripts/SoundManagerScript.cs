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
    public static AudioClip skeletonFootstepSound;
    // zombie sounds
    public static AudioClip zombieHurtSound;
    public static AudioClip zombieDeathSound;
    public static AudioClip zombieAttackSound;
    public static AudioClip zombieBlockSound;
    public static AudioClip zombieFootstepSound;

    // Player Sounds
    public static AudioClip footstepSound;
    public static AudioClip pickupTileSound;
    public static AudioClip throwTileSound;
    public static AudioClip playerTakeDamageSound;

    // Effect Sounds
    public static AudioClip magmaSound;
    public static AudioClip fireballSound; 
    public static AudioClip smokeSound;
    public static AudioClip sandstormSound;  
    public static AudioClip stormSound; 
    public static AudioClip mudSound; 

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
        skeletonFootstepSound = Resources.Load<AudioClip>("skeletonfootstep");
        zombieFootstepSound = Resources.Load<AudioClip>("zombiefootstep");
        pickupTileSound = Resources.Load<AudioClip>("pickuptile");
        throwTileSound = Resources.Load<AudioClip>("throwtile");
        playerTakeDamageSound = Resources.Load<AudioClip>("playerdamage");
        magmaSound = Resources.Load<AudioClip>("magma");
        fireballSound = Resources.Load<AudioClip>("fireball");
        smokeSound = Resources.Load<AudioClip>("smoke");
        sandstormSound = Resources.Load<AudioClip>("sandstorm");
        stormSound = Resources.Load<AudioClip>("storm");
        mudSound = Resources.Load<AudioClip>("mud"); 
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
        if (audioSrc == null) 
        {
            audioSrc = FindObjectOfType<SoundManagerScript>().gameObject.GetComponent<AudioSource>();
        }

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
            case "skeletonfootstep":
                audioSrc.clip = skeletonFootstepSound;
                audioSrc.loop = true;
                audioSrc.Play();
                break;
            case "zombiefootstep":
                audioSrc.clip = zombieFootstepSound;
                audioSrc.loop = true;
                audioSrc.Play();
                break;
            case "pickuptile":
                audioSrc.PlayOneShot(pickupTileSound);
                break;
            case "throwtile":
                audioSrc.PlayOneShot(throwTileSound);
                break;
            case "playerdamage":
                audioSrc.PlayOneShot(playerTakeDamageSound);
                break;
            case "magma":
                audioSrc.PlayOneShot(magmaSound);
                break;
            case "fireball":
                audioSrc.PlayOneShot(fireballSound);
                break;
            case "smoke":
                audioSrc.PlayOneShot(smokeSound);
                break;
            case "sandstorm":
                audioSrc.PlayOneShot(sandstormSound);
                break;
            case "storm":
                audioSrc.PlayOneShot(stormSound);
                break;
            case "mud":
                audioSrc.PlayOneShot(mudSound);
                break;
        }
    }
    public static void EndSound(string clip) 
    {
        audioSrc = FindObjectOfType<SoundManagerScript>().gameObject.GetComponent<AudioSource>();
        
        audioSrc.clip = null;
        audioSrc.loop = false;
    }
}
