using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    [SerializeField] private float minShadowHeight;
    [SerializeField] private float maxShadowHeight;
    private static float floorHeight;
    [SerializeField] private Collider floorCollider;


    public static float FloorHeight
    {
        get { return floorHeight; }
    }
    public static float MinShadowHeight
    {
        get { return instance.minShadowHeight; }
    }

    public static float MaxShadowHeight
    {
        get { return instance.maxShadowHeight; }
    }
    public static World instance;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        floorHeight = floorCollider.bounds.max.y;

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
