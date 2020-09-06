using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteHolder : MonoBehaviour
{
    [SerializeField] private Sprite[] scoreUISprites;
    [SerializeField] private Sprite[] shortScoreUISprites;

    private static SpriteHolder instance;
    private void Awake()
    {
        instance = this;
    }

    public static Sprite GetScoreUISprite(int index)
    {
        Sprite[] sprites = GameManager.SHORT_GAME ? instance.shortScoreUISprites : instance.scoreUISprites;

        index = Mathf.Clamp(index, 0, sprites.Length - 1);
        return sprites[index];
    }
}
