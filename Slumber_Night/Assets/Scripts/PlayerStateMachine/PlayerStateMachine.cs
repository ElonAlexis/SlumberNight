using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    //Declare reference variables 
    PlayerInput playerInput;
    CharacterController characterController;
    Animator anim;

    PlayerBaseState _currentState;
    PlayerStateFactory _states;

    public PlayerBaseState CurrentState {get {return _currentState;} set{_currentState = value;}}

    // Variables 
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    Vector3 appliedMovement;

    Vector3 forwardDirection;
    //Vector3 slideMovement; 
    bool isMovementPressed;
    bool isRunPressed;
    bool isSlidePressed;

    float rotationFactorPerFrame = 0.1f;
    float playerSpeed = 5f;
    float runMultiplier = 3f;
    //float groundedGravity = -0.05f;
    //float gravity = -0.03f;

    // Variables to store optimised setter/getter parameter ID's for animations
    int isWalkingHash;
    int isRunningHash;
    int isIdleHash;
    int isSlideHash;
    int isFalling;

    void Awake()
    {
        if (playerInput == null)
        {
            playerInput = new PlayerInput();
        }

        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();

        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isIdleHash = Animator.StringToHash("isIdle");
        isSlideHash = Animator.StringToHash("isSliding");
        isFalling = Animator.StringToHash("isFalling");

        playerInput.CharacterControls.Movement.started += OnMovementInput;
        playerInput.CharacterControls.Movement.canceled += OnMovementInput;
        playerInput.CharacterControls.Movement.performed += OnMovementInput;
        playerInput.CharacterControls.Run.started += OnRun;
        playerInput.CharacterControls.Run.canceled += OnRun;
        playerInput.CharacterControls.RunningSlide.started += OnSlide;
        playerInput.CharacterControls.RunningSlide.canceled += OnSlide;



    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleRotation();
        _currentState.Updatestate();
        characterController.Move(appliedMovement * playerSpeed * Time.deltaTime);
    }

    void OnRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    void OnSlide(InputAction.CallbackContext context)
    {
        isSlidePressed = context.ReadValueAsButton();
    }

    void HandleRotation()
    {
        Vector3 positionToLookAt;
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0f;
        positionToLookAt.z = currentMovement.z;

        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed || isSlidePressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame);
        }
    }

    void OnMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;

        currentRunMovement.x = currentMovementInput.x * runMultiplier;
        currentRunMovement.z = currentMovementInput.y * runMultiplier;



        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
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
