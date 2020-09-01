using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopcornParticleSpawner : MonoBehaviour
{
    [SerializeField] private float maxInitialKernelDistance;

    [SerializeField]  private Collider initialSpawnArea;
    [SerializeField] private Collider topSpawnArea;

    [SerializeField] private GameObject kernelPreFab;
    [SerializeField] private float unPoppedColliderSize =0.02f;
    [SerializeField] private float poppedColliderSize = 0.034f;
    [SerializeField] private float unPoppedScale = 36f;
    [SerializeField] private float poppedColliderScale = 68f;

    [SerializeField] private Material[] unPoppedMaterials;
    [SerializeField] private Material[] poppedMaterials;


    [SerializeField] private int initialKernelsNumber = 120;
    [SerializeField] private int burstNumber = 32;

    [SerializeField] private float maxPopInterval = 0.5f;

    [SerializeField] private Mesh[] kernelMeshes;
    [SerializeField] private Mesh seedMesh;


    private GameObject[] initialKernels;

    private void Start()
    {
    }

   private int stateIndex = 0;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            switch (stateIndex)
            {
                case 0:
                {
                    SpawnKernels();
                    break;
                }
                case 1:
                {
                        StartCoroutine(PopKernels());
                        Invoke("UnPhysicaliseInitialKernels", 6f);
                        break;
                }
                case 2:
                {
                        StartCoroutine(SpawnKernelBurst());
                        StartCoroutine(SpawnRoutine());
                        break;
                }
            }
            stateIndex++;


        }
    }
    [SerializeField] private Animator cameraAnimator;
    private void SpawnKernels()
    {
        initialKernels = new GameObject[initialKernelsNumber];
        Bounds bounds = initialSpawnArea.bounds;

        for (int i = 0; i < initialKernelsNumber; i++)
        {
            //yield return new WaitForSeconds(Random.Range(0, maxPopInterval));
            Vector3 position = new Vector3
                (Random.Range(bounds.min.x, bounds.max.x),
                 Random.Range(bounds.min.y, bounds.max.y),
                 Random.Range(bounds.min.z, bounds.max.z));

            GameObject kernel = Instantiate(kernelPreFab, position, Quaternion.identity) ;
            kernel.transform.localScale = Vector3.one * unPoppedScale;

            kernel.GetComponent<MeshFilter>().mesh = seedMesh;
            kernel.GetComponent<MeshRenderer>().materials = unPoppedMaterials;

            kernel.GetComponent<BoxCollider>().size =
                new Vector3(unPoppedColliderSize, unPoppedColliderSize, unPoppedColliderSize);

            initialKernels[i] = kernel;
        }
    }

    IEnumerator PopKernels()
    {
        for (int i = 0; i < initialKernelsNumber; i++)
        {

            GameObject kernel = initialKernels[i];
            kernel.transform.localScale = Vector3.one * poppedColliderScale;
            kernel.GetComponent<MeshFilter>().mesh = kernelMeshes[Random.Range(0, kernelMeshes.Length)];
            kernel.GetComponent<BoxCollider>().size = Vector3.one * poppedColliderSize;
            kernel.GetComponent<MeshRenderer>().materials = poppedMaterials;

            yield return new WaitForSeconds(Random.Range(0, maxPopInterval));

        }
    }

    void UnPhysicaliseInitialKernels()
    {
        if (initialKernels != null)
        {
            Vector3 position = transform.position;
            for (int i = 0; i < initialKernels.Length; i++)
            {
                if (initialKernels[i] != null)
                {
                    if(Vector3.Distance( initialKernels[i].transform.position, position) > maxInitialKernelDistance)
                    {
                        Destroy(initialKernels[i]);
                    }
                    else
                    {
                        Destroy(initialKernels[i].GetComponent<Rigidbody>());
                        Destroy(initialKernels[i].GetComponent<Collider>());
                    }
                }

            }
        }

        initialKernels = null;
    }
    [SerializeField] Transform explosionPoint;
    [SerializeField] float explosionForce;
    [SerializeField] float explosionRadius;
    [SerializeField] float explosionUpwardsModifier;


    IEnumerator SpawnKernelBurst()
    {
        cameraAnimator.SetTrigger("Transition");
        GameManager.GameState=(GameManager.GameStates.InGame);

        
        yield return null;
        Bounds bounds = topSpawnArea.bounds;
        Vector3 explosionPosition = explosionPoint.position;
        for (int i = 0; i < burstNumber; i++)
        {
           // yield return new WaitForSeconds(Random.Range(0, maxPopInterval));
            Vector3 position = new Vector3
                (Random.Range(bounds.min.x, bounds.max.x),
                 Random.Range(bounds.min.y, bounds.max.y),
                 Random.Range(bounds.min.z, bounds.max.z));
            GameObject kernel = Instantiate(kernelPreFab, position, Quaternion.identity);
            kernel.GetComponent<MeshFilter>().mesh = kernelMeshes[Random.Range(0, kernelMeshes.Length)];

            Rigidbody rigidbody = kernel.GetComponent<Rigidbody>();
            rigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRadius, explosionUpwardsModifier);
            Destroy(kernel, 2f);
        }
    }
    [SerializeField] private float maxRoutineInterval=1f;

    IEnumerator SpawnRoutine()
    {
        while(GameManager.GameState==GameManager.GameStates.InGame)
        {
            yield return new WaitForSeconds(Random.Range(0, maxRoutineInterval));
            Bounds bounds = topSpawnArea.bounds;
            Vector3 explosionPosition = explosionPoint.position;
            Vector3 position = new Vector3
                (Random.Range(bounds.min.x, bounds.max.x),
                 Random.Range(bounds.min.y, bounds.max.y),
                 Random.Range(bounds.min.z, bounds.max.z));
            GameObject kernel = Instantiate(kernelPreFab, position, Quaternion.identity);
            kernel.GetComponent<MeshFilter>().mesh = kernelMeshes[Random.Range(0, kernelMeshes.Length)];
            Rigidbody rigidbody = kernel.GetComponent<Rigidbody>();
            rigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRadius, explosionUpwardsModifier);
            Destroy(kernel, 2f);
            
        }
        
    }

}
