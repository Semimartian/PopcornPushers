using System;
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
    [SerializeField] private float graphicsRotationSpeed;

    //[SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Collider collider;

    [SerializeField] private Transform graphicsTransform;

    [SerializeField] private Animator animator;

    [SerializeField] private Color colour;
    public Color Colour { get { return colour; } }

    [SerializeField] private Bucket bucket;

    private Vector3 externalForces;
    private float externalForcesReductionPerSecond=8f;
    public Bucket Bucket
    {
        get { return bucket; }
    }

   public Transform crownAnchor;

    /*[SerializeField] private TMPro.TextMeshProUGUI text;
    [SerializeField] private Transform textAnchor;*/



    void Start()
    {
        Initialise();
    }
    private void Initialise()
    {
        

    }

    void FixedUpdate()
    {
        if (externalForces.magnitude > 0)
        {
            transform.position += externalForces * Time.deltaTime;

            externalForces = Vector3.ClampMagnitude
                (externalForces, externalForces.magnitude - (externalForcesReductionPerSecond * Time.deltaTime));
        }


        // UpdateText();
        bool walking = false;
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
            walking = true;
            float influence = 0.5f;

            Quaternion zQ = new Quaternion();
            if (inputDirection.z != 0)
            {
                zQ = Quaternion.Euler(0, inputDirection.z == 1 ? 0 : 180, 0);
                influence += 0.5f; 
            }
            Quaternion xQ = new Quaternion();
            if (inputDirection.x != 0)
            {
                xQ = Quaternion.Euler(0, inputDirection.x == 1 ? 90 : -90, 0);
                influence -= 0.5f;

            }
            Quaternion rotation = Quaternion.Lerp(xQ, zQ, influence);
            transform.rotation = Quaternion.RotateTowards
                (transform.rotation, rotation, graphicsRotationSpeed * Time.deltaTime);

            Vector3 movementVector = Vector3.ClampMagnitude(inputDirection, walkingSpeed * Time.deltaTime);

           /* RaycastHit collisionDetector ;
           // Debug.Log("transform.forward" + transform.forward);
           if(Physics.Raycast(transform.position, transform.forward,out collisionDetector, 3f))
           {
                Debug.Log("hit");
                movementVector = Vector3.ClampMagnitude
                    (movementVector, Vector3.Magnitude
                    (new Vector3(transform.position.x, transform.position.y, collider.bounds.max.z) - collisionDetector.point));
                movementVector = Vector3.zero;

           }
            Debug.DrawRay(transform.position, transform.forward, Color.red, 0.2f);*/
            // inputDirection

           // rigidbody.MovePosition(transform.position + movementVector);
           transform.position += movementVector;

            TryPush();

            
            
           // transform.Translate(Vector3.forward * walkingSpeed * Time.deltaTime);
        }

        animator.SetBool("Walking", walking);
    }


    private void TryPush()
    {
        float height = collider.bounds.max.y- collider.bounds.min.y;
        for (float i = 0; i < 1.1f; i+=0.25f)
        {

            RaycastHit collisionDetector;

            // Debug.Log("transform.forward" + transform.forward);
            if (Physics.Raycast(transform.position + Vector3.up*(height*i), transform.forward, out collisionDetector, 1.5f))
            {
                Character otherCharacter = (collisionDetector.collider.gameObject.GetComponent<Character>());
                if (otherCharacter != null)
                {
                    //otherCharacter.rigidbody.AddExplosionForce(3f,transform.position,2f);
                    Vector3 force = transform.forward * 10;
                    force.y = 5;
                    otherCharacter.externalForces = force;
                    Debug.Log("hit");
                    break;

                }

            }

            Debug.DrawRay(transform.position, transform.forward, Color.red, 0.2f);
        }

       

    }
}
