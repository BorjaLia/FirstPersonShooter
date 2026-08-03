using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]

public class PlayerMovement : MonoBehaviour
{
    [Header("Input map")]
    [SerializeField] public InputActionReference dashActionReference;

    [Header("Camera settings")]
    [SerializeField] private Vector2 lookSensitivity = new Vector2(50.0f, 50.0f);

    [Header("Movement settings")]
    [SerializeField] private float jumpForce = 1.0f;
    [SerializeField] private float walkSpeed = 1.0f;
    [SerializeField] private float dragCoeficient = 0.75f;
    [SerializeField] private float airMovementMultiplier = 0.5f;

    [Header("Dash settings")]
    [SerializeField] private float dashForce = 15.0f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1.0f;
    [SerializeField] private float dashCoyote = 0.1f;

    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    [SerializeField] private Camera cam;
    private Rigidbody rb;

    private Vector2 inputDir = new Vector2(0.0f, 0.0f);
    private Vector2 lookDir = new Vector2(0.0f, 0.0f);

    private float currentDashCooldown = 0.0f;
    private float dashTimer = 0.0f;
    private bool isDashing = false;
    private Vector3 currentDashDirection;
    private bool queueDash = false;

    private bool queueJump = false;
    private bool isGrounded = true;

    private float xRotation = 0f;

    private IGameplayManager gameplayManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (!rb) Debug.LogError("No rigidbody found!");

        if (!cam) Debug.LogError("No camera found!");

        if (!groundCheck) Debug.LogError("No floor check found!");

        gameplayManager = ServiceLocator.Get<IGameplayManager>();
    }

    void Update()
    {
        if (gameplayManager != null && gameplayManager.IsPaused) return;

        if (dashActionReference.action.WasPressedThisFrame())
        {
            queueDash = true;
        }
    }

    void FixedUpdate()
    {
        if (gameplayManager != null && gameplayManager.IsPaused) return;

        Move();
    }

    private void LateUpdate()
    {
        if (gameplayManager != null && gameplayManager.IsPaused) return;
        Look();
    }

    private void Move()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        Dash();

        if (!isDashing)
        {
            Vector3 currentHorizontalVel = new Vector3(rb.linearVelocity.x, 0.0f, rb.linearVelocity.z);
            rb.AddForce(-currentHorizontalVel * dragCoeficient, ForceMode.VelocityChange);

            //Walk
            if (inputDir != Vector2.zero)
        {
            Vector3 dir = new Vector3(inputDir.x, 0.0f, inputDir.y);
            dir *= walkSpeed;

            if (!isGrounded) dir *= airMovementMultiplier;

            rb.AddRelativeForce(dir, ForceMode.VelocityChange);
        }
    }
        //jump

        if (queueJump && isGrounded)
        {
            rb.AddRelativeForce(new Vector3(0.0f, jumpForce, 0.0f), ForceMode.VelocityChange);
            isGrounded = false;
        }
        queueJump = false;
    }

    private void Dash()
    {
        if (currentDashCooldown > 0.0f) currentDashCooldown -= Time.fixedDeltaTime;

        if (queueDash)
        {
            if (currentDashCooldown <= dashCoyote)
            {
                isDashing = true;
                dashTimer = dashDuration;
                currentDashCooldown = dashCooldown;

                Vector3 input = new Vector3(inputDir.x, 0.0f, inputDir.y);
                Vector3 localDashDir = input != Vector3.zero ? input.normalized : Vector3.forward;

                currentDashDirection = transform.TransformDirection(localDashDir);
            }
            queueDash = false;
        }

        if (isDashing)
        {
            dashTimer -= Time.fixedDeltaTime;

            if (dashTimer <= 0.0f) isDashing = false;
            else
            {
                rb.linearVelocity = new Vector3(
                    currentDashDirection.x * dashForce,
                    rb.linearVelocity.y,
                    currentDashDirection.z * dashForce
                );
            }
        }
    }

    private void Look()
    {
        if (lookDir == Vector2.zero) return;

        transform.Rotate(transform.up, lookDir.x * lookSensitivity.x);

        xRotation -= lookDir.y * lookSensitivity.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    public void OnMove(InputValue value)
    {
        inputDir = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        lookDir = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (gameplayManager != null && gameplayManager.IsPaused) return;

        queueJump = true;
    }
}
