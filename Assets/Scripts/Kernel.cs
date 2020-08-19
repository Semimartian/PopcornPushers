using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kernel : MonoBehaviour
{
    [SerializeField] private Transform graphics;
    private Vector3 rotation;
    [SerializeField] private float fallSpeed;
    private  bool falling;
    private float yVelocity;
    [SerializeField] private float bounceForce;
    [SerializeField] private float gravity;

   public MeshFilter meshFilter;


    // Update is called once per frame

    private void Start()
    {
        float r = 160f;
        rotation = new Vector3
            (Random.Range(-r, r), Random.Range(-r, r), Random.Range(-r, r));
    }
    void FixedUpdate()
    {
        graphics.Rotate(rotation * Time.deltaTime);
        //Debug.Log(" Collider:" + collider.bounds.min.y);
        if (falling)
        {
            float t =
            (transform.position.y - World.MaxShadowHeight) / (World.MinShadowHeight - World.MaxShadowHeight);//TODO: Optimise
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);

            transform.Translate(new Vector3(0, -fallSpeed * Time.deltaTime, 0));
            if (transform.position.y <= World.FloorHeight)
            {
                //Debug.Log("bounce- Collider:" + collider.bounds.min.y +"floor:"+ World.FloorHeight);
                falling = false;
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
    public void SpawnInitialise()
    {
        transform.localScale = Vector3.zero;
        falling = true;
    }
    private void Die()
    {
        PopcornSpawner.recycleKernel(this);
    }

    private void Bounce()
    {
        yVelocity = bounceForce;
    }
    public void Collect()
    {
        Die();
    }
}
