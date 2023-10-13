using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class PlayerMovement1 : MonoBehaviour
{
    [Header("Movement")]
    public float jumpHeight;
    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask groundmask;
    public bool isGrounded;
    public Transform groundCheck;
    public float groundCheckRadius = 0.4f;
    public Transform orientation;
    public float attackRange;
    float horizontalInput;
    Animator anim;
    Rigidbody rb;
    public Camera cam;
    bool facingRight;
    Collider[] groundCollisions;
    public bool isHanging = false;
    public Transform raycastClimb;
    public Transform raycastAttack;
    public LayerMask ledgeLayer;
    bool canMove;
    public float delayTime;
    Vector3 point;
    public bool attacking;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        anim = GetComponent<Animator>();
        canMove = true;
        facingRight = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // ground check
        groundCollisions = Physics.OverlapSphere(groundCheck.position, groundCheckRadius, groundmask);
        if(groundCollisions.Length >0){
            isGrounded = true;
        }
        else isGrounded = false;
        
        MyInput();
        LedgeDetection();        
        if (isGrounded && rb.velocity.y<0){
               anim.SetBool("jump", false);
        }
        if (Input.GetMouseButtonDown(0) && !attacking)
        {
            StartCoroutine(AttackCo());
        }
    }

    IEnumerator AttackCo(){
        attacking = true;
        anim.SetBool("attack", true);
        if(Physics.Raycast(raycastAttack.transform.position, raycastAttack.transform.forward, out RaycastHit hit, attackRange)){
                Enemy enemy = hit.transform.GetComponent<Enemy>();
                if(hit.collider.CompareTag("Enemy")){
                    enemy.TakeDamage(50);
                    enemy.KnockedBack();
                }
        }
        yield return new WaitForSeconds(.26f);
        anim.SetBool("attack", false);
        yield return new WaitForSeconds(.3f);
        attacking = false;

    }

    void LedgeDetection(){
        
        if (!isHanging)
        {
            // Check for ledge while jumping
            anim.SetBool("finishClimb", false);
            if (Physics.Raycast(raycastClimb.transform.position, raycastClimb.transform.forward, out RaycastHit ledgeHit, 1f, ledgeLayer))
            {
                // Grab the ledge
                if (Physics.Raycast(ledgeHit.point + (transform.forward * .1f) + (Vector3.up * 0.6f * playerHeight), Vector3.down, out var targetPos, playerHeight))
                point = targetPos.point;
                StartCoroutine(LedgeVaultCo(ledgeHit));
            }
                
        }

        if(isHanging){
            if(Input.GetButtonDown("Jump")){
                isHanging = false;
                anim.SetBool("ledge", false);
                StartCoroutine(ClimbingCo());
            }
        }
    }
    IEnumerator ClimbingCo(){
        yield return new WaitForSeconds(delayTime);
        transform.position = point;
        anim.SetBool("finishClimb", true);

        rb.useGravity = true;
        canMove = true;
    }

    IEnumerator LedgeVaultCo(RaycastHit ledgeHit){
        isHanging = true;
        anim.SetBool("ledge", true);
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        Vector3 offSet = transform.forward *-0.1f + transform.up*-2.25f;
        transform.position = ledgeHit.point + offSet;
        canMove = false;
        yield return new WaitForSeconds(.3f);

    }
 
    private void MyInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        anim.SetFloat("speed", Mathf.Abs(horizontalInput));
        if(canMove){
            rb.velocity = new Vector3(0, rb.velocity.y, horizontalInput*7);
        }
        else return;
        // when to jump
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            isGrounded = false;
            anim.SetBool("jump", true);
            rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
            
            
        }

        if(horizontalInput > 0 && !facingRight || horizontalInput <0 && facingRight)
            Flip(); 
       
    }

    private void Flip(){
        facingRight = !facingRight;
        transform.Rotate(new Vector3(0,180,0));

    }
  

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
     
    }
    void EndJump(){
        anim.SetBool("jump", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        // Check if the collision is with the target object
        if (Input.GetMouseButtonDown(0) && other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.TakeDamage(50);
        }
        PauseMenu obj = GetComponent<PauseMenu>();
        if(other.gameObject.tag== "Finish"){
            obj.winMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
    }
}