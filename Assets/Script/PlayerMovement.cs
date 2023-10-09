using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    Animator anim;
    CharacterController controller;
    Vector3 velocity;
    float gravity = -9.8f * 2;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundmask;
    public bool isGrounded;
    public float jumpHeight;
    float t;
    public Camera cam;
    // private Rigidbody rb;
    // public LayerMask ledgeLayer;
    // public float ledgeCheckDistance = 0.5f;
    // public float climbForce = 5f;

    // public bool isHanging = false;
    // private RaycastHit ledgeHit;
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        //rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float Horizontal = Input.GetAxis("Horizontal");
        anim.SetFloat("speed", Mathf.Abs(Horizontal));

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundmask);

        if(isGrounded && velocity.y <0){
               cam.transform.position = new Vector3(cam.transform.position.x,transform.position.y+1,transform.position.z );
            velocity.y = -2f;
            anim.SetBool("jump", false);

        }

        cam.transform.position = new Vector3(cam.transform.position.x,cam.transform.position.y,transform.position.z );
        
        float horizInput = Input.GetAxis("Horizontal");
        float horiz = horizInput * 7 * Time.deltaTime;
        controller.Move(new Vector3(0, 0, horiz));

        if(Input.GetButtonDown("Jump") && isGrounded){
            velocity.y = Mathf.Sqrt(jumpHeight * -2.5f *gravity);
            anim.SetBool("jump", true);
        }
        
        // if (!isHanging)
        // {
        //     if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hitInfo, 20f)){
        //         Debug.Log("hitsomthing");
        //         Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward)*hitInfo.distance, Color.green);
        //     }
        //     // Check for ledge while jumping
        //     if (Input.GetButtonDown("Jump"))
        //     {
        //         if (Physics.Raycast(transform.position, transform.forward, out ledgeHit, ledgeCheckDistance, ledgeLayer))
        //         {
        //             Debug.DrawRay(transform.position, transform.forward * ledgeCheckDistance, Color.green);
        //             // Grab the ledge
        //             isHanging = true;
        //             rb.useGravity = false;
        //             rb.velocity = Vector3.zero;
        //             transform.position = ledgeHit.point;
        //         }
                
        //     }
        // }
        // else
        // {
        //     // Climb onto the ledge
        //     if (Input.GetButtonDown("Jump"))
        //     {
        //         rb.AddForce(Vector3.up * climbForce, ForceMode.Impulse);
        //         isHanging = false;
        //         rb.useGravity = true;
        //     }
        // }

        if(transform.forward.z >0 && horizInput<0 || transform.forward.z <0 && horizInput>0){
            transform.Rotate(new Vector3(0,180,0));
        }

        velocity.y += gravity *Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void EndJump(){
        anim.SetBool("jump", false);
    }

   



    
    
}
