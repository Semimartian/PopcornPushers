using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public enum KernelTypes:byte
{
    Good,Bad
}*/

public enum KernelStates : byte
{
    Falling, Bouncing, Collected
}

public class Kernel : MonoBehaviour
{
    private KernelStates state;

    //public KernelTypes kernelType;

    [SerializeField] private Transform graphics;
    private Vector3 rotation;
   
    //[SerializeField] private float fallSpeed;
    //private  bool falling;
    private float yVelocity;
    //[SerializeField] private float bounceForce;
    //[SerializeField] private float gravity;

    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer renderer;

    [SerializeField] private GameObject highQualityGraphics;
    [SerializeField] private GameObject lowQualityGraphics;

    public bool IsCollectable
    { get { return state != KernelStates.Collected; } }


    private void Start()
    {
        highQualityGraphics.SetActive(!GameManager.Perform);
        lowQualityGraphics.SetActive(GameManager.Perform);
    }

    void FixedUpdate()
    {
        if(state != KernelStates.Collected)
        {
            float deltaTime = Time.deltaTime;
            graphics.Rotate(rotation * deltaTime);
            //Debug.Log(" Collider:" + collider.bounds.min.y);
            if (state == KernelStates.Falling)
            {
                float t =
                (transform.position.y - World.MaxShadowHeight) / (World.MinShadowHeight - World.MaxShadowHeight);//TODO: Optimise
                transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);

                transform.Translate(new Vector3(0, -PhysicsManager.CollectablesFallSpeed *deltaTime, 0));
                if (transform.position.y <= World.CorrectedFloorHeight)
                {
                    //Debug.Log("bounce- Collider:" + collider.bounds.min.y +"floor:"+ World.FloorHeight);
                    Bounce();
                }
            }
            else
            {
                float newYVelocity = yVelocity - PhysicsManager.CollectablesGravity * deltaTime;
               // yVelocity -= PhysicsManager.CollectablesGravity * deltaTime;

                if (yVelocity >= 0 && newYVelocity<0)
                {
                    Disappear();
                    //Die();
                }
                yVelocity = newYVelocity;
                transform.Translate(new Vector3(0, yVelocity * deltaTime, 0));

            }
        }
    }

    private void Disappear()
    {
        StartCoroutine(BlinkCoroutine());
    }

    public void SpawnInitialise( Mesh mesh,Material[] materials )
    {
        //Debug.Log("material"+ material.ToString());
        float r = 160f;
        rotation = new Vector3
            (Random.Range(-r, r), Random.Range(-r, r), Random.Range(-r, r));
        transform.localScale = Vector3.zero;
        state = KernelStates.Falling;
        //kernelType = type;
        meshFilter.mesh = mesh;
        renderer.materials = materials;
    }
    public void Die()
    {
        PopcornSpawner.recycleKernel(this);
    }

    private void Bounce()
    {
        state = KernelStates.Bouncing;
        yVelocity = PhysicsManager.CollectablesBounceForce;
    }

    public void Collect()
    {
        state = KernelStates.Collected;
        //Die();
    }
    private IEnumerator BlinkCoroutine()
    {
        for (int i = 0; i < PhysicsManager.CollectablesNumberOfBlinks; i++)
        {
            graphics.gameObject.SetActive(false);
            yield return new WaitForSeconds(PhysicsManager.CollectablesBlinkInterval);
            graphics.gameObject.SetActive(true);
            yield return new WaitForSeconds(PhysicsManager.CollectablesBlinkInterval);
        }
        Die();
    }
}
