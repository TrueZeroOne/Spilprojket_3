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

    [Header("Ground")]
    public float groundDrag;
    public LayerMask whatIsGround;

    private Vector2 playerSize;
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

    [SerializeField] private AudioClip walkAudio;

    [Header("Size Change")]
    public float rbMassBig;
    public float rbMassSmall;

    public float rbGSBig;
    public float rbGSSmall;

    private float horizontalInput;
    private float verticalInput;
    public bool grabbingPlatform;
    private TinyBig tinyBig;
    public AnimStates currentAnimState;
    
    private AudioManager audioManager;

    [SerializeField] private PlayerInput playerInput;
    private Vector2 moveDirectionInput;
    private static readonly int SizeAnimID = Animator.StringToHash("size");
    private static readonly int Speed = Animator.StringToHash("speed");
    private static readonly int JumpSpeed = Animator.StringToHash("jumpSpeed");
    private static readonly int IsGrabbed = Animator.StringToHash("isGrabbed");

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
        audioManager = GetComponent<AudioManager>();
        tb = GetComponent<TinyBig>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        readyToJump = true;
        playerSize = GetComponent<SpriteRenderer>().sprite.bounds.extents;
        playerHeight = playerSize.y;
        tinyBig = GetComponent<TinyBig>();
        if (playerInput == null) playerInput = GetComponent<PlayerInput>();
    }
    public void Update()
    {
        AnimStateMachine();
        UpdateAnimVariables();

        //Ground Check
        playerSize = GetComponent<SpriteRenderer>().sprite.bounds.extents;
        playerHeight = playerSize.y;
        grounded = Physics2D.BoxCast(transform.position, new Vector2(playerSize.x, 0.05f),0, Vector2.down, playerHeight + 0.1f, whatIsGround);
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
            if(!grabbingPlatform)
                rb.gravityScale = rbGSBig;
        }
        else
        {
            rb.mass = rbMassSmall;
            if(!grabbingPlatform)
                rb.gravityScale = rbGSSmall;
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
        if (playerInput.actions["Jump"].triggered && readyToJump && grounded && !grabbingPlatform)
        {
            readyToJump = false;
            audioManager.PlayJump();
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

            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

            // on ground
            if (grounded)
                rb.AddForce(moveDirection.normalized * (moveSpeed * 10f), ForceMode2D.Force);


            // in air
            else if (!grounded)
                rb.AddForce(moveDirection.normalized * (moveSpeed * 10f * airMultiplier), ForceMode2D.Force);
        }
        else
            rb.velocity = new Vector2(0, rb.velocity.y);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        if (tinyBig.sizeBig)
        {
            // reset y velocity
            rb.velocity = new Vector3(rb.velocity.x, jumpForceBig);
            currentAnimState = AnimStates.bigJump;
            rb.AddForce(transform.up * jumpForceBig, ForceMode2D.Impulse);
        }
        else if (!tinyBig.sizeBig)
        {
            // reset y velocity
            rb.velocity = new Vector3(rb.velocity.x, jumpForceSmall);
            currentAnimState = AnimStates.smallJump;
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
        if (horizontalInput == 0 && grounded)
        {
            if (!tinyBig.sizeBig)
            {
                currentAnimState = AnimStates.smallIdle;
                return;
            }
            else
            {
                currentAnimState = AnimStates.bigIdle;
                return;
            }
        }
        if (!tinyBig.sizeBig)
        {
            if (!grabbingPlatform)
            {

                if (!grounded)
                {
                    currentAnimState = AnimStates.smallJump;
                    return;
                }
                else
                {
                    currentAnimState = AnimStates.smallRunning;
                    return;
                }
                    
            }
            else
            {
                currentAnimState = AnimStates.smallGrabbed;
                return;
            }
        }
            
        else if (tinyBig.sizeBig)
        {
            if (!grabbingPlatform)
            {
                if (!grounded)
                {
                    currentAnimState = AnimStates.bigJump;
                    return;
                }
                else
                {
                    currentAnimState = AnimStates.bigRunning;
                    return;
                }
            }
            else
            {
                currentAnimState = AnimStates.bigGrabbed;
                return;
            }
        }
    }
    private void UpdateAnimVariables()
    {
        //print($"{rb.velocity.y} | {grounded} | {tinyBig.sizeBig}");
        if (tinyBig.sizeBig)
        {
            if (rb.velocity.y is >= -7.159f and <= 0.01f) anim.SetFloat(JumpSpeed, 0);
            else anim.SetFloat(JumpSpeed, rb.velocity.y);
        }
        else
        {
            if (rb.velocity.y is >= -4.959f and <= 0.01f) anim.SetFloat(JumpSpeed, 0);
            else anim.SetFloat(JumpSpeed, rb.velocity.y);
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Eat")) 
            print("Eat");
        anim.SetBool(IsGrabbed, grabbingPlatform);
        anim.SetFloat(Speed, rb.velocity.x);
        anim.SetFloat(SizeAnimID, tinyBig.sizeBig ? 1 : 0);
        #region OldAnimationStuff

        /*if (currentAnimState == AnimStates.bigIdle)
        {
            anim.SetFloat(SizeAnimID, 1);
            anim.SetBool(IdleAnimID, true);
            anim.SetBool(JumpAnimID, false);
        }
        else if (currentAnimState == AnimStates.smallIdle)
        {
            anim.SetFloat(SizeAnimID, 0);
            anim.SetBool(IdleAnimID, true);
            anim.SetBool(JumpAnimID, false);
        }
        else if (currentAnimState is AnimStates.smallGrabbed or AnimStates.smallRunning)
        {
            anim.SetFloat(SizeAnimID, 0);
            anim.SetBool(IdleAnimID, false);
        }
        else if (currentAnimState is AnimStates.bigGrabbed or AnimStates.bigRunning)
        {
            anim.SetFloat(SizeAnimID, 1);
            anim.SetBool(IdleAnimID, false);
        }
        else if (currentAnimState is AnimStates.smallJump)
        {
            anim.SetFloat(SizeAnimID, 0);
            anim.SetBool(IdleAnimID, false);
            anim.SetBool(JumpAnimID, true);
        }
        else if (currentAnimState is AnimStates.bigJump)
        {
            anim.SetFloat(SizeAnimID, 0);
            anim.SetBool(IdleAnimID, false);
            anim.SetBool(JumpAnimID, true);
        }*/

        #endregion
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
