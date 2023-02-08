using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public Transform orientation;
    public Rigidbody2D rb;
    Vector2 moveDirection;

    [Header ("Ground")]
    public float groundDrag;
    public LayerMask whatIsGround;

    public float playerHeight;
    public bool grounded;

    [Header("Jumping")]
    public KeyCode jumpKey = KeyCode.Space;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;

    bool readyToJump;

    float horizontalInput;
    float verticalInput;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        readyToJump = true;
        playerHeight = transform.lossyScale.y;

    }
    public void Update()
    {
        //Ground Check
        playerHeight = transform.lossyScale.y;
        grounded = Physics2D.Raycast(transform.position, Vector2.down, playerHeight + 0.2f, whatIsGround);
        //Debug.Log("Eagle has Landed");

        PlayerInput();
        SpeedControl();


        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }
    public void PlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (verticalInput >= 0.5f&& readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    public void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode2D.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode2D.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f);

        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, jumpForce);

        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;

    }


}
