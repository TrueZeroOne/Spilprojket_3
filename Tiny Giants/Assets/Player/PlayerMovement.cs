using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public Transform orientation;
    public Rigidbody2D rb;
    private Vector2 moveDirection;
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

    private bool readyToJump;

    [Header("Animations")]
    public Animator anim;

    [Header("Size Change")]
    public float rbMassBig;
    public float rbMassSmall;

    [Header("Audio")]
    [SerializeField] private AudioClip jumpAudio;

    private float horizontalInput;
    private float verticalInput;
    public bool grabbingPlatform;
    private TinyBig tinyBig;
    public AnimStates currentAnimState;

    [SerializeField] private PlayerInput playerInput;
    private Vector2 moveDirectionInput;
    private static readonly int Size = Animator.StringToHash("Size");
    private static readonly int Idle = Animator.StringToHash("Idle");

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
        //UpdateAnimVariables();

        //Ground Check
        playerHeight = GetComponent<SpriteRenderer>().sprite.bounds.extents.y;
        grounded = Physics2D.Raycast(transform.position, Vector2.down, playerHeight + 0.05f, whatIsGround);
        //Debug.Log("Eagle has Landed");

        //AnimStateMachine();
        PlayerInput();
        SpeedControl();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        if (tinyBig.sizeBig)
        {
            rb.mass = rbMassBig;
        }
        else
        {
            rb.mass = rbMassSmall;
        }
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
        if (!grabbingPlatform)
        {
            if (horizontalInput < 0)
                GetComponent<SpriteRenderer>().flipX = true;
            else if (horizontalInput > 0)
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
            rb.velocity = new Vector2(0,rb.velocity.y);
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
            //anim.Play("JumpBig");
            rb.AddForce(transform.up * jumpForceBig, ForceMode2D.Impulse);
        }
        else if (!tinyBig.sizeBig)
        {
            // reset y velocity
            rb.velocity = new Vector3(rb.velocity.x, jumpForceSmall);
            //anim.Play("JumpSmall");
            rb.AddForce(transform.up * jumpForceSmall, ForceMode2D.Impulse);
        } 
    }
    private void ResetJump()
    {
        readyToJump = true;
    }

    public AnimStates GetAnimationState() => currentAnimState;

    private void AnimStateMachine()
    {
        if (!tinyBig.sizeBig)
            if (horizontalInput != 0)
                if (!grabbingPlatform)
                    if (!grounded)
                        currentAnimState = AnimStates.smallJump;
                    else
                        currentAnimState = AnimStates.smallRunning;
                else
                    currentAnimState = AnimStates.smallGrabbed;
            else if (horizontalInput != 0 && grounded) currentAnimState = AnimStates.smallIdle;
        else if (tinyBig.sizeBig) 
            if (horizontalInput != 0)
                if (!grabbingPlatform)
                    if (!grounded)
                        currentAnimState = AnimStates.bigJump;
                    else
                        currentAnimState = AnimStates.bigRunning;
                else
                    currentAnimState = AnimStates.bigGrabbed;
            else if (horizontalInput != 0 && grounded)
                currentAnimState = AnimStates.bigIdle;
    }
    private void UpdateAnimVariables()
    {

        if (currentAnimState == AnimStates.bigIdle)
        {
            anim.SetFloat(Size, 1);
            anim.SetBool(Idle, true);
        }
        else if (currentAnimState == AnimStates.smallIdle)
        {
            anim.SetFloat(Size, 0);
            anim.SetBool(Idle, true);
        }
        else if (currentAnimState is AnimStates.smallJump or AnimStates.smallGrabbed or AnimStates.smallRunning)
        {
            anim.SetFloat(Size, 0);
            anim.SetBool(Idle, false);
        }
        else if (currentAnimState is AnimStates.bigJump or AnimStates.bigGrabbed or AnimStates.bigRunning)
        {
            anim.SetFloat(Size, 1);
            anim.SetBool(Idle, false);
        }
    }
}

public enum AnimStates
{
    smallIdle,
    bigIdle,
    smallRunning,
    bigRunning,
    smallGrabbed,
    bigGrabbed,
    smallJump,
    bigJump
}
