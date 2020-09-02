using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollower : MonoBehaviour
{
    public Transform target;
    [SerializeField] float yOffset;
    public void SetYOffset(float offset)
    {
        yOffset = offset;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = 
            Camera.main.WorldToScreenPoint(target.position) + Vector3.up * yOffset;
    }
}
