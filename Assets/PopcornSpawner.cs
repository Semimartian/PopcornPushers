using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopcornSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Collider rangeCollider;
    private Bounds range;
    [SerializeField] private GameObject kernelPreFab;

    void Start()
    {
        range = rangeCollider.bounds;
        InvokeRepeating("SpawnKernel", 5, 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnKernel()
    {
        Vector3 spawnPosition ;
        spawnPosition.y = World.MinShadowHeight;
        spawnPosition.x = Random.Range(range.min.x, range.max.x);
        spawnPosition.z = Random.Range(range.min.z, range.max.z);

        Instantiate(kernelPreFab, spawnPosition, Quaternion.identity);
    }
}
