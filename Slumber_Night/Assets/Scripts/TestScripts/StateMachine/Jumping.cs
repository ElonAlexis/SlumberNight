using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class Jumping : BaseState
    {
        
        public override void EnterState()
        {
            // Code to execute when entering the Jumping state
            Debug.Log("Entering Jumping state");
            playerController = GameObject.FindObjectOfType<PlayerController>();
            playerController.anim.Play("Jump");
        }

        public override void UpdateState()
        {
            // Code to execute while in the Jumping state
          //  Debug.Log("Jumping state");
            HandleJump();
        }

        public override void ExitState()
        {
            // Code to execute when exiting the Jumping state
          //  Debug.Log("Exiting Jumping state");
        }

        void HandleJump()
    {
        if(!playerController.isJumping && playerController.characterController.isGrounded && playerController.isJumpPressed)
        {
            playerController.isJumping = true;            
            playerController.currentMovement.y = playerController.initialJumpVelocity * 1.5f;
            playerController.currentRunMovement.y = playerController.initialJumpVelocity * 1.5f;
        }
        else if(playerController.isJumping && playerController.characterController.isGrounded && !playerController.isJumpPressed)
        {
            Debug.Log("Landed");
            playerController.isJumping = false;
        }
    }

       
    }

