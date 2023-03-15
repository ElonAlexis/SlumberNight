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
    Vector3 appliedMovement; 
    Vector3 forwardDirection;
    //Vector3 slideMovement; 
    bool isMovementPressed;
    bool isRunPressed;
    bool isSlidePressed; 
    bool isJumpPressed;
    public bool isJumping; 
    bool isJumpAnimating; 

    float rotationFactorPerFrame = 0.1f;
    float playerSpeed = 7f;
    float runMultiplier = 3f;
    float groundedGravity = -9f;
    float gravity = -9f;
    float initialJumpVelocity; 
    float maxJumpHeight = 30f; 
    float maxJumpTime = 25f;


    // Variables to store optimised setter/getter parameter ID's for animations
    int isWalkingHash;
    int isRunningHash; 
    int isIdleHash; 
    int isSlideHash;
    int isFallingHash;
    int isJumpingHash;
    int isRunJumpingHash; 




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
        isFallingHash = Animator.StringToHash("isFalling");
        isJumpingHash = Animator.StringToHash("isJumping");
        isRunJumpingHash = Animator.StringToHash("isRunJump");

        playerInput.CharacterControls.Movement.started += OnMovementInput;
        playerInput.CharacterControls.Movement.canceled += OnMovementInput;
        playerInput.CharacterControls.Movement.performed += OnMovementInput;
        playerInput.CharacterControls.Run.started += OnRun;
        playerInput.CharacterControls.Run.canceled += OnRun;
        playerInput.CharacterControls.RunningSlide.started += OnSlide;
        playerInput.CharacterControls.RunningSlide.canceled += OnSlide;
        playerInput.CharacterControls.Jump.started += OnJump;
        playerInput.CharacterControls.Jump.canceled += OnJump;

        SetupJumpVariables();
    }
    void SetupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }
    void HandleJump()
    {
        if(!isJumping && characterController.isGrounded && isJumpPressed)
        {
            isJumping = true;            
            isJumpAnimating = true;
            currentMovement.y = initialJumpVelocity * 1.5f;
            currentRunMovement.y = initialJumpVelocity * 1.5f;
        }
        else if(isJumping && characterController.isGrounded && !isJumpPressed)
        {
            Debug.Log("Landed");
            isJumping = false;
        }
    }

    void OnJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
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
        currentMovement.x =  currentMovementInput.x;
        currentMovement.z =  currentMovementInput.y;

        currentRunMovement.x = currentMovementInput.x * runMultiplier;
        currentRunMovement.z = currentMovementInput.y * runMultiplier;

        

        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }

    void HandleAnimation()
    {
        bool isWalking = anim.GetBool(isWalkingHash);
        bool isRunning = anim.GetBool(isRunningHash);

            if (!isMovementPressed)
            {
                anim.SetBool(isIdleHash, true);
                anim.SetBool(isWalkingHash, false);
                anim.SetBool(isRunningHash, false);
                anim.SetBool(isSlideHash, false);
                anim.SetBool(isFallingHash, false);
                anim.SetBool(isJumpingHash, false);
                anim.SetBool(isRunJumpingHash, false);

        }
            else if (isMovementPressed && !isWalking)
            {
                anim.SetBool(isWalkingHash, true);
                anim.SetBool(isIdleHash, false);
                anim.SetBool(isRunningHash, false);
                anim.SetBool(isSlideHash, false);
                anim.SetBool(isFallingHash, false);
                anim.SetBool(isJumpingHash, false);
                anim.SetBool(isRunJumpingHash, false);

        }      
            
            if (isMovementPressed && isRunPressed)
            {
                anim.SetBool(isRunningHash, true);
                anim.SetBool(isWalkingHash, false);
                anim.SetBool(isIdleHash, false);
                anim.SetBool(isSlideHash, false);
                anim.SetBool(isFallingHash, false);
                anim.SetBool(isJumpingHash, false);
                anim.SetBool(isRunJumpingHash, false);


                if (isSlidePressed)
                {
                    anim.SetBool(isSlideHash, true);
                    anim.SetBool(isRunningHash, false);
                    anim.SetBool(isWalkingHash, false);
                    anim.SetBool(isIdleHash, false);
                    anim.SetBool(isFallingHash, false);
                    anim.SetBool(isJumpingHash, false);
                    anim.SetBool(isRunJumpingHash, false);

            }
                if(isJumpPressed)
                {
                    anim.SetBool(isRunJumpingHash, true);
                    anim.SetBool(isSlideHash, false);
                    anim.SetBool(isRunningHash, false);
                    anim.SetBool(isWalkingHash, false);
                    anim.SetBool(isIdleHash, false);
                    anim.SetBool(isFallingHash, false);
                    anim.SetBool(isJumpingHash, false);
                }
            }
            if(isJumping)
            {
                anim.SetBool(isJumpingHash, true);
                anim.SetBool(isIdleHash, false);
                anim.SetBool(isWalkingHash, false);
                anim.SetBool(isRunningHash, false);
                anim.SetBool(isSlideHash, false);
                anim.SetBool(isFallingHash, false);
            }
    }

    void HandleGravity()
    {
        bool isFalling = currentMovement.y < 0f;


        if( characterController.isGrounded)
        {         
            if(isJumpAnimating)
            {
                anim.SetBool(isJumpingHash, false);
            }  
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



    // Update is called once per frame
    void FixedUpdate()
    {
        forwardDirection = transform.forward;
        //characterController.Move(appliedMovement * playerSpeed * Time.deltaTime);

        HandleAnimation();
        HandleRotation();
        PlayerMove();
        Slide();
        HandleGravity();
        HandleJump();

    }

    void update()
    {
        
    }

    void PlayerMove() 
    {
        if (isRunPressed)
        {
           // appliedMovement.x = currentRunMovement.x;
          //  appliedMovement.z = currentRunMovement.z;
          characterController.Move(currentRunMovement * Time.deltaTime * playerSpeed);
        }
        else
        {
            // appliedMovement.x = currentMovement.x;
            //  appliedMovement.z = currentMovement.z;
            characterController.Move(currentMovement * Time.deltaTime * playerSpeed);

        }

       
    }
    void Slide()
    {
        if (isSlidePressed)
        {
            
            currentMovement = forwardDirection;
            currentMovement = Vector3.ClampMagnitude(currentMovement,playerSpeed);
            characterController.height = 1f;
        }
        else
        {
            characterController.height = 1.45f;
        }
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
