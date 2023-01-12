using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{

    //Declare reference variables 
    PlayerInput playerInput;
    CharacterController characterController;
    Animator anim; 

    // Variables 
    Vector2 currentMovementInput; 
    Vector3 currentMovement;
    Vector3 currentRunMovement; 
    //Vector3 slideMovement; 
    bool isMovementPressed;
    bool isRunPressed;
    bool isSlidePressed; 

    [SerializeField]
    float rotationFactorPerFrame = 30f;
    [SerializeField]
    float playerSpeed = 5f;
    float runMultiplier = 3f;
    float groundedGravity = -0.05f;
    float gravity = -0.5f;

    // Variables to store optimised setter/getter parameter ID's
    int isWalkingHash;
    int isRunningHash; 
    int isIdleHash; 
    int isSlideHash;




    // Start is called before the first frame update
    void Awake()
    {
        if(playerInput == null)
        {
            playerInput = new PlayerInput();
        }
        
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>(); 

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isIdleHash = Animator.StringToHash("isIdle");
        isSlideHash = Animator.StringToHash("isSliding");

        playerInput.CharacterControls.Movement.started += OnMovementInput;
        playerInput.CharacterControls.Movement.canceled += OnMovementInput;
        playerInput.CharacterControls.Movement.performed += OnMovementInput;
        playerInput.CharacterControls.Run.started += OnRun;
        playerInput.CharacterControls.Run.canceled += OnRun;
        playerInput.CharacterControls.RunningSlide.started += OnSlide;
        playerInput.CharacterControls.RunningSlide.canceled += OnSlide;


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

        if(isMovementPressed || isSlidePressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame );
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

    void HandleAnimation()
    {
        bool isWalking = anim.GetBool(isWalkingHash);
        bool isRunning = anim.GetBool(isRunningHash);

        if(isMovementPressed && !isWalking)
        {
            anim.SetBool(isWalkingHash, true);
            anim.SetBool(isIdleHash, false);
            anim.SetBool(isRunningHash, false);
            anim.SetBool(isSlideHash, false);

        }
        else if(!isMovementPressed && isWalking)
        {
            anim.SetBool(isIdleHash, true);
            anim.SetBool(isWalkingHash, false);
            anim.SetBool(isRunningHash, false);
            anim.SetBool(isSlideHash, false);

        }

        if(isMovementPressed && isRunPressed)
        {
            anim.SetBool(isRunningHash, true);
            anim.SetBool(isWalkingHash, false);
            anim.SetBool(isIdleHash, false);
            anim.SetBool(isSlideHash, false);
           
        }
        else if((!isMovementPressed || !isRunning) && isRunning)
        {
            anim.SetBool(isIdleHash, true);
            anim.SetBool(isWalkingHash, false);
            anim.SetBool(isRunningHash, false);
            anim.SetBool(isSlideHash, false);
        }

        if (isMovementPressed && isSlidePressed)
        {
            anim.SetBool(isSlideHash, true);
            anim.SetBool(isRunningHash, false);
            anim.SetBool(isWalkingHash, false);
            anim.SetBool(isIdleHash, false);
            characterController.height = 0.5f;
        }

        if(!isSlidePressed)
        {
            characterController.height = 1.45f;
            
        }




    }

    void HandleGravity()
    {
        if( characterController.isGrounded)
        {            
            currentMovement.y = groundedGravity;
            currentRunMovement.y = groundedGravity;
        }
        else 
        {            
            currentMovement.y += gravity;
            currentRunMovement.y += gravity;
        }
    }



    // Update is called once per frame
    void Update()
    {
        if(isRunPressed)
        {
            characterController.Move(currentRunMovement * Time.deltaTime * playerSpeed);
        }
        else
        {
            characterController.Move(currentMovement * Time.deltaTime * playerSpeed);
        }
        
        HandleAnimation();
        HandleRotation();
        HandleGravity();

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
