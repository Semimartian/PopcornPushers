using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodiumCharacter : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer renderer;
    [SerializeField] Animator animator;

    public void Prepare(Material mat,int place)
    {
        renderer.material = mat;
        animator.SetInteger("Place", place);
        animator.SetTrigger("Play");

    }
}
