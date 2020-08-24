using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;
    // Start is called before the first frame update
    private void Start()
    {
        transform.localScale = Vector3.one * 3.5f;
    }
    public void SpawnInitialise()
    {
        particleSystem.Play();
        Invoke("Die", 2f);
    }
    private void Die()
    {
        PopcornSpawner.recycleExplosion(this);
    }
}
