using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : MonoBehaviour
{
    private int kernels = 0;
    [SerializeField] private Transform[] kernelSlots;

    public int Kernels
    {
        get { return kernels; }
    }

    private void Start()
    {
        Initialise();
    }

    private void Initialise()
    {
        kernels = 0;
    }
    private void CollectKernel(Kernel kernel)
    {

        kernel.Collect();
        for (int i = 0; i < kernelSlots.Length; i++)
        {
            if(kernelSlots[i].childCount == 0)
            {
                kernel.transform.parent = kernelSlots[i];
                kernel.transform.localPosition = Vector3.zero;
                break;

            }
        }

      //  if(kernel.kernelType == KernelTypes.Good)
        {
            kernels += 1;

        }
       /* else
        {
            kernels -= 1;
        }*/
        LiveLeaderboard.instance.ScoreChanged();
        Debug.Log("kernels:" + kernels);
    }

    private void CollectBomb(Bomb bomb)
    {
        bomb.Collect();
        for (int i = kernelSlots.Length-1; i >= 0; i--)
        {
            if (kernelSlots[i].childCount != 0)
            {
                Kernel kernelAtSlot = kernelSlots[i].GetChild(0).GetComponent<Kernel>();
                kernelSlots[i].DetachChildren();
                kernelAtSlot.Die();
                break;
            }
        }
        Debug.Log("BOOM!!!");

        if (kernels > 0)
        {
            kernels -= 1;
            LiveLeaderboard.instance.ScoreChanged();
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        Kernel kernel = other.GetComponent<Kernel>();
        if(kernel!=null && kernel.IsCollectable)
        {
            CollectKernel(kernel);
        }
        else
        {
            Bomb bomb = other.GetComponent<Bomb>();
            if (bomb != null)
            {
                CollectBomb(bomb);
            }
        }

    }


}
