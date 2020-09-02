using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager instance;
    private static GameStates DT_gameState;
    public static GameStates GameState
    {
        get { return DT_gameState; }
        set
        {
            DT_gameState = value;
            instance.UICanvas.enabled = (value != GameStates.Intro);
        }
    }
    public bool perform;
    [SerializeField] private Canvas UICanvas;
    public static bool Perform
    {
        get
        { return instance.perform; }
    }

    public enum GameStates
    {
       Intro, InGame,Podium
    }
    void Start()
    {
        instance = this;
        GameState = GameStates.Intro;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.V))
        {
            Win();
        }

    }


    [SerializeField] Animator[] transitioners;
    [SerializeField] PodiumCharacter[] podiumCharacters;
    [SerializeField] private bool allowPushing;
    [SerializeField] public static bool AllowPushing
    {
        get { return instance.allowPushing; }
    }

    [SerializeField] GameObject crown;
    private static void Win()
    {
        GameState = GameStates.Podium;

        for (int i = 0; i < instance.transitioners.Length; i++)
        {
            instance.transitioners[i].SetTrigger("Transition");

        }

        Character[] charactersOrderdbyScore = LiveLeaderboard.GetCharactersOrderedScore();
       
        UIFollower[] allUIFollowers = FindObjectsOfType<UIFollower>();
        //UIFollower[] UIFollowersOrderdbyScore = new UIFollower[charactersOrderdbyScore.Length];

        for (int i = 0; i < instance.podiumCharacters.Length; i++)
        {
            for (int j = 0; j < allUIFollowers.Length; j++)
            {

                 if (allUIFollowers[j].target == charactersOrderdbyScore[i].uiAnchor)
                 {
                    allUIFollowers[j].target = instance.podiumCharacters[i].transform;
                    allUIFollowers[j].SetYOffset(236);
                }
            }
            instance.podiumCharacters[i].Prepare( charactersOrderdbyScore[i].Material,i+1);
        }
        Destroy(instance.crown);//.SetActive(false);

        instance.StartCoroutine(instance.FireConfettis());
    }

    [SerializeField] private ParticleSystem[] confettiSystems;
    [SerializeField] private EndPopcornSpawner endPopcornSpawner;

    private IEnumerator FireConfettis()
    {
        yield return new WaitForSeconds(1f);
        endPopcornSpawner.Play();
        confettiSystems[0].Play();
        //yield return new WaitForSeconds(0.2f);
        confettiSystems[1].Play();
        yield return new WaitForSeconds(0.2f);
        confettiSystems[2].Play();
        yield return new WaitForSeconds(0.2f);
        confettiSystems[3].Play();


    }

}
