using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopcornParticleSpawner : MonoBehaviour
{
    [SerializeField]  private Collider area;
    [SerializeField] private GameObject kernelPreFab;
    [SerializeField] private int kernels =120;
    [SerializeField] private float maxPopInterval = 0.5f;

    [SerializeField] private Mesh[] kernelMeshes;


    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(SpawnKernels());
        }
    }

    IEnumerator SpawnKernels()
    {
        Bounds bounds = area.bounds;

        for (int i = 0; i < kernels; i++)
        {
            yield return new WaitForSeconds(Random.Range(0, maxPopInterval));
            Vector3 position = new Vector3
                (Random.Range(bounds.min.x, bounds.max.x),
                 Random.Range(bounds.min.y, bounds.max.y),
                 Random.Range(bounds.min.z, bounds.max.z));
           GameObject kernel = Instantiate(kernelPreFab, position, Quaternion.identity) ;
            kernel.GetComponent<MeshFilter>().mesh = kernelMeshes[Random.Range(0, kernelMeshes.Length)];
        }
    }

}
