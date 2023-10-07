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
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float Horizontal = Input.GetAxis("Horizontal");
        anim.SetFloat("speed", Mathf.Abs(Horizontal));

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundmask);

        if(isGrounded && velocity.y <0){
            velocity.y = -2f;
        }
        
        float horizInput = Input.GetAxis("Horizontal") * 7 * Time.deltaTime;
        controller.Move(new Vector3(0, 0, horizInput));

        if(Input.GetButtonDown("Jump") && isGrounded){
            velocity.y = Mathf.Sqrt(jumpHeight * -2 *gravity);
        }

        if(transform.forward.z >0 && horizInput<0 || transform.forward.z <0 && horizInput>0){

        }

        velocity.y += gravity *Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void EndTurn(){
        anim.SetBool("turning", false);
    }
}
