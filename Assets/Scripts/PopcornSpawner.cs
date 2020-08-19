﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopcornSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Collider rangeCollider;
    private Bounds range;
    [SerializeField] private Kernel kernelPreFab;
    private static List<Kernel> kernelPool;
    [SerializeField] private Mesh[] kernelMeshes;


    void Start()
    {
        range = rangeCollider.bounds;
        int kernelsToSpawn = 32;
        kernelPool = new List<Kernel>(kernelsToSpawn);
        for (int i = 0; i < kernelsToSpawn; i++)
        {
            Kernel kernel = Instantiate(kernelPreFab);
            kernel.meshFilter.mesh = kernelMeshes[Random.Range(0, kernelMeshes.Length)];
            kernel.gameObject.SetActive(false);
            kernelPool.Add(kernel);
        }

    }



    private float nextSpawn;
   [SerializeField]  private float maxSpawnInterval;

    void Update()
    {
        if(Time.time >= nextSpawn)
        {
            SpawnKernel();
            nextSpawn += Random.Range(0, maxSpawnInterval);
        }
    }

    private void SpawnKernel()
    {
        Vector3 spawnPosition ;
        spawnPosition.y = World.MinShadowHeight;
        spawnPosition.x = Random.Range(range.min.x, range.max.x);
        spawnPosition.z = Random.Range(range.min.z, range.max.z);

        Kernel kernel = kernelPool[0];
        kernelPool.RemoveAt(0);
        kernel.gameObject.SetActive(true);
        kernel.transform.position = spawnPosition;
        kernel.SpawnInitialise();
    }

    public static void recycleKernel(Kernel kernel)
    {
        kernel.gameObject.SetActive(false);
        kernelPool.Add(kernel);
    }
}