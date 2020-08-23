using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopcornSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Collider rangeCollider;
    private Bounds range;
    [SerializeField] private Kernel kernelPreFab;
    private static List<Kernel> kernelPool;
    [SerializeField] private Mesh[] goodKernelMeshes;

    [SerializeField] private Bomb bombPreFab;
    private static List<Bomb> bombPool;
    //[SerializeField] private Mesh[] badKernelMeshes;


    void Start()
    {
        maxSpawnInterval = initialMaxSpawnInterval;
        range = rangeCollider.bounds;

        int kernelsToSpawn = 56;
        kernelPool = new List<Kernel>(kernelsToSpawn);
        for (int i = 0; i < kernelsToSpawn; i++)
        {
            Kernel kernel = Instantiate(kernelPreFab);
            kernel.gameObject.SetActive(false);
            kernelPool.Add(kernel);
        }

        int bombsToSpawn = 12;
        bombPool = new List<Bomb>(bombsToSpawn);
        for (int i = 0; i < bombsToSpawn; i++)
        {
            Bomb bomb = Instantiate(bombPreFab);
            bomb.gameObject.SetActive(false);
            bombPool.Add(bomb);
        }

    }

    private float nextSpawn;
   [SerializeField]  private float initialMaxSpawnInterval;
    public float maxSpawnInterval;

    [SerializeField] private float minMaxSpawnInteval;
    [SerializeField] private float maxSpawnreductionPerSecond;


    void Update()
    {
        if(Time.time >= nextSpawn)
        {
            SpawnKernel();
            nextSpawn += Random.Range(0, maxSpawnInterval);
        }
        if (maxSpawnInterval > minMaxSpawnInteval)
        {
            maxSpawnInterval -= maxSpawnreductionPerSecond * Time.deltaTime;

        }
        else
        {
            maxSpawnInterval = minMaxSpawnInteval;
        }
    }

    private void SpawnKernel()
    {
        Vector3 spawnPosition ;
        spawnPosition.y = World.MinShadowHeight;
        spawnPosition.x = Random.Range(range.min.x, range.max.x);
        spawnPosition.z = Random.Range(range.min.z, range.max.z);

        bool spawnBomb = Random.Range(0, 100) > 90;
        //TODO: repeatin code
        if (spawnBomb)
        {
            Bomb bomb = bombPool[0];
            bombPool.RemoveAt(0);
            bomb.gameObject.SetActive(true);
            bomb.transform.position = spawnPosition;
            bomb.SpawnInitialise();
        }
        else
        {
            Kernel kernel = kernelPool[0];
            kernelPool.RemoveAt(0);
            kernel.gameObject.SetActive(true);
            kernel.transform.position = spawnPosition;
            Mesh mesh = goodKernelMeshes[Random.Range(0, goodKernelMeshes.Length)];
            kernel.SpawnInitialise(mesh);

        }
    }

    public static void recycleKernel(Kernel kernel)
    {
        kernel.gameObject.SetActive(false);
        kernelPool.Add(kernel);
    }
    public static void recycleBomb(Bomb bomb)
    {
        bomb.gameObject.SetActive(false);
        bombPool.Add(bomb);
    }
}
