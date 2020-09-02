using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPopcornSpawner : MonoBehaviour
{

    [SerializeField] private Collider spawnArea;
    [SerializeField] private GameObject kernelPreFab;
    [SerializeField] private float maxRoutineInterval = 1f;
    [SerializeField] private Mesh[] kernelMeshes;

    public void Play( )
    {
        StartCoroutine(SpawnRoutine());
    }
   private IEnumerator SpawnRoutine()
    {
        Bounds bounds = spawnArea.bounds;

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0, maxRoutineInterval));
            //Vector3 explosionPosition = explosionPoint.position;
            Vector3 position = new Vector3
                (Random.Range(bounds.min.x, bounds.max.x),
                 Random.Range(bounds.min.y, bounds.max.y),
                 Random.Range(bounds.min.z, bounds.max.z));
            Quaternion angle = Quaternion.Euler
                (Random.Range(-360, 360), Random.Range(-360, 360), Random.Range(-360, 360));
            GameObject kernel = Instantiate(kernelPreFab, position, angle);
            kernel.GetComponent<MeshFilter>().mesh = kernelMeshes[Random.Range(0, kernelMeshes.Length)];
           // Destroy(kernel, 4f);

        }
    }
}
