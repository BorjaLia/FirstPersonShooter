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

    [SerializeField] private Camera cam;
    private Rigidbody rb;

    private Vector2 inputDir = new Vector2(0.0f,0.0f);
    private Vector2 lookDir = new Vector2(0.0f,0.0f);
    private bool queueJump = false;

    private bool isRunning = true;
    private bool isGrounded = true;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (!rb) Debug.LogError("No rigidbody found!");
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Look();
        Move();
    }

    private void Move()
    {
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        //Walk
        Vector3 dir = new Vector3(inputDir.x, 0.0f, inputDir.y);
        dir *= currentSpeed;
        if (inputDir != Vector2.zero) rb.AddRelativeForce(dir,ForceMode.Acceleration);

        //jump
        if (queueJump && isGrounded) rb.AddRelativeForce(new Vector3(0.0f,jumpForce,0.0f),ForceMode.VelocityChange);
    }

    private void Look()
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
