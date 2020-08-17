using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kernel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float t =
            (transform.position.y - World.MaxShadowHeight) / (World.MinShadowHeight - World.MaxShadowHeight);//TODO: Optimise
        transform.localScale = Vector3.Lerp(Vector3.one,Vector3.zero, t);
    }
}
