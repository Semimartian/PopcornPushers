using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsManager : MonoBehaviour
{
    public static PhysicsManager instance;
    private void Awake()
    {
        instance = this;
    }
    [SerializeField] private float collectablesFallSpeed = 5;
    public static float CollectablesFallSpeed
    {
        get { return instance.collectablesFallSpeed; }
    }

    [SerializeField] private float collectablesBounceForce = 8;
    public static float CollectablesBounceForce
    {
        get { return instance.collectablesBounceForce; }
    }

    [SerializeField] private float collectablesGravity = 6;
    public static float CollectablesGravity
    {
        get { return instance.collectablesGravity; }
    }

    [SerializeField] private float collectablesBlinkInterval;
    public static float CollectablesBlinkInterval
    {
        get { return instance.collectablesBlinkInterval; }
    }
    [SerializeField] private int collectablesNumberOfBlinks;
    public static float CollectablesNumberOfBlinks
    {
        get { return instance.collectablesNumberOfBlinks; }
    }
}
