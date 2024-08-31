using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Playercontroller : MonoBehaviour
{
    [SerializeField] float torqueAmount = 1f;
    [SerializeField] float jumpForce = 1000f;
    [SerializeField] float gravityOnGrounded = 10f;
    [SerializeField] float gravityOnJumping = 10f;
    [SerializeField] Transform groundCheck;  // Reference to the GroundCheck GameObject
    [SerializeField] LayerMask groundLayer;  // LayerMask to specify what is considered ground

    Rigidbody2D rb2d;
    Animator animator;

    bool isGrounded = false;
    float groundCheckRadius = 0.2f; //radius of circle that check groundcheck

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer); //check if player is grounded or not 

        if(isGrounded){
            animator.SetBool("isJumping", false);
            rb2d.gravityScale = gravityOnGrounded;
        }else{
            animator.SetBool("isJumping", true);
            rb2d.gravityScale = gravityOnJumping;
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            rb2d.AddTorque(torqueAmount);   //Add torque to rotate left
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            rb2d.AddTorque(-torqueAmount);  //Add torque to rotate right

        if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W)) && isGrounded)
        {
            animator.SetBool("isJumping", true);
            rb2d.gravityScale = gravityOnJumping;   //changing gravity while jumping to avoide floty jump
            rb2d.AddForce(Vector2.up * jumpForce * Time.deltaTime, ForceMode2D.Impulse);
        }
    }

    void OnDrawGizmos()
    {
        // Draw a circle in the Scene view to visualize the ground check
        if(groundCheck == null){return;}
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
