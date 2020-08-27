using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager instance;
    public static GameStates gameState;
    public bool perform;
    public static bool Perform
    {
        get
        { return instance.perform; }
    }

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
        if (Input.GetKeyDown(KeyCode.V))
        {
            Win();
        }
    }


    [SerializeField] Animator[] transitioners;

    private static void Win()
    {
        gameState = GameStates.Podium;

        for (int i = 0; i < instance.transitioners.Length; i++)
        {
            instance.transitioners[i].SetTrigger("Transition");

        }
    }
}
