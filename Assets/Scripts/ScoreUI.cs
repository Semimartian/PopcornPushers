using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreUI : MonoBehaviour
{
    public Character character;
    [SerializeField] private TMPro.TextMeshProUGUI text;
    [SerializeField] private Image image;

    // Start is called before the first frame update
    void Start()
    {
       // text.color = character.Colour;
        image.color = character.Colour; ;
    }
    public void UpdateText()
    {
        // text.transform.position = Camera.main.WorldToScreenPoint(textAnchor.position);
        int score = character.Bucket.Kernels;
        text.text = score.ToString();
        image.sprite = SpriteHolder.GetScoreUISprite(score);

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
