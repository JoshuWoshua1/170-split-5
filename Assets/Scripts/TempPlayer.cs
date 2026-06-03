using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class TempPlayer : MonoBehaviour
{
    public static TempPlayer Instance { get; private set; }
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private string dashingParameter = "IsDashing";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private CharacterController characterController;
    private Vector2 moveInput;

    [SerializeField] private float dashDistance = 2f;
    [SerializeField] private float dashCooldown = 0.4f;
    [SerializeField] private float dashDuration = 0.15f;
    
    private float dashTimer;
    private float dashCooldownTimer;
    private bool isDashing;
    private Vector3 dashDirection;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple instances of TempPlayer detected. Destroying duplicate.");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // bool isMoving = moveInput.sqrMagnitude > 0.01f;

        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = moveInput.x < 0f;
        }

        if (animator != null)
        {
            animator.SetBool(dashingParameter, isDashing);
        }

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f)
            {
                isDashing = false;
                dashCooldownTimer = dashCooldown;
            }
        }
        else if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
        Vector3 velocity = moveDirection * moveSpeed;

        if (isDashing)
        {
            velocity = dashDirection * (dashDistance / Mathf.Max(dashDuration, 0.01f));
        }

        if (characterController != null)
        {
            characterController.Move(velocity * Time.fixedDeltaTime);
        }
    }

    private void StartDash()
    {
        Vector3 inputDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        dashDirection = inputDirection.sqrMagnitude > 0.01f
            ? inputDirection.normalized
            : transform.forward;

        isDashing = true;
        dashTimer = dashDuration;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnDash(InputValue value)
    {
        if (value.Get<float>() > 0.5f && !isDashing && dashCooldownTimer <= 0f)
        {
            StartDash();
        }
    }
}
