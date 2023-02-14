using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public Transform orientation;
    public Rigidbody2D rb;
    Vector2 moveDirection;
    public TinyBig tb;

    [Header ("Ground")]
    public float groundDrag;
    public LayerMask whatIsGround;

    public float playerHeight;
    public bool grounded;

    [Header("Jumping")]
    public KeyCode jumpKey = KeyCode.Space;
    public float jumpForceBig;
    public float jumpForceSmall;
    public float jumpCooldown;
    public float airMultiplier;

    bool readyToJump;

    [Header("Animations")]
    public Animator anim;
    public Animation smallWalk;
    public Animation bigWalk;
    public Animation smallJumping;
    public Animation bigJumping;

    [Header("Audio")]
    [SerializeField] private AudioClip jumpAudio;

    float horizontalInput;
    float verticalInput;
    public bool grabbingPlatform = false;
    private TinyBig tinyBig;

    [SerializeField] private PlayerInput playerInput;
    private Vector2 moveDirectionInput;

    private void OnEnable()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.actions.Enable();
    }

    private void OnDisable()
    {
        playerInput.actions.Disable();
    }

    public void Start()
    {
        tb = GetComponent<TinyBig>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        readyToJump = true;
        playerHeight = GetComponent<SpriteRenderer>().sprite.bounds.extents.y;
        tinyBig = GetComponent<TinyBig>();
        if (playerInput == null) playerInput = GetComponent<PlayerInput>();
    }
    public void Update()
    {
        //Ground Check
        playerHeight = GetComponent<SpriteRenderer>().sprite.bounds.extents.y;
        grounded = Physics2D.Raycast(transform.position, Vector2.down, playerHeight + 0.05f, whatIsGround);
        //Debug.Log("Eagle has Landed");

        PlayerInput();
        SpeedControl();

        if (!tinyBig.sizeBig)
        {
            smallWalk.Play();
        }

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
        // horizontalInput = Input.GetAxisRaw("Horizontal");
        // verticalInput = Input.GetAxisRaw("Vertical");
        Vector2 moveDirectionInput = playerInput.actions["Move"].ReadValue<Vector2>();
        
        horizontalInput = moveDirectionInput.x;
        verticalInput = moveDirectionInput.y;

        // when to jump
        if (playerInput.actions["Jump"].triggered && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    public void MovePlayer()
    {
        
        if (horizontalInput < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (horizontalInput > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        if (!grabbingPlatform)
        {
            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

            // on ground
            if (grounded)
                rb.AddForce(moveDirection.normalized * (moveSpeed * 10f), ForceMode2D.Force);
                                           
            // in air
            else if (!grounded)
                rb.AddForce(moveDirection.normalized * (moveSpeed * 10f * airMultiplier), ForceMode2D.Force);
        }
        else
        {
            rb.velocity = new Vector2(0,rb.velocity.y);
        }
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
        GetComponent<AudioSource>().clip = jumpAudio;
        GetComponent<AudioSource>().Play();
        if (tinyBig.sizeBig)
        {
            // reset y velocity
            rb.velocity = new Vector3(rb.velocity.x, jumpForceBig);

            rb.AddForce(transform.up * jumpForceBig, ForceMode2D.Impulse);
        }
        else if (!tinyBig.sizeBig)
        {
            // reset y velocity
            rb.velocity = new Vector3(rb.velocity.x, jumpForceSmall);

            rb.AddForce(transform.up * jumpForceSmall, ForceMode2D.Impulse);
        } 
    }
    private void ResetJump()
    {
        readyToJump = true;
    }


}
