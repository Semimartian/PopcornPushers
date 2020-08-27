using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KernelTypes:byte
{
    Good,Bad
}

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
    [SerializeField] private float fallSpeed;
    //private  bool falling;
    private float yVelocity;
    [SerializeField] private float bounceForce;
    [SerializeField] private float gravity;

    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private GameObject highQualityGraphics;
    [SerializeField] private GameObject lowQualityGraphics;

    public bool IsCollectable
    { get { return state != KernelStates.Collected; } }


    private void Start()
    {
        highQualityGraphics.SetActive(!GameManager.Perform);
        lowQualityGraphics.SetActive(GameManager.Perform);

    }
    // Update is called once per frame

    void FixedUpdate()
    {
        if(state!= KernelStates.Collected)
        {
            graphics.Rotate(rotation * Time.deltaTime);
            //Debug.Log(" Collider:" + collider.bounds.min.y);
            if (state == KernelStates.Falling)
            {
                float t =
                (transform.position.y - World.MaxShadowHeight) / (World.MinShadowHeight - World.MaxShadowHeight);//TODO: Optimise
                transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);

                transform.Translate(new Vector3(0, -fallSpeed * Time.deltaTime, 0));
                if (transform.position.y <= World.CorrectedFloorHeight)
                {
                    //Debug.Log("bounce- Collider:" + collider.bounds.min.y +"floor:"+ World.FloorHeight);
                    Bounce();
                }
            }
            else
            {
                yVelocity -= gravity * Time.deltaTime;
                transform.Translate(new Vector3(0, yVelocity * Time.deltaTime, 0));

                if (yVelocity < 0)
                {
                    Die();
                }
            }
        }
       

    }
    public void SpawnInitialise( Mesh mesh )
    {
        float r = 160f;
        rotation = new Vector3
            (Random.Range(-r, r), Random.Range(-r, r), Random.Range(-r, r));
        transform.localScale = Vector3.zero;
        state = KernelStates.Falling;
        //kernelType = type;
        meshFilter.mesh = mesh;
    }
    public void Die()
    {
        PopcornSpawner.recycleKernel(this);
    }

    private void Bounce()
    {
        state = KernelStates.Bouncing;
        yVelocity = bounceForce;
    }

    public void Collect()
    {
        state = KernelStates.Collected;
        //Die();
    }
}
