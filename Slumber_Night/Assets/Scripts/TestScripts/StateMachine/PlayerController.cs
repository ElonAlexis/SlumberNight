using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class PlayerController : MonoBehaviour
{
    private StateMachine stateMachine;
    PlayerInput playerInput;
    public CharacterController characterController;

    public Animator anim; 

    //InputAction ush;
    
    public TextMeshProUGUI textMeshPro;
    private BaseState currentState;

    public Vector2 currentMovementInput; 
    public  Vector3 currentMovement;
    public Vector3 currentRunMovement; 
    public bool isMovementPressed;
    public bool isRunPressed;
    public bool isJumpPressed;
    public bool isPushPressed;
    public bool isPullingPressed;

     float groundedGravity = -9f;
    float gravity = -9f;
    public float initialJumpVelocity; 

    float maxJumpHeight = 30f; 
    float maxJumpTime = 25f;

    public bool isJumping; 
    bool isJumpAnimating; 
    public bool slowRun; 
    

   public float rotationFactorPerFrame = 0.1f;
    public float runMultiplier;



    void Awake()
    {
         if(playerInput == null)
        {
            playerInput = new PlayerInput();
        }
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>(); 

        playerInput.CharacterControls.Movement.started += OnMovementInput;
        playerInput.CharacterControls.Movement.canceled += OnMovementInput;
        playerInput.CharacterControls.Movement.performed += OnMovementInput;
        playerInput.CharacterControls.Run.started += OnRun;
        playerInput.CharacterControls.Run.canceled += OnRun;
        playerInput.CharacterControls.Jump.started += OnJump;
        playerInput.CharacterControls.Jump.canceled += OnJump;

        playerInput.CharacterControls.Push.started += OnPush;
        playerInput.CharacterControls.Push.canceled += OnPush;
        playerInput.CharacterControls.Push.performed += OnPush;

        playerInput.CharacterControls.Pull.started += OnPull;
        playerInput.CharacterControls.Pull.canceled += OnPull;

        SetupJumpVariables();
    }


    void OnMovementInput(InputAction.CallbackContext context)
    {        
        currentMovementInput = context.ReadValue<Vector2>();
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }
    void OnRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }
    void OnJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }   
    void OnPush(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            isPushPressed = true;
        }
    }
    void OnPull(InputAction.CallbackContext context)
    {
        isPullingPressed = context.ReadValueAsButton();
    }
    
    private void Start()
    {
        // Initialize the state machine and add states
        stateMachine = new StateMachine();
        stateMachine.AddStates();
        
        // Set the initial state to Idle
        stateMachine.SetState(CharacterState.Idle);

        
    }

    private void Update()
    {
        // Update the current state
        stateMachine.UpdateState();

        // Display Current State 
         textMeshPro.text = stateMachine.currentState.GetType().Name;         
        // Check for input and transition to a new state if necessary

        if (isJumpPressed)
        {
            stateMachine.SetState(CharacterState.Jumping);
        }
        else if(isMovementPressed && isRunPressed && slowRun)
        {             
            stateMachine.SetState(CharacterState.SlowRunning);      
        }
        else if (isMovementPressed && isRunPressed)
        {
            stateMachine.SetState(CharacterState.Running);               
        }
        else if (isMovementPressed)
        {
            stateMachine.SetState(CharacterState.Walking);
        }
        else
        {
            stateMachine.SetState(CharacterState.Idle);
        }

    }

    private void FixedUpdate()
    {
        HandleGravity();
    }

    void HandleGravity()
    {
        bool isFalling = currentMovement.y < 0f;

        if(characterController.isGrounded)
        { 
            currentMovement.y = groundedGravity;
            currentRunMovement.y = groundedGravity;
        }
        else if(isFalling)
        {
            float newYVelocity = currentMovement.y + gravity;
            float nextYVelocity = (currentMovement.y + newYVelocity) * 0.509f;
            currentMovement.y = nextYVelocity;
            currentRunMovement.y = newYVelocity;
        }
        else 
        {           
            float previousYVelocity = currentMovement.y; 
            float newYVelocity = currentMovement.y + (gravity * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f;
            currentMovement.y += gravity;
            currentRunMovement.y += gravity;
        }
    }

    void SetupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }


    void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    void OnDisable()
    {
       playerInput.CharacterControls.Disable();
    }
}
