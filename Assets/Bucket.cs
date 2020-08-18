using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : MonoBehaviour
{
    [SerializeField] private Character character;
    private int kernels = 0;

    private void Start()
    {
        Initialise();
    }

    private void Initialise()
    {
        kernels = 0;
    }
    private void Collect(Kernel kernel)
    {
        kernel.Collect();
        kernels += 1;
        Debug.Log("kernels:" + kernels);
    }

    private void OnTriggerEnter(Collider other)
    {
        Kernel kernel = other.GetComponent<Kernel>();
        if(kernel!=null)
        {
            Collect(kernel);
        }
    }
}
