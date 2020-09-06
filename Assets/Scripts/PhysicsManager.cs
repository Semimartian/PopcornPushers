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
    private void Start()
    {
        maxKernelSize = Vector3.one;
        if (GameManager.SHORT_GAME)
        {
            maxKernelSize *= 1.333f;
        }

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


    private Vector3 maxKernelSize;
    public static Vector3 MaxKernelSize
    { get { return instance.maxKernelSize; }
    }
}
