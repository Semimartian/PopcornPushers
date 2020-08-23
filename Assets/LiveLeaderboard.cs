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


    private void Start()
    {
        instance = this;
        ScoreChanged();
    }

    public void ScoreChanged()
    {
        scoreUIs.Sort((b, a) => (a.character.Bucket.Kernels.CompareTo(b.character.Bucket.Kernels)));
        scoreUIsParent.DetachChildren();
        for (int i = scoreUIs.Count-1; i >=0 ; i--)
        {
            scoreUIs[i].UpdateText();
            scoreUIs[i].transform.parent = scoreUIsParent;
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
