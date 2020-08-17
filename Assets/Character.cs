using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private KeyCode upKey;
    [SerializeField] private KeyCode downKey;
    [SerializeField] private KeyCode rightKey;
    [SerializeField] private KeyCode leftKey;

    [SerializeField] private float walkingSpeed;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private Rigidbody rigidbody;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 inputDirection = Vector3.zero;
        if (Input.GetKey(upKey))
        {
            inputDirection.z = 1;
        }
        else if (Input.GetKey(downKey))
        {
            inputDirection.z = -1;
        }

        if (Input.GetKey(rightKey))
        {
            inputDirection.x = 1;
        }
        else if (Input.GetKey(leftKey))
        {
            inputDirection.x = -1;
        }

        if(inputDirection!= Vector3.zero)
        {
            float influence = 0.5f;

            Quaternion zQ = new Quaternion();
            if (inputDirection.z != 0)
            {
                zQ = Quaternion.Euler(0, inputDirection.z == 1 ? 180 : 0, 0);
                influence += 0.5f; 
            }
            Quaternion xQ = new Quaternion();
            if (inputDirection.x != 0)
            {
                xQ = Quaternion.Euler(0, inputDirection.x == 1 ? -90 : 90, 0);
                influence -= 0.5f;

            }
            Quaternion rotation = Quaternion.Lerp(xQ, zQ, influence);
            transform.rotation = Quaternion.RotateTowards
                (transform.rotation, rotation, rotationSpeed* Time.deltaTime);
            transform.Translate(Vector3.forward * walkingSpeed * Time.deltaTime);
        }
    }
}
