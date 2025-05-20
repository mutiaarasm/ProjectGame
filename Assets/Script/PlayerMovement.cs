using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private PlayerController playerController;

    private Vector2 moveInput;

    private enum MovementState { idle = 0, run = 1, jump = 2, walk = 3 }

    [Header("Jump Settings")]
    [SerializeField] private LayerMask jumpableGround;
    private BoxCollider2D coll;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();

        playerController = new PlayerController();
    }

    private void OnEnable()
    {
        playerController.Enable();

        playerController.Movement.Move.performed += OnMovePerformed;
        playerController.Movement.Move.canceled += OnMoveCanceled;

        playerController.Movement.Jump.performed += OnJumpPerformed;
    }

    private void OnDisable()
    {
        playerController.Movement.Move.performed -= OnMovePerformed;
        playerController.Movement.Move.canceled -= OnMoveCanceled;

        playerController.Movement.Jump.performed -= OnJumpPerformed;

        playerController.Disable();
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        Jump();
    }

    private void Update()
    {
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        float absMoveX = Mathf.Abs(moveInput.x);
        float currentSpeed = 0f;

        if (absMoveX > 0.5f)
        {
            currentSpeed = runSpeed;
        }
        else if (absMoveX > 0.01f)
        {
            currentSpeed = walkSpeed;
        }
        else
        {
            currentSpeed = 0f;
        }

        Vector2 targetVelocity = new Vector2(moveInput.x * currentSpeed, rb.velocity.y);
        rb.velocity = targetVelocity;
    }

    private void UpdateAnimation()
{
    MovementState state;

    if (!IsGrounded())
    {
        // Saat di udara, langsung pakai animasi jump
        state = MovementState.jump;
    }
    else
    {
        float absMoveX = Mathf.Abs(moveInput.x);

        if (absMoveX > 0.5f)
        {
            state = MovementState.run;
        }
        else if (absMoveX > 0.01f)
        {
            state = MovementState.walk;
        }
        else
        {
            state = MovementState.idle;
        }

        if (moveInput.x > 0f)
            sprite.flipX = false;
        else if (moveInput.x < 0f)
            sprite.flipX = true;
    }

    anim.SetInteger("state", (int)state);
}


    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}
