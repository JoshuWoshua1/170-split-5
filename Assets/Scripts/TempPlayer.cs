using UnityEngine;
using UnityEngine.InputSystem;

public class TempPlayer : MonoBehaviour
{
    private const float MovementThreshold = 0.01f;
    private const float DashInputThreshold = 0.5f;

    public static TempPlayer Instance { get; private set; }
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private Vector3 particleRearOffset = new Vector3(-0.25f, 0f, 0f);
    // [SerializeField] private string movingParameter = "IsMoving";
    [SerializeField] private string dashingParameter = "IsDashing";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private CharacterController characterController;
    private Vector2 moveInput;

    [SerializeField] private float dashDistance = 2f;
    [SerializeField] private float dashCooldown = 0.4f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float particleTailTime = 0.12f;
    
    private float dashTimer;
    private float dashCooldownTimer;
    private float particleTailTimer;
    private bool isDashing;
    private Vector3 dashDirection;
    private bool lastFacingRight = true;

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
        InitializeReferences();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFacingDirection();
        UpdateAnimatorState();
        UpdateParticleSystem();
        UpdateDashTimer();
    }

    void FixedUpdate()
    {
        Vector3 moveDirection = GetMoveDirection();
        Vector3 velocity = GetMoveVelocity(moveDirection);

        if (characterController != null)
        {
            characterController.Move(velocity * Time.fixedDeltaTime);
        }
    }

    private void UpdateFacingDirection()
    {
        if (spriteRenderer == null)
        {
            return;
        }

        if (moveInput.x > MovementThreshold)
        {
            lastFacingRight = true;
        }
        else if (moveInput.x < -MovementThreshold)
        {
            lastFacingRight = false;
        }

        spriteRenderer.flipX = !lastFacingRight;

        if (particleSystem != null)
        {
            var shape = particleSystem.shape;
            shape.rotation = new Vector3(0f, lastFacingRight ? 0f : 180f, 0f);

            Vector3 particlePosition = particleSystem.transform.localPosition;
            particlePosition.x = lastFacingRight
                ? -Mathf.Abs(particleRearOffset.x)
                : Mathf.Abs(particleRearOffset.x);
            particlePosition.y = particleRearOffset.y;
            particlePosition.z = particleRearOffset.z;
            particleSystem.transform.localPosition = particlePosition;
        }
    }

    private void UpdateAnimatorState()
    {
        if (animator != null)
        {
            animator.SetBool(dashingParameter, isDashing);
        }
    }

    private void UpdateParticleSystem()
    {
        if (particleSystem == null)
        {
            return;
        }

        if (isDashing)
        {
            particleTailTimer = particleTailTime;
            if (!particleSystem.isPlaying)
            {
                particleSystem.Play();
            }
            return;
        }

        if (particleTailTimer > 0f)
        {
            particleTailTimer -= Time.deltaTime;
            if (!particleSystem.isPlaying)
            {
                particleSystem.Play();
            }
            return;
        }

        if (particleSystem.isPlaying)
        {
            particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    private void UpdateDashTimer()
    {
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f)
            {
                isDashing = false;
                dashCooldownTimer = dashCooldown;
            }
            return;
        }

        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }

    private void InitializeReferences()
    {
        characterController ??= GetComponent<CharacterController>();
        spriteRenderer ??= GetComponentInChildren<SpriteRenderer>();
        animator ??= GetComponentInChildren<Animator>();
        particleSystem ??= GetComponentInChildren<ParticleSystem>();
    }

    private Vector3 GetMoveDirection()
    {
        return new Vector3(moveInput.x, 0f, moveInput.y).normalized;
    }

    private Vector3 GetMoveVelocity(Vector3 moveDirection)
    {
        if (isDashing)
        {
            return dashDirection * (dashDistance / Mathf.Max(dashDuration, MovementThreshold));
        }

        return moveDirection * moveSpeed;
    }

    private void StartDash()
    {
        Vector3 inputDirection = GetMoveDirection();
        dashDirection = inputDirection.sqrMagnitude > MovementThreshold
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
        if (value.Get<float>() > DashInputThreshold && !isDashing && dashCooldownTimer <= 0f)
        {
            StartDash();
        }
    }
}
