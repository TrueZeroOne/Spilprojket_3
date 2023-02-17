using System;
using Unity.VisualScripting;
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

    public float rbGSBig;
    public float rbGSSmall;

    [Header("Audio")]
    [SerializeField] private AudioClip jumpAudio;
    [SerializeField] private AudioClip walkAudio;

    private float horizontalInput;
    private float verticalInput;
    public bool grabbingPlatform;
    private TinyBig tinyBig;
    public AnimStates currentAnimState;

    [SerializeField] private PlayerInput playerInput;
    private Vector2 moveDirectionInput;
    private static readonly int SizeAnimID = Animator.StringToHash("Size");
    private static readonly int IdleAnimID = Animator.StringToHash("Idle");
    private static readonly int JumpAnimID = Animator.StringToHash("Jump");
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
        AnimStateMachine();
        UpdateAnimVariables();

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
        if (playerInput.actions["Jump"].triggered && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
        AudioSource AS = GetComponent<AudioSource>();
        if (grounded &&horizontalInput < -0.05  || horizontalInput > 0.05 && grounded)
        {
            AS.clip = walkAudio;
            AS.volume = 0.2f;
            if (!AS.isPlaying)
                AS.Play();
        }
        else
        {
            AS.Stop();
            AS.volume = 1;
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
        GetComponent<AudioSource>().clip = jumpAudio;
        GetComponent<AudioSource>().Play();
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
            if (rb.velocity.y is >= -7.159f and <= 0.01f)
            {
                anim.SetFloat(JumpSpeed, 0);
            }
            else
            {
                anim.SetFloat(JumpSpeed, rb.velocity.y);
            }
        }
        else
        {
            if (rb.velocity.y is >= -4.959f and <= 0.01f)
            {
                anim.SetFloat(JumpSpeed, 0);
            }
            else
            {
                anim.SetFloat(JumpSpeed, rb.velocity.y);
            }
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
