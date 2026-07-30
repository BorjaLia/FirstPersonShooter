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
    [SerializeField] private float dashForce = 1.0f;
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
    private bool queueDash = false;

    private bool queueJump = false;
    private bool isGrounded = true;

    private float xRotation = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (!rb) Debug.LogError("No rigidbody found!");

        if (!cam) Debug.LogError("No camera found!");

        if (!groundCheck) Debug.LogError("No floor check found!");
    }

    void Update()
    {
        if (dashActionReference.action.WasPressedThisFrame())
        {
            queueDash = true;
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        Look();
    }

    private void Move()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);


        //Walk
        if (inputDir != Vector2.zero)
        {
            float currentSpeed = walkSpeed;

            Vector3 dir = new Vector3(inputDir.x, 0.0f, inputDir.y);    

            dir *= currentSpeed;
            //dir.y = rb.linearVelocity.y;

            //rb.linearVelocity = Vector3.zero;

            rb.AddForce(-(new Vector3(rb.linearVelocity.x, 0.0f, rb.linearVelocity.z) * dragCoeficient),ForceMode.VelocityChange);

            Dash(dir);

            if (!isGrounded) dir *= airMovementMultiplier;

            rb.AddRelativeForce(dir, ForceMode.VelocityChange);
        }

        //jump

        if (queueJump && isGrounded)
        {
            rb.AddRelativeForce(new Vector3(0.0f, jumpForce, 0.0f), ForceMode.VelocityChange);
            isGrounded = false;
        }
        queueJump = false;
    }

    private void Dash(Vector3 dir)
    {
       if (currentDashCooldown > 0.0f)
        {
            currentDashCooldown -= Time.deltaTime;
            if (currentDashCooldown < 0.0f)
            {
                currentDashCooldown = 0.0f;
                Debug.Log("Dash Ready");
            }
            if (currentDashCooldown >= dashCoyote) queueDash = false;
            return;
        }

        if (queueDash)
        {
            rb.AddRelativeForce(new Vector3(dir.x * dashForce, 0.0f,dir.z * dashForce), ForceMode.VelocityChange);
            currentDashCooldown = dashCooldown;
            queueDash = false;
        }
    }

    private void Look()
    {
        if (lookDir == Vector2.zero) return;

        transform.Rotate(transform.up, lookDir.x * lookSensitivity.x * Time.deltaTime);

        xRotation -= lookDir.y * lookSensitivity.y * Time.deltaTime;
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
        queueJump = true;
    }
}
