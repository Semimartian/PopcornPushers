using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveLeaderboard : MonoBehaviour
{
    /*private class PlayerRating
    {
        public Character character;
        public int place;
    }*/
    [SerializeField] Transform[] slots;
    //[SerializeField] Transform[] slots;

    //private PlayerRating[] playerRatings;
    [SerializeField] private List<ScoreUI> scoreUIs;
    [SerializeField] private float scoreUISpeed = 0.5f;
    public static LiveLeaderboard instance;
    [SerializeField] private Transform scoreUIsParent;
    [SerializeField] private UIFollower Crown;
    [SerializeField] private int kernelsToWin;

    public static Character[] GetCharactersOrderedScore()
    {
        Character[] charactersOrderedByScore = new Character[instance.scoreUIs.Count];
        for (int i = 0; i < charactersOrderedByScore.Length; i++)
        {
            charactersOrderedByScore[i] = instance.scoreUIs[i].character;
        }
        return charactersOrderedByScore;
    }
    private void Start()
    {
        instance = this;
        ScoreChanged();
        for (int i = 0; i < scoreUIs.Count; i++)
        {
            scoreUIs[i].transform.position = slots[i].position;
        }
    }

    public void ScoreChanged()
    {
        if (GameManager.GameState != GameManager.GameStates.InGame)
        {
           // return;
        }
        scoreUIs.Sort((b, a) => (a.character.Bucket.Kernels.CompareTo(b.character.Bucket.Kernels)));
        scoreUIsParent.DetachChildren();
        for (int i = scoreUIs.Count-1; i >=0 ; i--)
        {
            scoreUIs[i].UpdateText();
            scoreUIs[i].transform.parent = scoreUIsParent;
        }


        int highestScore = 0;
        Character winner = null;
        Crown.gameObject.SetActive(false);
        for (int i = 0; i < scoreUIs.Count - 1; i++)
        {
            if(scoreUIs[i].character.Bucket.Kernels > highestScore)
            {
                highestScore = scoreUIs[i].character.Bucket.Kernels;
                winner = scoreUIs[i].character;
            }
            else if (scoreUIs[i].character.Bucket.Kernels == highestScore)
            {
                winner = null;
            }
        }

        if(winner!= null)
        {
            Crown.gameObject.SetActive(true);
            Crown.target = winner.uiAnchor;
            if(winner.Bucket.Kernels>= kernelsToWin)
            {
               // GameManager.Win();
            }
        }

        
    }


    private void Update()
    {
        for (int i = 0; i < scoreUIs.Count; i++)
        {
            scoreUIs[i].transform.position =
                Vector3.MoveTowards(scoreUIs[i].transform.position, slots[i].position, scoreUISpeed * Time.deltaTime);
        }

    }
}
