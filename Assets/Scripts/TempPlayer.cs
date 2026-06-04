using UnityEngine;
using UnityEngine.InputSystem;

public class TempPlayer : MonoBehaviour
{
    private const float MovementThreshold = 0.01f;
    private const float DashInputThreshold = 0.5f;

    public static TempPlayer Instance { get; private set; }
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private SpriteRenderer playerSprite; // changed to playerSprite for clarity
    [SerializeField] private Animator cowAnimator; // changed to cowAnimator for clarity
    [SerializeField] private ParticleSystem dashParticle; // changed to dashParticle for clarity
    [SerializeField] private Vector3 dashParticleRearOffset = new Vector3(-0.25f, 0f, 0f);
    // [SerializeField] private string movingParameter = "IsMoving";
    [SerializeField] private string dashingParameter = "IsDashing";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private CharacterController characterController;
    private Vector2 moveInput;

    [SerializeField] private float dashDistance = 2f;
    [SerializeField] private float dashCooldown = 0.4f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float particleTailTime = 0.12f;
    [SerializeField] private int dashCharges = 2;
    [SerializeField] private float dashRecoveryRate = 3f; // in seconds
    private float dashChargeTimer;
    private float dashTimer;
    private float dashCooldownTimer;
    private float particleTailTimer;
    private bool isDashing;
    private Vector3 dashDirection;
    private bool lastFacingRight = true;

    [SerializeField] private int health = 3;

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
        if (playerSprite == null)
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

        playerSprite.flipX = !lastFacingRight;

        if (dashParticle != null)
        {
            var shape = dashParticle.shape;
            shape.rotation = new Vector3(0f, lastFacingRight ? 0f : 180f, 0f);

            Vector3 particlePosition = dashParticle.transform.localPosition;
            particlePosition.x = lastFacingRight
                ? -Mathf.Abs(dashParticleRearOffset.x)
                : Mathf.Abs(dashParticleRearOffset.x);
            particlePosition.y = dashParticleRearOffset.y;
            particlePosition.z = dashParticleRearOffset.z;
            dashParticle.transform.localPosition = particlePosition;
        }
    }

    private void UpdateAnimatorState()
    {
        if (cowAnimator != null)
        {
            cowAnimator.SetBool(dashingParameter, isDashing);
        }
    }

    private void UpdateParticleSystem()
    {
        if (dashParticle == null)
        {
            return;
        }

        if (isDashing)
        {
            particleTailTimer = particleTailTime;
            if (!dashParticle.isPlaying)
            {
                dashParticle.Play();
            }
            return;
        }

        if (particleTailTimer > 0f)
        {
            particleTailTimer -= Time.deltaTime;
            if (!dashParticle.isPlaying)
            {
                dashParticle.Play();
            }
            return;
        }

        if (dashParticle.isPlaying)
        {
            dashParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
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
        playerSprite ??= GetComponentInChildren<SpriteRenderer>();
        cowAnimator ??= GetComponentInChildren<Animator>();
        dashParticle ??= GetComponentInChildren<ParticleSystem>();
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

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void OnDash(InputValue value)
    {
        if (value.Get<float>() > DashInputThreshold && !isDashing && dashCooldownTimer <= 0f)
        {
            StartDash();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameManager.Instance.GameOver();
        Debug.Log("Player has died.");
    }

    public int GetHealth()
    {
        return health;
    }
}
