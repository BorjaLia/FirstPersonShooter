using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement settings")]
    [SerializeField] private float jumpForce = 1.0f;
    [SerializeField] private float walkSpeed = 1.0f;
    [SerializeField] private float runSpeed = 1.0f;
    [SerializeField] private Vector2 lookSensitivity = new Vector2 (50.0f, 50.0f);

    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    [SerializeField] private Camera cam;
    private Rigidbody rb;

    private Vector2 inputDir = new Vector2(0.0f,0.0f);
    private Vector2 lookDir = new Vector2(0.0f,0.0f);
    private bool queueJump = false;

    private bool isRunning = true;
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
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        //Walk
        //Vector3 dir = new Vector3(inputDir.x, 0.0f, inputDir.y);
        Vector3 dir = new Vector3(inputDir.x, 0.0f, inputDir.y);
        dir *= currentSpeed;
        //if (inputDir != Vector2.zero) rb.AddRelativeForce(dir, ForceMode.Acceleration);
        if (inputDir != Vector2.zero)
        {
            dir.y = rb.linearVelocity.y;
            rb.linearVelocity = Vector3.zero;
            if (inputDir != Vector2.zero) rb.AddRelativeForce(dir,ForceMode.VelocityChange);
        }

        //jump
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        if (queueJump && isGrounded)
        {
            rb.AddRelativeForce(new Vector3(0.0f, jumpForce, 0.0f), ForceMode.VelocityChange);
            queueJump = false;
            isGrounded = false;
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

    private void CheckGround()
    {

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
