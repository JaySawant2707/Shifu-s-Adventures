using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Playercontroller : MonoBehaviour
{
    [SerializeField] float torqueAmount = 1f;
    [SerializeField] float jumpForce = 1000f;
    [SerializeField] float gravityOnGroundDown = 10f;
    [SerializeField] float gravityOnGroundUp = 10f;
    //[SerializeField] float gravityOnJumping = 10f;
    [SerializeField] float gravityOnJumpingDown = 10f;
    [SerializeField] float gravityOnJumpingUp = 10f;
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

        if (isGrounded)
        {
            animator.SetBool("isJumping", false);
            rb2d.gravityScale = gravityOnGroundDown;
        }
        else
        {
            animator.SetBool("isJumping", true);
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            RotateOnJump(torqueAmount);
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            RotateOnJump(-torqueAmount);

        if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W)) && isGrounded)
        {
            Jump();
        }

        if (rb2d.velocity.y < 0f && !isGrounded)
        {
            rb2d.gravityScale = gravityOnJumpingDown;
        }
        else if (rb2d.velocity.y > 0f && !isGrounded)
        {
            rb2d.gravityScale = gravityOnJumpingUp;
        }

        if (rb2d.velocity.y > 0f && isGrounded)
        {
            Debug.Log("Velocity : " + rb2d.velocity.y);
            rb2d.gravityScale = gravityOnGroundUp;
        }
    }

    private void RotateOnJump(float torqueAmount)
    {
        rb2d.AddTorque(torqueAmount);
    }

    private void Jump()
    {
        animator.SetBool("isJumping", true);
        rb2d.gravityScale = gravityOnJumpingUp;   //changing gravity while jumping to avoide floty jump
        rb2d.AddForce(Vector2.up * jumpForce * Time.deltaTime, ForceMode2D.Impulse);
    }

    void OnDrawGizmos()
    {
        // Draw a circle in the Scene view to visualize the ground check
        if (groundCheck == null) { return; }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
