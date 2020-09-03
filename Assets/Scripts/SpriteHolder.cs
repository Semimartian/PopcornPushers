using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteHolder : MonoBehaviour
{
   [SerializeField] private Sprite[] ScoreUISprites;
    private static SpriteHolder instance;
    private void Awake()
    {
        instance = this;
    }

    public static Sprite GetScoreUISprite(int index)
    {
        index= Mathf.Clamp(index, 0, instance.ScoreUISprites.Length - 1);
        return instance.ScoreUISprites[index];
    }
}
