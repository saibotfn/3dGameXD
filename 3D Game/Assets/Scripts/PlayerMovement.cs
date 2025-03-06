using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private InputSystem_Actions inputActions;
    private Stats playerStats;

    [Header("Movement Settings")]
    private float moveSpeed;
    [SerializeField] private float jumpHeight = 2.0f;
    private float gravity = -9.81f;
    private float sprintSpeed;
    public float currentSpeed;
    private Vector3 velocity;
    private Vector2 moveInput;

    [Header("Flight")]
    public bool isFlying = false;
    public float flyingSpeed = 10.0f;


    [Header("Look Settings")]
    public float sensitivity;
    private Vector2 lookInput;
    private float xRotation = 0f;

    [Header("References")]
    public Transform cameraTransform;




    private void Awake()
    {
        playerStats = GetComponent<Stats>();
        controller = GetComponent<CharacterController>();
        inputActions = new InputSystem_Actions();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentSpeed = moveSpeed;
    }

    private void Start()
    {
        moveSpeed = playerStats.speed;
        sprintSpeed = playerStats.sprintSpeed;
    }

    private void Update()
    {
        if (!isFlying)
        {
            Move();
            Sprint();
            ApplyGravity();
        }
        else
        {
            MoveFlying();
        }

    }

    private void Move()
    {
        Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(moveDirection * currentSpeed * Time.deltaTime);
    }

    private void MoveFlying()
    {
        Vector3 moveDirection = transform.right * moveInput.x + Camera.main.transform.forward * moveInput.y;
        controller.Move(moveDirection * flyingSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        if (controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void FlyUpDown(float moveDirection)
    {
        Debug.Log("yo");
        transform.position += new Vector3(0, moveDirection * flyingSpeed * Time.deltaTime, 0);
    }




    private void Sprint()
    {
        if (Keyboard.current.leftShiftKey.IsPressed())
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = moveSpeed;
        }
        
    }

    private void ApplyGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Look(Vector2 input)
    {
        if(Time.timeScale > 0f)
        {
            float sensitivity = PlayerPrefs.GetFloat("Sensitivity", 0.5f);
            float mouseX = input.x * sensitivity;
            float mouseY = input.y * sensitivity;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }

    }

    public void EnableFlight(bool flyingToggle)
    {
        if (flyingToggle == true)
        {
            isFlying = flyingToggle;
            gravity = 0f;
        }
        else
        {
            isFlying = false;
            gravity = -9.82f;
        }
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        if (isFlying == true)
        {
            inputActions.Player.FlyingUpDown.performed += ctx => FlyUpDown(ctx.ReadValue<float>());
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inputActions.Player.Look.performed += ctx => Look(ctx.ReadValue<Vector2>());
        inputActions.Player.Jump.performed += _ => Jump();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }


}