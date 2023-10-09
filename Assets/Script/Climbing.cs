using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing : MonoBehaviour
{
    public LayerMask ledgeLayer;
    public float ledgeCheckDistance = 0.5f;
    public float climbForce = 5f;

    public bool isHanging = false;
    private RaycastHit ledgeHit;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!isHanging)
        {
            // Check for ledge while jumping
            if (Input.GetButtonDown("Jump"))
            {
                if (Physics.Raycast(transform.position, transform.forward, out ledgeHit, ledgeCheckDistance, ledgeLayer))
                {
                    Debug.DrawRay(transform.position, transform.forward * ledgeCheckDistance, Color.green);
                    // Grab the ledge
                    isHanging = true;
                    rb.useGravity = false;
                    rb.velocity = Vector3.zero;
                    transform.position = ledgeHit.point;
                }
                Debug.Log("jumping");
            }
        }
        else
        {
            // Climb onto the ledge
            if (Input.GetButtonDown("Jump"))
            {
                rb.AddForce(Vector3.up * climbForce, ForceMode.Impulse);
                isHanging = false;
                rb.useGravity = true;
            }
        }
    }
}
