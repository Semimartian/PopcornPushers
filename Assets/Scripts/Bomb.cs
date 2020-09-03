using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private KernelStates state;

    [SerializeField] private Transform graphics;
    private Vector3 rotation;
    private float yVelocity;
    [SerializeField] private GameObject highQualityGraphics;
    [SerializeField] private GameObject lowQualityGraphics;

    /* public bool IsCollectable
     { get { return state != KernelStates.Collected; } }*/

    private void Start()
    {
        highQualityGraphics.SetActive(!GameManager.Perform);
        lowQualityGraphics.SetActive(GameManager.Perform);

    }
    void FixedUpdate()
    {
        if (state != KernelStates.Collected)
        {
            graphics.Rotate(rotation * Time.deltaTime);
            if (state == KernelStates.Falling)
            {
                float t =
                (transform.position.y - World.MaxShadowHeight) / (World.MinShadowHeight - World.MaxShadowHeight);//TODO: Optimise
                transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);

                transform.Translate(new Vector3(0, -PhysicsManager.CollectablesFallSpeed * Time.deltaTime, 0));
                if (transform.position.y  <= World.CorrectedFloorHeight)
                {
                    //Debug.Log("bounce- Collider:" + collider.bounds.min.y +"floor:"+ World.FloorHeight);
                    Bounce();
                }
            }
            else
            {
                yVelocity -= PhysicsManager.CollectablesGravity * Time.deltaTime;
                transform.Translate(new Vector3(0, yVelocity * Time.deltaTime, 0));

                if (yVelocity < 0)
                {
                    Die();
                }
            }
        }
    }

    public void SpawnInitialise()
    {
        float r = 160f;
        rotation = new Vector3
            (Random.Range(-r, r), Random.Range(-r, r), Random.Range(-r, r));
        transform.localScale = Vector3.zero;
        state = KernelStates.Falling;
    }

    private void Die()
    {
        PopcornSpawner.recycleBomb(this);
    }

    private void Bounce()
    {
        state = KernelStates.Bouncing;
        yVelocity = PhysicsManager.CollectablesBounceForce;
    }

    public void Collect()
    {
        PopcornSpawner.SpawnExplosion(transform.position);
        SoundManager.PlayOneShotSoundAt(SoundNames.Explosion, transform.position);
        Die();
    }
}
