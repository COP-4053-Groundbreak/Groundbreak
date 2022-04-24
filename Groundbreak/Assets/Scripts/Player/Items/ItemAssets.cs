using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script that holds references to all the item sprites, attatched to SpriteAssets game object
public class ItemAssets : MonoBehaviour
{
    // Singleton patern to create only one instance of this class
    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    // Passive items
    public Sprite healthRingSprite;
    public Sprite speedBootsSprite;
    public Sprite lightShieldSprite;
    public Sprite firePendantSprite;
    public Sprite waterPendantSprite;
    public Sprite earthPendantSprite;
    public Sprite airPendantSprite;
    public Sprite initiativeWandSprite;

    // Lv 2
    public Sprite cheeseSprite;
    public Sprite mercuryBootsSprite;
    public Sprite heavyShieldSprite;
    public Sprite fireStaffSprite;
    public Sprite waterStaffSprite;
    public Sprite earthStaffSprite;
    public Sprite airStaffSprite;
    public Sprite wildWandSprite;

    // Consumable Items
    public Sprite smallHealthPotion;
    public Sprite largeHealthPotion;
    public Sprite smallSpeedPotion;
    public Sprite largeSpeedPotion;
    public Sprite smallStoneSkinPotion;
    public Sprite largeStoneSkinPotion;
    public Sprite smallStrengthPotion;
    public Sprite largeStrengthPotion;

    // Active Items
    public Sprite swordSprite;
    public Sprite bowSprite;
    public Sprite blinkRuneSprite;
    public Sprite fireballScrollSprite;
    public Sprite repulsionWandSprite;
    public Sprite attractionWandSprite;


}
