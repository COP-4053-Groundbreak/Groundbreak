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

    public Sprite healthRingSprite;
    public Sprite speedBootsSprite;
    public Sprite lightShieldSprite;
    public Sprite firePendantSprite;
    public Sprite waterPendantSprite;
    public Sprite earthPendantSprite;
    public Sprite airPendantSprite;
    public Sprite initiativeWandSprite;

}
