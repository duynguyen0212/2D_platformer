using System.Collections;
using System.Collections.Generic;
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
    private Rigidbody rb;
    bool hanging;
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
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
        

        if(transform.forward.z >0 && horizInput<0 || transform.forward.z <0 && horizInput>0){
            transform.Rotate(new Vector3(0,180,0));
        }

        velocity.y += gravity *Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void EndJump(){
        anim.SetBool("jump", false);
    }

    void Climbing(){

        if(rb.velocity.y<0 && !hanging){
            RaycastHit downHit;
            Vector3 lineDownStart = (transform.position +Vector3.up*1.5f)+transform.forward;
            Vector3 lineDownEnd = (transform.position +Vector3.up*0.7f)+transform.forward;
            Physics.Linecast(lineDownStart,lineDownEnd, out downHit, LayerMask.GetMask("Ledge"));
            //Foward Cast
            if (downHit.collider != null)
            {
                RaycastHit fwdHit;
                Vector3 lineFwdStart = new Vector3(transform.position.x,downHit.point.y -0.1f, transform.position.z);
                Vector3 lineFwdEnd = new Vector3(transform.position.x,downHit.point.y -0.1f, transform.position.z) +transform.forward;
                Physics.Linecast(lineFwdStart,lineFwdEnd, out fwdHit, LayerMask.GetMask("Ledge"));

                Debug.DrawLine(lineFwdStart, lineFwdEnd, Color.red);

                if (fwdHit.collider != null)
                {
                    rb.useGravity = false;
                    rb.velocity = Vector3.zero;

                    hanging = true;

                    //Hang animation

                    Vector3 hangingPos = new Vector3(fwdHit.point.x, downHit.point.y, fwdHit.point.z);
                    Vector3 offset = transform.forward * -0.1f + transform.up * -1f;

                    hangingPos += offset;
                    transform.position = hangingPos;

                    transform.forward = -fwdHit.normal;


                }
            }
        }


    }



    
    
}
