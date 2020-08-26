using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager instance;
    public static GameStates gameState;
    public enum GameStates
    {
        InGame,Podium
    }
    void Start()
    {
        instance = this;
        gameState = GameStates.InGame;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    [SerializeField] Animator[] transitioners;

    public static void Win()
    {
        gameState = GameStates.Podium;

        for (int i = 0; i < instance.transitioners.Length; i++)
        {
            instance.transitioners[i].SetTrigger("Transition");

        }
    }
}
