using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class TempPlayer : MonoBehaviour
{
    public static TempPlayer Instance { get; private set; }
    [SerializeField] private float moveSpeed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private CharacterController characterController;
    private Vector2 moveInput;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
        if (characterController != null)
        {
            characterController.Move(moveDirection * moveSpeed * Time.fixedDeltaTime);
        }
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}
