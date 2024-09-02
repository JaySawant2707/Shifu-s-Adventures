using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Playercontroller : MonoBehaviour
{
    [SerializeField] float torqueAmount = 1f;
    [SerializeField] float jumpForce = 1000f;
    [SerializeField] float speedOnGoingUp = 30f;
    [SerializeField] float gravityOnGrounded = 10f;
    [SerializeField] float gravityOnJumpingDown = 10f;
    [SerializeField] float gravityOnJumpingUp = 10f;
    [SerializeField] Transform groundCheck;  // Reference to the GroundCheck GameObject
    [SerializeField] LayerMask groundLayer;  // LayerMask to specify what is considered ground

    SurfaceEffector2D surfaceEffector2D;
    Rigidbody2D rb2d;
    Animator animator;

    public bool isGrounded = false;
    float groundCheckRadius = 0.2f; //radius of circle that check groundcheck

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        surfaceEffector2D = FindObjectOfType<SurfaceEffector2D>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer); //check if player is grounded or not 

        if (isGrounded)
        {
            animator.SetBool("isJumping", false);
            rb2d.gravityScale = gravityOnGrounded;
        }
        else
        {
            animator.SetBool("isJumping", true);
        }


        if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetMouseButton(0)) && isGrounded)
        {
            Jump();
        }

        RotatePlayer();

        JumpPhysics();

        SpeedPhysics();
    }

    void SpeedPhysics()
    {
        if (rb2d.velocity.y > 0f && isGrounded)
        {
            surfaceEffector2D.speed = speedOnGoingUp;
        }
        else if (rb2d.velocity.y < 0f && isGrounded)
        {
            surfaceEffector2D.speed = 30f;
        }
    }

    void JumpPhysics()
    {
        if (rb2d.velocity.y < 0f && !isGrounded)
        {
            rb2d.gravityScale = gravityOnJumpingDown;
        }
        else if (rb2d.velocity.y > 0f && !isGrounded)
        {
            rb2d.gravityScale = gravityOnJumpingUp;
        }
    }

    void RotatePlayer()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            rb2d.AddTorque(torqueAmount);
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            rb2d.AddTorque(-torqueAmount);
    }

    void Jump()
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
