//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopcornParticleSpawner : MonoBehaviour
{
    [SerializeField] private float initialKernelDestructionDistance;


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
    [SerializeField] private OneShotSound oneShotSoundPreFab;

    private int stateIndex = 0;
    [SerializeField] private float timeToUnphysicalise = 30f;
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
                        Invoke("UnPhysicaliseInitialKernels", timeToUnphysicalise);
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

    [SerializeField] private AudioClip[] popSounds;

    private IEnumerator PopKernels()
    {
        for (int i = 0; i < initialKernelsNumber; i++)
        {

            GameObject kernel = initialKernels[i];
            kernel.transform.localScale = Vector3.one * poppedColliderScale;
            kernel.GetComponent<MeshFilter>().mesh = kernelMeshes[Random.Range(0, kernelMeshes.Length)];
            kernel.GetComponent<BoxCollider>().size = Vector3.one * poppedColliderSize;
            kernel.GetComponent<MeshRenderer>().materials = poppedMaterials;
            OneShotSound sound = Instantiate(oneShotSoundPreFab, kernel.transform.position, Quaternion.identity);
            sound.Play(popSounds[Random.Range(0, popSounds.Length)], 0.7f, 1.3f);
            yield return new WaitForSeconds(Random.Range(0, maxPopInterval));

        }
    }

    private void UnPhysicaliseInitialKernels()
    {
        if (initialKernels != null)
        {
            Vector3 position = transform.position;
            List<Transform> distantKernels = new List<Transform>();
            for (int i = 0; i < initialKernels.Length; i++)
            {
                if (initialKernels[i] != null)
                {
                    if(Vector3.Distance( initialKernels[i].transform.position, position) > initialKernelDestructionDistance)
                    {
                        distantKernels.Add(initialKernels[i].transform);
                    }
                    else
                    {
                        Destroy(initialKernels[i].GetComponent<Rigidbody>());
                        Destroy(initialKernels[i].GetComponent<Collider>());
                    }
                }

            }

            StartCoroutine(ShrinkAndDestroy(distantKernels.ToArray(), 0.1f, 1f));

        }

        initialKernels = null;
    }
    [System.Serializable] struct ExplosionParameters
    {
        public Transform point;
        public float force;
        public float radius;
        public float upwardsModifier;
    }
    [SerializeField] ExplosionParameters burstExplosion;
    [SerializeField] ExplosionParameters routineExplosion;

    private Transform[] burstKernels;
    IEnumerator SpawnKernelBurst()
    {
        cameraAnimator.SetTrigger("Transition");
        GameManager.GameState=(GameManager.GameStates.InGame);

        
        yield return null;
        Bounds bounds = topSpawnArea.bounds;
        Vector3 explosionPosition = burstExplosion.point.position;
        burstKernels = new Transform[burstNumber];
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
            rigidbody.AddExplosionForce
                (burstExplosion.force, explosionPosition, burstExplosion.radius, burstExplosion.upwardsModifier);

            burstKernels[i] = kernel.transform;
        }
        StartCoroutine(ShrinkAndDestroy(burstKernels,1.6f,1.6f));
    }

    IEnumerator ShrinkAndDestroy(Transform[] transforms, float startIn, float timeToShrink)
    {
        //Debug.Log("transforms.length" + transforms.Length);
        yield return new WaitForSeconds(startIn);

        float time = 0;
        Vector3 reducer = (transforms[0].localScale / timeToShrink);
        while (time< timeToShrink)
        {
            Vector3 newScale = (transforms[0].localScale - ((reducer * Time.deltaTime) ));
            for (int i = 0; i < transforms.Length; i++)
            {
                transforms[i].localScale = newScale;
            }
            time += Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < transforms.Length; i++)
        {
            Destroy(transforms[i].gameObject);
        }

    }

    [SerializeField] private float maxRoutineInterval=1f;

    IEnumerator SpawnRoutine()
    {
        while(GameManager.GameState==GameManager.GameStates.InGame)
        {
            yield return new WaitForSeconds(Random.Range(0, maxRoutineInterval));
            Bounds bounds = topSpawnArea.bounds;
            Vector3 explosionPosition = routineExplosion.point.position;
            Vector3 position = new Vector3
                (Random.Range(bounds.min.x, bounds.max.x),
                 Random.Range(bounds.min.y, bounds.max.y),
                 Random.Range(bounds.min.z, bounds.max.z));
            GameObject kernel = Instantiate(kernelPreFab, position, Quaternion.identity);
            kernel.GetComponent<MeshFilter>().mesh = kernelMeshes[Random.Range(0, kernelMeshes.Length)];
            Rigidbody rigidbody = kernel.GetComponent<Rigidbody>();
            rigidbody.AddExplosionForce
                (routineExplosion.force, explosionPosition, routineExplosion.radius, routineExplosion.upwardsModifier);
            Destroy(kernel, 2f);
            
        }
        
    }

}
