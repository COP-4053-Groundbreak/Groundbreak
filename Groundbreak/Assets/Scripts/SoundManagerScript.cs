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
    public static AudioClip playerSword;
    public static AudioClip playerBow;
    public static AudioClip playerTeleport;

    // Effect Sounds
    public static AudioClip magmaSound;
    public static AudioClip fireballSound; 
    public static AudioClip smokeSound;
    public static AudioClip sandstormSound;  
    public static AudioClip stormSound; 
    public static AudioClip mudSound; 

    // Tree sounds
    public static AudioClip treeWalkSound;
    public static AudioClip treeHurtSound;
    public static AudioClip treeAttackSound;
    public static AudioClip treeDeadSound;

    // Mushroom Sounds
    public static AudioClip mushroomWalkSound;
    public static AudioClip mushroomHurtSound;
    public static AudioClip mushroomAttackSound;
    public static AudioClip mushroomDeadSound;

    // goblin 
    public static AudioClip goblinWalkSound;
    public static AudioClip goblinHurtSound;
    public static AudioClip goblinDeadSound;
    public static AudioClip goblinAttackSound;

    // troll 2nd boss
    public static AudioClip trollWalkSound;
    public static AudioClip trollHurtSound;
    public static AudioClip trollAttackSound;
    public static AudioClip trollDeadSound;


    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        // loading sound from resources folder.
        
        // zombie boss sound
        zombieHurtSound = Resources.Load<AudioClip> ("zombiehurt");
        zombieDeathSound = Resources.Load<AudioClip> ("zombiedeath");
        zombieAttackSound = Resources.Load<AudioClip> ("zombieattack");
        zombieBlockSound = Resources.Load<AudioClip> ("zombieblock");
        zombieFootstepSound = Resources.Load<AudioClip>("zombiefootstep");

        // skeleton
        skeletonFootstepSound = Resources.Load<AudioClip>("skeletonfootstep");
        swordSound = Resources.Load<AudioClip> ("sword");
        arrowShotSound = Resources.Load<AudioClip> ("arrowshot");
        spellCastSound = Resources.Load<AudioClip> ("spellcast");
        skeletonHurtSound = Resources.Load<AudioClip> ("skeletonhurt");
        skeletonDeathSound = Resources.Load<AudioClip> ("skeletondeath");

        // tree sounds
        treeWalkSound = Resources.Load<AudioClip> ("treeWalking");
        treeHurtSound = Resources.Load<AudioClip> ("treeHurt");
        treeAttackSound = Resources.Load<AudioClip> ("treeAttack");
        treeDeadSound = Resources.Load<AudioClip> ("treeDead");

        // goblin
        goblinWalkSound = Resources.Load<AudioClip> ("goblinWalk");
        goblinHurtSound = Resources.Load<AudioClip> ("goblinHurt");
        goblinDeadSound = Resources.Load<AudioClip> ("goblinDead");
        goblinAttackSound = Resources.Load<AudioClip> ("goblinAttack");

        // troll
        trollWalkSound = Resources.Load<AudioClip> ("trollWalk");
        trollHurtSound = Resources.Load<AudioClip> ("trollHurt");
        trollAttackSound = Resources.Load<AudioClip> ("trollAttack");
        trollDeadSound = Resources.Load<AudioClip> ("trollDead");

        // player
        pickupTileSound = Resources.Load<AudioClip>("pickuptile");
        throwTileSound = Resources.Load<AudioClip>("throwtile");
        playerTakeDamageSound = Resources.Load<AudioClip>("playerdamage");
        footstepSound = Resources.Load<AudioClip>("footstep");
        playerSword = Resources.Load<AudioClip>("PlayerSword");
        playerBow = Resources.Load<AudioClip>("PlayerBow");
        playerTeleport = Resources.Load<AudioClip>("teleport");

        // mushroom
        mushroomAttackSound = Resources.Load<AudioClip>("mushroomPunch");
        mushroomHurtSound = Resources.Load<AudioClip>("mushroomHurt");
        mushroomDeadSound = Resources.Load<AudioClip>("mushroomDead");
        mushroomWalkSound = Resources.Load<AudioClip>("mushroomWalk");

        // effects
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
            case "treeWalking":
                audioSrc.clip = treeWalkSound;
                audioSrc.loop = true;
                audioSrc.Play();
                break;
            case "treeHurt":
                audioSrc.PlayOneShot (treeHurtSound);
                break;
            case "treeAttack":
                audioSrc.PlayOneShot (treeAttackSound);
                break;
            case "treeDead":
                audioSrc.PlayOneShot (treeDeadSound);
                break;
            case "mushroomHurt":
                audioSrc.PlayOneShot (mushroomHurtSound);
                break;
            case "mushroomDead":
                audioSrc.PlayOneShot (mushroomDeadSound);
                break;
            case "mushroomAttack":
                audioSrc.PlayOneShot (mushroomAttackSound);
                break;
            case "mushroomWalk":
                audioSrc.clip = mushroomWalkSound;
                audioSrc.loop = true;
                audioSrc.Play();
                break;
            case "goblinWalk":
                audioSrc.clip = goblinWalkSound;
                audioSrc.loop = true;
                audioSrc.Play();
                break;
            case "goblinHurt":
                audioSrc.PlayOneShot (goblinHurtSound);
                break;
            case "goblinDead":
                audioSrc.PlayOneShot (goblinDeadSound);
                break;
            case "goblinAttack":
                audioSrc.PlayOneShot (goblinAttackSound);
                break;
            case "trollHurt":
                audioSrc.PlayOneShot (trollHurtSound);
                break;
            case "trollDead":
                audioSrc.PlayOneShot (trollDeadSound);
                break;
            case "trollAttack":
                audioSrc.PlayOneShot (trollAttackSound);
                break;
            case "trollWalk":
                audioSrc.clip = trollWalkSound;
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
            case "playerSword":
                audioSrc.PlayOneShot(playerSword);
                break;
            case "playerBow":
                audioSrc.PlayOneShot(playerBow);
                break;
            case "playerTeleport":
                audioSrc.PlayOneShot(playerTeleport);
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
